using System;

using ClassLibrary;
using ClassLibrary.Tiles;

namespace Editor
{
    public class WinTileSheet : TileSheet
    {
        public WinTileSheet(Texture texture, string sheetName, Size tileSize)
            : base(texture, sheetName, tileSize)
        {
        }

        public override void Draw(DrawContext drawContext, TileDefinition tileDefinition, Rectangle destination)
        {
            //spriteBatch.Draw(this.texture, destination, tileDefinition.Rectangle, Color.White);

            var winDrawContext = (WinDrawContext)drawContext;

            winDrawContext.DrawImage(this.Texture, tileDefinition.Rectangle, destination);
        }

        protected override TileDefinition CreateTileDefinition(TileSheet tileSheet, string tileName, Rectangle rectangle)
        {
            return new WinTileDefinition(tileSheet, tileName, rectangle);
        }
    }
}