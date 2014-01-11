// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;

namespace GameFramework.IO.TiledSharp
{
    internal class TmxMap : TmxDocument
    {
        internal TmxMap(string filename)
        {
            var doc = this.ReadXml(filename);
            var map = doc.Element("map");

            this.Version = (string)map.Attribute("version");
            this.Orientation = (OrientationType)Enum.Parse(typeof(OrientationType), map.Attribute("orientation").Value, true);
            this.Width = (int)map.Attribute("width");
            this.Height = (int)map.Attribute("height");
            this.TileWidth = (int)map.Attribute("tilewidth");
            this.TileHeight = (int)map.Attribute("tileheight");
            this.BackgroundColor = new TmxColor(map.Attribute("backgroundcolor"));

            this.Tilesets = new TmxList<TmxTileset>();
            foreach (var e in map.Elements("tileset"))
                this.Tilesets.Add(new TmxTileset(e, this.TmxDirectory));

            this.Layers = new TmxList<TmxLayer>();
            foreach (var e in map.Elements("layer"))
                this.Layers.Add(new TmxLayer(e, this.Width, this.Height));

            this.ObjectGroups = new TmxList<TmxObjectGroup>();
            foreach (var e in map.Elements("objectgroup"))
                this.ObjectGroups.Add(new TmxObjectGroup(e));

            this.ImageLayers = new TmxList<TmxImageLayer>();
            foreach (var e in map.Elements("imagelayer"))
                this.ImageLayers.Add(new TmxImageLayer(e, this.TmxDirectory));

            this.Properties = new PropertyDict(map.Element("properties"));
        }

        public string Version { get; private set; }

        public OrientationType Orientation { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int TileWidth { get; private set; }

        public int TileHeight { get; private set; }

        public TmxColor BackgroundColor { get; private set; }

        public TmxList<TmxTileset> Tilesets { get; private set; }

        public TmxList<TmxLayer> Layers { get; private set; }

        public TmxList<TmxObjectGroup> ObjectGroups { get; private set; }

        public TmxList<TmxImageLayer> ImageLayers { get; private set; }

        public PropertyDict Properties { get; private set; }

        internal enum OrientationType : byte
        {
            Orthogonal,
            Isometric,
            Staggered
        }
    }
}
