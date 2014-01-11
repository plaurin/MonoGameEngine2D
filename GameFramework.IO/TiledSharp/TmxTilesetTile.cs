// Modified for MonoGameEngine2D by Pascal Laurin @2014
// Originals can be found at https://github.com/marshallward/TiledSharp
// Original notice below:
// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Collections.Generic;
using System.Xml.Linq;

namespace GameFramework.IO.TiledSharp
{
    internal class TmxTilesetTile
    {
        internal TmxTilesetTile(XElement tile, TmxList<TmxTerrain> terrains, string tmxDir = "")
        {
            this.Id = (int)tile.Attribute("id");

            this.TerrainEdges = new List<TmxTerrain>(4);

            var strTerrain = (string)tile.Attribute("terrain") ?? ",,,";
            foreach (var v in strTerrain.Split(','))
            {
                int result;
                var success = int.TryParse(v, out result);
                TmxTerrain edge;
                if (success)
                    edge = terrains[result];
                else
                    edge = null;
                this.TerrainEdges.Add(edge);
            }

            this.Probability = (double?)tile.Attribute("probability") ?? 1.0;
            this.Image = new TmxImage(tile.Element("image"), tmxDir);
            this.Properties = new PropertyDict(tile.Element("properties"));
        }

        public int Id { get; private set; }

        public List<TmxTerrain> TerrainEdges { get; private set; }

        public double Probability { get; private set; }

        public TmxImage Image { get; private set; }

        public PropertyDict Properties { get; private set; }

        // Human-readable aliases to the Terrain markers

        public TmxTerrain TopLeft
        {
            get { return this.TerrainEdges[0]; }
        }

        public TmxTerrain TopRight
        {
            get { return this.TerrainEdges[1]; }
        }

        public TmxTerrain BottomLeft
        {
            get { return this.TerrainEdges[2]; }
        }

        public TmxTerrain BottomRight
        {
            get { return this.TerrainEdges[3]; }
        }
    }
}