using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ContentSync
{
    public class Program
    {
        // Should always be Content?
        private const string ContentFolder = @"Content\";
        private static readonly XNamespace Xmlns = "http://schemas.microsoft.com/developer/msbuild/2003";

        public static void Main(string[] args)
        {
            try
            {
                Console.Write("ContentSync: ");

                if (args.Length != 2)
                {
                    Console.WriteLine("Expected arguments source and destination projects file path");
                    return;
                }

                var source = args[0];
                var destination = args[1];

                if (!File.Exists(source))
                {
                    Console.WriteLine("Source project file not found: " + source);
                    return;
                }

                if (!File.Exists(destination))
                {
                    Console.WriteLine("Destination project file not found: " + destination);
                    return;
                }

                Console.WriteLine(source + " to " + destination);

                var destinationDocument = XDocument.Load(destination);

                var sourceContentFiles = GetContentFiles(source).ToList();
                var destinationContentFiles = GetLinkedContentFiles(destinationDocument).ToList();

                var relativePathFromSourceToDestination = GetRelativePathFromSourceToDestination(source, destination);

                var hasChanges = AddMissingContents(sourceContentFiles, destinationContentFiles, destinationDocument, relativePathFromSourceToDestination);

                hasChanges |= RemoveExtraContent(sourceContentFiles, destinationContentFiles, destinationDocument);

                // Check conformity of existing content

                if (hasChanges)
                {
                    Console.WriteLine("Saving the updated project file " + destination);
                    //destinationDocument.Save(destination);
                }
                else
                {
                    Console.WriteLine("No changes detected");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static IEnumerable<XElement> GetContentFiles(string projectPath)
        {
            var projectDocument = XDocument.Load(projectPath);

            return GetContentFiles(projectDocument);
        }

        public static IEnumerable<XElement> GetLinkedContentFiles(string projectPath)
        {
            var projectDocument = XDocument.Load(projectPath);

            return GetLinkedContentFiles(projectDocument);
        }

        public static IEnumerable<XElement> GetLinkedContentFiles(XDocument projectDocument)
        {
            return projectDocument.Descendants(Xmlns + "Content")
                .Where(x => GetLinkPath(x).StartsWith(ContentFolder));
        }

        public static string GetRelativePathFromSourceToDestination(string sourceProjectPath,
            string destinationProjectPath)
        {
            var sourceProjectContentFolder = Path.GetDirectoryName(sourceProjectPath);
            var destinationProjectContentFolder = Path.GetDirectoryName(destinationProjectPath);

            var file = new Uri(sourceProjectContentFolder);

            // Must end in a slash to indicate folder
            var folder = new Uri(destinationProjectContentFolder + Path.DirectorySeparatorChar);

            return Uri.UnescapeDataString(folder.MakeRelativeUri(file).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public static bool AddMissingContents(List<XElement> sourceContentFiles, List<XElement> destinationContentFiles,
            XDocument destinationDocument, string relativePathFromSourceToDestination)
        {
            var hasChanges = false;

            var missingContentFiles = sourceContentFiles.Select(GetContentPath)
                .Except(destinationContentFiles.Select(GetLinkPath))
                .ToList();

            if (missingContentFiles.Any())
            {
                hasChanges = true;
                //        <Content Include="..\SamplesBrowser\Content\Sandbox\HexSheet.png">
                //          <Link>Content\Sandbox\HexSheet.png</Link>
                //          <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
                //        </Content>

                foreach (var missingContentFile in missingContentFiles)
                {
                    Console.WriteLine("Adding missing content file " + missingContentFile);
                    destinationDocument.Root.Add(
                        new XElement(Xmlns + "ItemGroup",
                            new XElement(Xmlns + "Content",
                                new XAttribute("Include", Path.Combine(relativePathFromSourceToDestination, missingContentFile)),
                                new XElement(Xmlns + "Link", missingContentFile),
                                new XElement(Xmlns + "CopyToOutputDirectory", "PreserveNewest"))));
                }
            }

            return hasChanges;
        }

        public static bool RemoveExtraContent(List<XElement> sourceContentFiles, List<XElement> destinationContentFiles,
            XDocument destinationDocument)
        {
            var hasChanges = false;

            var extraContentFiles = destinationContentFiles.Select(GetLinkPath)
                .Except(sourceContentFiles.Select(GetContentPath))
                .ToList();

            if (extraContentFiles.Any())
            {
                hasChanges = true;

                foreach (var extraContentFile in extraContentFiles)
                {
                    Console.WriteLine("Remove extra content file " + extraContentFile);

                    var extraElement = destinationDocument.Descendants(Xmlns + "Link").Single(x => x.Value == extraContentFile).Parent;

                    extraElement.Remove();
                }
            }

            return hasChanges;
        }

        private static IEnumerable<XElement> GetContentFiles(XDocument projectDocument)
        {
            return projectDocument.Descendants(Xmlns + "Content")
                .Where(x => GetContentPath(x).StartsWith(ContentFolder));
        }

        private static string GetContentPath(XElement contentElement)
        {
            return contentElement.Attribute("Include") != null
                ? contentElement.Attribute("Include").Value
                : string.Empty;
        }

        private static string GetLinkPath(XElement contentElement)
        {
            return contentElement.Element(Xmlns + "Link") != null
                ? contentElement.Element(Xmlns + "Link").Value
                : string.Empty;
        }
    }
}
