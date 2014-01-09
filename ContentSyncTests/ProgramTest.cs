using System;
using System.Linq;
using System.Xml.Linq;
using ContentSync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentSyncTests
{
    [TestClass]
    public class ProgramTest
    {
        private const string Source = @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\SamplesBrowser\SamplesBrowser.csproj";
        private const string Destination = @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\WindowsSamples\WindowsSamples.csproj";

        private static readonly XNamespace Xmlns = "http://schemas.microsoft.com/developer/msbuild/2003";

        [TestMethod]
        public void TestGetContentFiles()
        {
            var contentFiles = Program.GetContentFiles(Source);

            Assert.AreEqual(5, contentFiles.Count());
        }

        [TestMethod]
        public void TestGetLinkedContentFiles()
        {
            var contentFiles = Program.GetLinkedContentFiles(Destination);

            Assert.AreEqual(5, contentFiles.Count());
        }

        [TestMethod]
        public void GetRelativePathFromSourceToDestination()
        {
            var result = Program.GetRelativePathFromSourceToDestination(Source, Destination);

            Assert.AreEqual(@"..\SamplesBrowser", result);
        }

        [TestMethod]
        public void WhenMissingContentShouldAddLinks()
        {
            var sourceContentFiles = Program.GetContentFiles(Source).ToList();
            var destinationContentFiles = Program.GetLinkedContentFiles(Destination).ToList();
            var destinationDocument = XDocument.Load(Destination);
            var relativePath = Program.GetRelativePathFromSourceToDestination(Source, Destination);

            sourceContentFiles.Add(new XElement(Xmlns + "Content",
                new XAttribute("Include", @"Content\New1.png")));

            Program.AddMissingContents(sourceContentFiles, destinationContentFiles, destinationDocument, relativePath);

            var newDestinationContentFiles = destinationDocument.Descendants(Xmlns + "Content")
                .Where(x => x.Element(Xmlns + "Link") != null && x.Element(Xmlns + "Link").Value.StartsWith(@"Content\")).ToList();

            Assert.AreEqual(destinationContentFiles.Count() + 1, newDestinationContentFiles.Count());

            var newElement = newDestinationContentFiles.Single(x => x.Element(Xmlns + "Link").Value.Contains("New1.png"));

            const string Expected = @"<Content Include=""..\SamplesBrowser\Content\New1.png"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003""><Link>Content\New1.png</Link><CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory></Content>";

            Assert.AreEqual(Expected, newElement.ToString(SaveOptions.OmitDuplicateNamespaces | SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void WhenExtraContentShouldRemoveLinks()
        {
            var sourceContentFiles = Program.GetContentFiles(Source).ToList();
            var destinationDocument = XDocument.Load(Destination);

            destinationDocument.Root.Add(
                new XElement(Xmlns + "ItemGroup",
                    new XElement(Xmlns + "Content",
                        new XAttribute("Include", @"..\SamplesBrowser\Content\Old.png"),
                        new XElement(Xmlns + "Link", @"Content\Old1.png"))));

            var destinationContentFiles = Program.GetLinkedContentFiles(destinationDocument).ToList();

            Program.RemoveExtraContent(sourceContentFiles, destinationContentFiles, destinationDocument);

            var newDestinationContentFiles = destinationDocument.Descendants(Xmlns + "Content")
                .Where(x => x.Element(Xmlns + "Link") != null && x.Element(Xmlns + "Link").Value.StartsWith(@"Content\")).ToList();

            Assert.AreEqual(destinationContentFiles.Count() - 1, newDestinationContentFiles.Count());
        }

        [TestMethod]
        public void TempTestMain()
        {
            Program.Main(new[] { Source, Destination });
        }
    }
}
