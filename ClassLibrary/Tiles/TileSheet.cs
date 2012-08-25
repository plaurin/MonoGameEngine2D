using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using ClassLibrary.Sheets;

namespace ClassLibrary.Tiles
{
    public class TileSheet : SheetBase
    {
        private readonly Size tilesSize;

        private readonly Dictionary<string, TileDefinition> definitions;

        public TileSheet(Texture texture, string name, Size tilesSize)
            : base(texture, name)
        {
            this.tilesSize = tilesSize;

            this.definitions = new Dictionary<string, TileDefinition>();
        }

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

        public void Draw(DrawContext drawContext, TileDefinition tileDefinition, Rectangle destination)
        {
            drawContext.DrawImage(this.Texture, tileDefinition.Rectangle, destination);
        }

        protected override IEnumerable<object> GetXml()
        {
            yield return new XElement("TileSize", this.tilesSize);
            yield return new XElement("Definitions", this.definitions.Select(d => d.Value.GetXml()));
        }

        public static SheetBase FromXml(XElement sheetElement, string name, Texture texture)
        {
            var tileSize = MathUtil.ParseSize(sheetElement.Element("TileSize").Value);
            var tileSheet = new TileSheet(texture, name, tileSize);

            foreach (var definitionElement in sheetElement.Elements("Definitions").Elements())
            {
                var tileDefinition = TileDefinition.FromXml(definitionElement, tileSheet);
                tileSheet.AddTileDefinition(tileDefinition);
            }

            return tileSheet;
        }
    }
}
