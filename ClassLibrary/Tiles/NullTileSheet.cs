using System;

namespace ClassLibrary.Tiles
{
    public class NullTileSheet : TileSheet
    {
        public NullTileSheet()
            : base(null, "null", Size.Zero)
        {
        }

        public override void Draw(DrawContext drawContext, TileDefinition tileDefinition, Rectangle destination)
        {
            // Do nothing NullObject pattern
        }

        protected override TileDefinition CreateTileDefinition(TileSheet tileSheet, string tileName, Rectangle rectangle)
        {
            throw new NotImplementedException();
        }
    }
}