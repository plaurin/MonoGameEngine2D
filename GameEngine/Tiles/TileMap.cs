using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;
using WindowsGame1.Maps;
using WindowsGame1.Sprites;

namespace WindowsGame1.Tiles
{
    public class TileMap : MapBase
    {
        private readonly TileDefinition[,] map;

        public TileMap(Size mapSize, Size tileSize, TileDefinition defaultTileDefinition = null)
        {
            this.MapSize = mapSize;
            this.TileSize = tileSize;

            this.map = new TileDefinition[mapSize.Width, mapSize.Height];
            if (defaultTileDefinition == null)
                defaultTileDefinition = NullTileDefinition.CreateInstance();

            for (var i = 0; i < mapSize.Width; i++)
                for (var j = 0; j < mapSize.Height; j++)
                {
                    this.map[i, j] = defaultTileDefinition;
                }
        }

        public Size MapSize { get; private set; }

        public Size TileSize { get; private set; }

        public TileDefinition this[int x, int y]
        {
            get { return this.map[x, y]; }
            set { this.map[x, y] = value; }
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            for (var i = 0; i < this.MapSize.Width; i++)
                for (var j = 0; j < this.MapSize.Height; j++)
                {
                    var destination =
                        new Rectangle(i * this.TileSize.Width, j * this.TileSize.Height, this.TileSize.Width, this.TileSize.Height)
                        .Scale(camera.ZoomFactor)
                        .Translate(camera.GetSceneTranslationVector(this.ParallaxScrollingVector));

                    this.map[i, j].Draw(spriteBatch, destination);
                }
        }

        public override XElement GetXml()
        {
            var tileReferences = this.CreateTileReferences().ToList();

            return new XElement("TileMap",
                new XElement("MapSize", this.MapSize),
                new XElement("TileSize", this.TileSize),
                new XElement("HexDefinitionReferences", tileReferences.Select(x =>
                    new XElement("Reference",
                        new XAttribute("id", x.Id),
                        new XAttribute("sheetName", x.Definition.SheetName),
                        new XAttribute("name", x.Definition.Name)))),
                new XElement("Hexes", this.GetRowsXml(tileReferences)));
        }

        private IEnumerable<TileReference> CreateTileReferences()
        {
            var tileDefinitions = new List<TileDefinition>();
            for (var i = 0; i < this.MapSize.Width; i++)
                for (var j = 0; j < this.MapSize.Height; j++)
                {
                    tileDefinitions.Add(this.map[i, j]);
                }

            return tileDefinitions
                .Distinct()
                .Select((x, i) => new TileReference { Id = i, Definition = x });
        }

        private IEnumerable<XElement> GetRowsXml(List<TileReference> tileReferences)
        {
            for (var i = 0; i < this.MapSize.Width; i++)
            {
                var row = new int[this.MapSize.Height];
                for (var j = 0; j < this.MapSize.Height; j++)
                {
                    row[j] = tileReferences.Single(x => x.Definition == this.map[i, j]).Id;
                }

                yield return new XElement("Row", string.Join(", ", row));
            }
        }

        private struct TileReference
        {
            public int Id { get; set; }

            public TileDefinition Definition { get; set; }
        }
    }
}