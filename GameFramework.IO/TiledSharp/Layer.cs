// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace GameFramework.IO.TiledSharp
{
    internal class TmxLayer : ITmxElement
    {
        internal TmxLayer(XElement layer, int width, int height)
        {
            this.Name = (string)layer.Attribute("name");
            this.Opacity = (double?)layer.Attribute("opacity") ?? 1.0;
            this.Visible = (bool?)layer.Attribute("visible") ?? true;

            var data = layer.Element("data");

            var encoding = (string)data.Attribute("encoding");

            this.Tiles = new List<TmxLayerTile>();
            if (encoding == "base64")
            {
                var decodedStream = new TmxBase64Data(data);
                var stream = decodedStream.Data;

                using (var br = new BinaryReader(stream))
                    for (int j = 0; j < height; j++)
                        for (int i = 0; i < width; i++)
                            this.Tiles.Add(new TmxLayerTile(br.ReadUInt32(), i, j));
            }
            else if (encoding == "csv")
            {
                var csvData = data.Value;
                int k = 0;
                foreach (var s in csvData.Split(','))
                {
                    var gid = uint.Parse(s.Trim());
                    var x = k % width;
                    var y = k / width;
                    this.Tiles.Add(new TmxLayerTile(gid, x, y));
                    k++;
                }
            }
            else if (encoding == null)
            {
                int k = 0;
                foreach (var e in data.Elements("tile"))
                {
                    var gid = (uint)e.Attribute("gid");
                    var x = k % width;
                    var y = k / width;
                    this.Tiles.Add(new TmxLayerTile(gid, x, y));
                    k++;
                }
            }
            else throw new Exception("TmxLayer: Unknown encoding.");

            this.Properties = new PropertyDict(layer.Element("properties"));
        }

        public string Name { get; private set; }

        public double Opacity { get; private set; }

        public bool Visible { get; private set; }

        public List<TmxLayerTile> Tiles { get; private set; }

        public PropertyDict Properties { get; private set; }
    }
}
