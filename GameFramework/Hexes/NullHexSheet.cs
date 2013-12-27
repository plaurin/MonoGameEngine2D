using System;

namespace GameFramework.Hexes
{
    public class NullHexSheet : HexSheet
    {
        public NullHexSheet()
            : base(null, "null", Size.Zero)
        {
        }

        public override void Draw(DrawContext drawContext, HexDefinition hexDefinition, Rectangle destination)
        {
            // Do nothing NullObjectPattern
        }

        //protected override HexDefinition CreateHexDefinition(HexSheet hexSheet, string hexName, Rectangle rectangle)
        //{
        //    throw new NotImplementedException();
        //}
    }
}