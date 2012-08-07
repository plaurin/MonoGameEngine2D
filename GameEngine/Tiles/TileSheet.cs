using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WindowsGame1.Sprites;

namespace WindowsGame1.Tiles
{
    public class TileSheet
    {
        private readonly Texture2D texture;
        private readonly Size tilesSize;
        private readonly Dictionary<string, TileDefinition> definitions;

        public TileSheet(Texture2D texture, string sheetName, Size tilesSize)
        {
            this.texture = texture;
            this.Name = sheetName;
            this.tilesSize = tilesSize;

            this.definitions = new Dictionary<string, TileDefinition>();
        }

        public string Name { get; private set; }

        public Dictionary<string, TileDefinition> Definitions
        {
            get
            {
                return this.definitions;
            }
        }

        public TileDefinition CreateTileDefinition(string tileName, Point tilePosition)
        {
            var rectangle = new Rectangle(tilePosition.X, tilePosition.Y, this.tilesSize.Width, this.tilesSize.Height);
            var tileDefinition = new TileDefinition(this, tileName, rectangle);
            this.definitions.Add(tileName, tileDefinition);

            return tileDefinition;
        }

        public void AddTileDefinition(TileDefinition tileDefinition)
        {
            this.definitions.Add(tileDefinition.Name, tileDefinition);
        }

        public void Draw(SpriteBatch spriteBatch, TileDefinition tileDefinition, Rectangle destination)
        {
            spriteBatch.Draw(this.texture, destination, tileDefinition.Rectangle, Color.White);
        }

        public XElement GetXml()
        {
            return new XElement("TileSheet",
                new XAttribute("name", this.Name),
                new XElement("Texture", this.texture.Name),
                new XElement("TileSize", this.tilesSize),
                new XElement("Definitions", this.definitions.Select(d => d.Value.GetXml())));
        }
    }
}
