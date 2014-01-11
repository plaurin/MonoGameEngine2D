// Modified for MonoGameEngine2D by Pascal Laurin @2014
// Originals can be found at https://github.com/marshallward/TiledSharp
// Original notice below:
// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace GameFramework.IO.TiledSharp
{
    internal class TmxTileset : TmxDocument, ITmxElement
    {
        internal TmxTileset(XDocument doc, string tmxDir) 
            : this(doc.Element("tileset"), tmxDir)
        {
        }

        internal TmxTileset(XElement tileset, string tmxDir = "")
        {
            var firstGid = tileset.Attribute("firstgid");
            var source = (string)tileset.Attribute("source");

            if (source != null)
            {
                // Prepend the parent TMX directory if necessary
                source = Path.Combine(tmxDir, source);

                // source is always preceded by firstgid
                this.FirstGid = (int)firstGid;

                // Everything else is in the TSX file
                var docTileset = this.ReadXml(source);
                var ts = new TmxTileset(docTileset, this.TmxDirectory);

                this.Name = ts.Name;
                this.TileWidth = ts.TileWidth;
                this.TileHeight = ts.TileHeight;
                this.Spacing = ts.Spacing;
                this.Margin = ts.Margin;
                this.TileOffset = ts.TileOffset;
                this.Image = ts.Image;
                this.Terrains = ts.Terrains;
                this.Tiles = ts.Tiles;
                this.Properties = ts.Properties;
            }
            else
            {
                // firstgid is always in TMX, but not TSX
                if (firstGid != null)
                    this.FirstGid = (int)firstGid;

                this.Name = (string)tileset.Attribute("name");
                this.TileWidth = (int)tileset.Attribute("tilewidth");
                this.TileHeight = (int)tileset.Attribute("tileheight");
                this.Spacing = (int?)tileset.Attribute("spacing") ?? 0;
                this.Margin = (int?)tileset.Attribute("margin") ?? 0;

                this.TileOffset = new TmxTileOffset(tileset.Element("tileoffset"));
                this.Image = new TmxImage(tileset.Element("image"), tmxDir);

                this.Terrains = new TmxList<TmxTerrain>();
                var terrainType = tileset.Element("terraintypes");
                if (terrainType != null)
                {
                    foreach (var e in terrainType.Elements("terrain"))
                        this.Terrains.Add(new TmxTerrain(e));
                }

                this.Tiles = new List<TmxTilesetTile>();
                foreach (var tileElement in tileset.Elements("tile"))
                {
                    var tile = new TmxTilesetTile(tileElement, this.Terrains, tmxDir);
                    this.Tiles.Add(tile);
                }

                this.Properties = new PropertyDict(tileset.Element("properties"));
            }
        }

        public int FirstGid { get; private set; }

        public string Name { get; private set; }

        public int TileWidth { get; private set; }

        public int TileHeight { get; private set; }

        public int Spacing { get; private set; }

        public int Margin { get; private set; }

        public TmxTileOffset TileOffset { get; private set; }

        public TmxImage Image { get; private set; }

        public TmxList<TmxTerrain> Terrains { get; private set; }

        public List<TmxTilesetTile> Tiles { get; private set; }

        public PropertyDict Properties { get; private set; }

        // TMX tileset element constructor
    }
}
