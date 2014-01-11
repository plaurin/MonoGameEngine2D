// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.IO;
using System.Xml.Linq;

namespace GameFramework.IO.TiledSharp
{
    internal class TmxDocument
    {
        protected XDocument ReadXml(string filepath)
        {
            //var asm = Assembly.GetExecutingAssembly();
            //var manifest = asm.GetManifestResourceNames();

            //var fileResPath = filepath.Replace(
            //        Path.DirectorySeparatorChar.ToString(), ".");
            //var fileRes = Array.Find(manifest, s => s.EndsWith(fileResPath));

            //// If there is a resource in the assembly, load the resource
            //// Otherwise, assume filepath is an explicit path
            //if (fileRes != null)
            //{
            //    Stream xmlStream = asm.GetManifestResourceStream(fileRes);
            //    xDoc = XDocument.Load(xmlStream);
            //    TmxDirectory = "";
            //}
            //else
            //{
            var doc = XDocument.Load(filepath);
            this.TmxDirectory = Path.GetDirectoryName(filepath);
            //}

            return doc;
        }

        public string TmxDirectory { get; private set; }
    }
}
