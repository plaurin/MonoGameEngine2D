using System;

using ClassLibrary;
using ClassLibrary.Hexes;

namespace Editor
{
    public class WinHexSheet : HexSheet
    {
        public WinHexSheet(Texture texture, string name, Size hexSize)
            : base(texture, name, hexSize)
        {
        }

        public override void Draw(DrawContext drawContext, HexDefinition hexDefinition, Rectangle destination)
        {
            var winDrawContext = (WinDrawContext)drawContext;

            winDrawContext.DrawImage(this.Texture, hexDefinition.Rectangle, destination);
        }

        protected override HexDefinition CreateHexDefinition(HexSheet hexSheet, string hexName, Rectangle rectangle)
        {
            return new WinHexDefinition(hexSheet, hexName, rectangle);
        }
    }
}