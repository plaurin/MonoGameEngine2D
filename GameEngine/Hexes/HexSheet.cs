using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Sprites;

namespace WindowsGame1.Hexes
{
    public class HexSheet
    {
        private readonly Texture2D texture2D;
        private readonly Dictionary<string, HexDefinition> definitions;

        public HexSheet(Texture2D texture2D, string sheetName, Size hexSize)
        {
            this.SheetName = sheetName;
            this.HexSize = hexSize;
            this.texture2D = texture2D;

            this.definitions = new Dictionary<string, HexDefinition>();
        }

        public string SheetName { get; set; }

        public Size HexSize { get; set; }

        public HexDefinition CreateTileDefinition(string hexName, Point hexPosition)
        {
            var rectangle = new Rectangle(hexPosition.X, hexPosition.Y, this.HexSize.Width, this.HexSize.Height);
            var hexDefinition = new HexDefinition(this, hexName, rectangle);

            this.definitions.Add(hexName, hexDefinition);
            return hexDefinition;
        }

        public void Draw(SpriteBatch spriteBatch, HexDefinition hexDefinition, Rectangle destination)
        {
            spriteBatch.Draw(this.texture2D, destination, hexDefinition.Rectangle, Color.White);
        }
    }
}
