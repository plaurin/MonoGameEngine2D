// Modified for MonoGameEngine2D by Pascal Laurin @2014
// Originals can be found at https://github.com/marshallward/TiledSharp
// Original notice below:
// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.IO;
using System.Xml.Linq;

namespace GameFramework.IO.TiledSharp
{
    internal class TmxImage
    {
        internal TmxImage(XElement image, string tmxDir = "")
        {
            if (image == null) return;

            var source = image.Attribute("source");

            if (source != null)
                // Append directory if present
                this.Source = Path.Combine(tmxDir, (string)source);
            else
            {
                this.Format = (string)image.Attribute("format");
                var data = image.Element("data");
                var decodedStream = new TmxBase64Data(data);
                this.Data = decodedStream.Data;
            }

            this.Trans = new TmxColor(image.Attribute("trans"));
            this.Width = (int?)image.Attribute("width");
            this.Height = (int?)image.Attribute("height");
        }

        public string Format { get; private set; }

        public string Source { get; private set; }

        public Stream Data { get; private set; }

        public TmxColor Trans { get; private set; }

        public int? Width { get; private set; }

        public int? Height { get; private set; }
    }
}