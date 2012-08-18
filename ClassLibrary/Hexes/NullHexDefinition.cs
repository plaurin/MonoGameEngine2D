using System;

namespace ClassLibrary.Hexes
{
    public class NullHexDefinition : HexDefinition
    {
        private NullHexDefinition()
            : base(new NullHexSheet(), "null", Rectangle.Empty)
        {
            this.Sheet.AddHexDefinition(this);
        }

        public static NullHexDefinition CreateInstance()
        {
            return new NullHexDefinition();
        }

        public override void Draw(DrawContext drawContext, Rectangle destination)
        {
            // Do nothing: Null Object Pattern
        }
    }
}