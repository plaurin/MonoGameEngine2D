using System;

namespace ClassLibrary.Tiles
{
    public class NullTileSheet : TileSheet
    {
        public NullTileSheet()
            : base(null, "null", Size.Zero)
        {
        }
    }
}