// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Xml.Linq;

namespace GameFramework.IO.TiledSharp
{
    internal class TmxTileOffset
    {
        internal TmxTileOffset(XElement tileOffset)
        {
            if (tileOffset == null)
            {
                this.X = 0;
                this.Y = 0;
            }
            else
            {
                this.X = (int)tileOffset.Attribute("x");
                this.Y = (int)tileOffset.Attribute("y");
            }
        }

        public int X { get; private set; }

        public int Y { get; private set; }
    }
}