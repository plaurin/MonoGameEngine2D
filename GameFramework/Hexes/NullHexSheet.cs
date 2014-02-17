using System;

namespace GameFramework.Hexes
{
    public class NullHexSheet : HexSheet
    {
        public NullHexSheet()
            : base(null, "NullHexSheet", Size.Zero)
        {
        }

        public override void Draw(IDrawContext drawContext, HexDefinition hexDefinition, Rectangle destination)
        {
            // Do nothing NullObjectPattern
        }
    }
}