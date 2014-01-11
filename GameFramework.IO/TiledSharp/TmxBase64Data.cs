// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.IO;
using System.Xml.Linq;
using Ionic.Zlib;

namespace GameFramework.IO.TiledSharp
{
    internal class TmxBase64Data
    {
        internal TmxBase64Data(XElement data)
        {
            if ((string)data.Attribute("encoding") != "base64")
                throw new Exception(
                    "TmxBase64Data: Only Base64-encoded data is supported.");

            var rawData = Convert.FromBase64String(data.Value);
            this.Data = new MemoryStream(rawData, false);

            var compression = (string)data.Attribute("compression");
            if (compression == "gzip")
                this.Data = new GZipStream(this.Data, CompressionMode.Decompress, false);
            else if (compression == "zlib")
                this.Data = new Ionic.Zlib.ZlibStream(this.Data,
                    Ionic.Zlib.CompressionMode.Decompress, false);
            else if (compression != null)
                throw new Exception("TmxBase64Data: Unknown compression.");
        }

        public Stream Data { get; private set; }
    }
}