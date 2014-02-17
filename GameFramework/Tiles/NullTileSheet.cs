using System;

namespace GameFramework.Tiles
{
    public class NullTileSheet : TileSheet
    {
        public NullTileSheet()
            : base(null, "NullTileSheet", Size.Zero)
        {
        }
    }
}