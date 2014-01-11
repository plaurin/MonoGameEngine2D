// Modified for MonoGameEngine2D by Pascal Laurin @2014
// Originals can be found at https://github.com/marshallward/TiledSharp
// Original notice below:
// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Globalization;
using System.Xml.Linq;

namespace GameFramework.IO.TiledSharp
{
    internal class TmxColor
    {
        internal TmxColor(XAttribute color)
        {
            if (color == null) return;

            var colorStr = ((string)color).TrimStart("#".ToCharArray());

            this.R = int.Parse(colorStr.Substring(0, 2), NumberStyles.HexNumber);
            this.G = int.Parse(colorStr.Substring(2, 2), NumberStyles.HexNumber);
            this.B = int.Parse(colorStr.Substring(4, 2), NumberStyles.HexNumber);
        }

        public int R { get; set; }

        public int G { get; set; }

        public int B { get; set; }
    }
}