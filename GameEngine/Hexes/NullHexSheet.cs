using System;

using WindowsGame1.Sprites;

namespace WindowsGame1.Hexes
{
    public class NullHexSheet : HexSheet
    {
        public NullHexSheet()
            : base(null, "null", Size.Zero)
        {
        }
    }
}