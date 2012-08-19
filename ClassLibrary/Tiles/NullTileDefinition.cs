using System;

namespace ClassLibrary.Tiles
{
    public class NullTileDefinition : TileDefinition
    {
        private NullTileDefinition()
            : base(new NullTileSheet(), "null", Rectangle.Empty)
        {
            this.Sheet.AddTileDefinition(this);
        }

        public static NullTileDefinition CreateInstance()
        {
            return new NullTileDefinition();
        }

        //public override void Draw(DrawContext drawContext, Rectangle destination)
        //{
        //    // Do nothing: Null Object Pattern
        //}
    }
}