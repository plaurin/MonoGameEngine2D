using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WindowsGame1.Sprites;

namespace WindowsGame1.Tiles
{
    public class TileSheet
    {
        private readonly Texture2D texture2D;
        private readonly Size tilesSize;
        private readonly IDictionary<string, TileDefinition> definitions;

        public TileSheet(Texture2D texture2D, string sheetName, Size tilesSize)
        {
            this.texture2D = texture2D;
            this.SheetName = sheetName;
            this.tilesSize = tilesSize;

            this.definitions = new Dictionary<string, TileDefinition>();
        }

        protected string SheetName { get; private set; }

        public TileDefinition CreateTileDefinition(string tileName, Point tilePosition)
        {
            var rectangle = new Rectangle(tilePosition.X, tilePosition.Y, this.tilesSize.Width, this.tilesSize.Height);
            var tileDefinition = new TileDefinition(this, tileName, rectangle);
            this.definitions.Add(tileName, tileDefinition);

            return tileDefinition;
        }

        public void Draw(SpriteBatch spriteBatch, TileDefinition tileDefinition, Rectangle destination)
        {
            spriteBatch.Draw(this.texture2D, destination, tileDefinition.Rectangle, Color.White);
        }
    }
}
