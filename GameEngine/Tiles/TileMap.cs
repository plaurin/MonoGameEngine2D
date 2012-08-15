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

        public TileMap(string name, Size mapSize, Size tileSize, TileDefinition defaultTileDefinition = null)
            : base(name)
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

        public override XElement ToXml()
        {
            var tileReferences = this.CreateTileReferences().ToList();

            return new XElement("TileMap",
                this.BaseToXml(),
                new XElement("MapSize", this.MapSize),
                new XElement("TileSize", this.TileSize),
                new XElement("TileDefinitionReferences", tileReferences.Select(x =>
                    new XElement("Reference",
                        new XAttribute("id", x.Id),
                        new XAttribute("sheetName", x.Definition.SheetName),
                        new XAttribute("name", x.Definition.Name)))),
                new XElement("Tiles", this.GetRowsXml(tileReferences)));
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

        public static TileMap FromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var name = mapElement.Attribute("name").Value;
            var mapSize = MathUtil.ParseSize(mapElement.Element("MapSize").Value);
            var tileSize = MathUtil.ParseSize(mapElement.Element("TileSize").Value);
            var tileReferences = GetTileReferences(gameResourceManager, mapElement.Element("TileDefinitionReferences")).ToList();
            var tiles = GetRowsFromXml(mapElement.Element("Tiles"));

            var map = new TileMap(name, mapSize, tileSize);
            map.BaseFromXml(mapElement);

            int x = 0;
            foreach (var row in tiles)
            {
                int y = 0;
                foreach (var element in row)
                {
                    map[x, y++] = tileReferences.Single(r => r.Id == element).Definition;
                }

                x++;
            }

            return map;
        }

        private static IEnumerable<IEnumerable<int>> GetRowsFromXml(XElement rowsElement)
        {
            return rowsElement.Elements()
                .Select(rowElement => rowElement.Value.Split(',').Select(x => int.Parse(x.Trim())));
        }

        private static IEnumerable<TileReference> GetTileReferences(GameResourceManager gameResourceManager, XElement tileReferencesElement)
        {
            return tileReferencesElement.Elements()
                .Select(x => new TileReference
                {
                    Id = int.Parse(x.Attribute("id").Value),
                    Definition = gameResourceManager
                        .GetTileSheet(x.Attribute("sheetName").Value)
                        .Definitions[x.Attribute("name").Value]
                });
        }

        private struct TileReference
        {
            public int Id { get; set; }

            public TileDefinition Definition { get; set; }
        }
    }
}