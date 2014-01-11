// Modified for MonoGameEngine2D by Pascal Laurin @2014
// Originals can be found at https://github.com/marshallward/TiledSharp
// Original notice below:
// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Xml.Linq;

namespace GameFramework.IO.TiledSharp
{
    internal class TmxTerrain : ITmxElement
    {
        internal TmxTerrain(XElement terrain)
        {
            this.Name = (string)terrain.Attribute("name");
            this.Tile = (int)terrain.Attribute("tile");
            this.Properties = new PropertyDict(terrain.Element("properties"));
        }

        public string Name { get; private set; }

        public int Tile { get; private set; }

        public PropertyDict Properties { get; private set; }
    }
}