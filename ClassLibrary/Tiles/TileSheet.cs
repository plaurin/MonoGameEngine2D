using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ClassLibrary.Tiles
{
    public class TileSheet
    {
        private readonly Size tilesSize;

        private readonly Dictionary<string, TileDefinition> definitions;

        public TileSheet(Texture texture, string sheetName, Size tilesSize)
        {
            this.Texture = texture;
            this.Name = sheetName;
            this.tilesSize = tilesSize;

            this.definitions = new Dictionary<string, TileDefinition>();
        }

        public string Name { get; private set; }

        public Texture Texture { get; private set; }

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
            //var tileDefinition = this.CreateTileDefinition(this, tileName, rectangle);
            var tileDefinition = new TileDefinition(this, tileName, rectangle);
            this.definitions.Add(tileName, tileDefinition);

            return tileDefinition;
        }

        public void AddTileDefinition(TileDefinition tileDefinition)
        {
            this.definitions.Add(tileDefinition.Name, tileDefinition);
        }

        public XElement GetXml()
        {
            return new XElement("TileSheet",
                new XAttribute("name", this.Name),
                new XElement("Texture", this.Texture.Name),
                new XElement("TileSize", this.tilesSize),
                new XElement("Definitions", this.definitions.Select(d => d.Value.GetXml())));
        }

        public void Draw(DrawContext drawContext, TileDefinition tileDefinition, Rectangle destination)
        {
            drawContext.DrawImage(this.Texture, tileDefinition.Rectangle, destination);
            //spriteBatch.Draw(this.texture, destination, tileDefinition.Rectangle, Color.White);
        }

        //public abstract void Draw(DrawContext drawContext, TileDefinition tileDefinition, Rectangle destination);

        //protected abstract TileDefinition CreateTileDefinition(TileSheet tileSheet, string tileName, Rectangle rectangle);
    }
}
