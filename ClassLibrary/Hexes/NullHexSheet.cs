using System;

namespace ClassLibrary.Hexes
{
    public class NullHexSheet : HexSheet
    {
        public NullHexSheet()
            : base(null, "null", Size.Zero)
        {
        }
    }
}