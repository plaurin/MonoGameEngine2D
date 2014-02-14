using System;

namespace GameFramework.Tiles
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

        public override bool ShouldDraw
        {
            get { return false; }
        }

        public override void Draw(IDrawContext drawContext, Rectangle destination)
        {
            // Do nothing: Null Object Pattern
        }
    }
}