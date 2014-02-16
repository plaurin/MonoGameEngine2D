using System;
using System.Collections.Generic;

using GameFramework.Sheets;

namespace GameFramework.Tiles
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

        public Size TilesSize
        {
            get { return this.tilesSize; }
        }

        public TileDefinition CreateTileDefinition(string tileName, Point tilePosition)
        {
            var rectangle = new RectangleInt(tilePosition.X, tilePosition.Y, this.TilesSize.Width, this.TilesSize.Height);
            var tileDefinition = new TileDefinition(this, tileName, rectangle);
            this.definitions.Add(tileName, tileDefinition);

            return tileDefinition;
        }

        public void AddTileDefinition(TileDefinition tileDefinition)
        {
            this.definitions.Add(tileDefinition.Name, tileDefinition);
        }

        public void Draw(IDrawContext drawContext, TileDefinition tileDefinition, Rectangle destination)
        {
            drawContext.DrawImage(new DrawImageParams
            {
                Texture = this.Texture,
                Source = tileDefinition.Rectangle,
                Destination = destination,
            });
        }
    }
}
