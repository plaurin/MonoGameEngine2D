using System;

using WindowsGame1.Sprites;

namespace WindowsGame1.Tiles
{
    public class NullTileSheet : TileSheet
    {
        public NullTileSheet()
            : base(null, "null", Size.Zero)
        {
        }
    }
}