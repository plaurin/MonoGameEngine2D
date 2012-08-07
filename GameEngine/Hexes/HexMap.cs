using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;
using WindowsGame1.Maps;
using WindowsGame1.Sprites;

namespace WindowsGame1.Hexes
{
    public class HexMap : MapBase
    {
        private readonly HexDefinition[,] map;
        private readonly HexGrid grid;

        public HexMap(Size mapSize, Size hexSize, HexDefinition defaultHexDefinition = null)
        {
            this.MapSize = mapSize;
            this.HexSize = hexSize;

            this.grid = HexGrid.CreateHexMap(hexSize.Width / 2, mapSize.Width);

            this.map = new HexDefinition[mapSize.Width, mapSize.Height];
            if (defaultHexDefinition == null)
                defaultHexDefinition = NullHexDefinition.CreateInstance();

            for (var i = 0; i < mapSize.Width; i++)
                for (var j = 0; j < mapSize.Height; j++)
                {
                    this.map[i, j] = defaultHexDefinition;
                }
        }

        public Size MapSize { get; private set; }

        public Size HexSize { get; private set; }

        public HexDefinition this[int x, int y]
        {
            get { return this.map[x, y]; }
            set { this.map[x, y] = value; }
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            for (var i = 0; i < this.MapSize.Width; i++)
                for (var j = 0; j < this.MapSize.Height; j++)
                {
                    var gridElement = this.grid[i, j];

                    var destination = gridElement.Rectangle
                        .Scale(camera.ZoomFactor)
                        .Translate(camera.GetSceneTranslationVector(this.ParallaxScrollingVector));

                    this.map[i, j].Draw(spriteBatch, destination);
                }
        }

        public override XElement ToXml()
        {
            var hexReferences = this.CreateHexReferences().ToList();

            return new XElement("HexMap",
                this.BaseToXml(),
                new XElement("MapSize", this.MapSize),
                new XElement("HexSize", this.HexSize),
                new XElement("HexDefinitionReferences", hexReferences.Select(x =>
                    new XElement("Reference",
                        new XAttribute("id", x.Id),
                        new XAttribute("sheetName", x.Definition.SheetName),
                        new XAttribute("name", x.Definition.Name)))),
                new XElement("Hexes", this.GetRowsXml(hexReferences)));
        }

        private IEnumerable<HexReference> CreateHexReferences()
        {
            var hexDefinitions = new List<HexDefinition>();
            for (var i = 0; i < this.MapSize.Width; i++)
                for (var j = 0; j < this.MapSize.Height; j++)
                {
                    hexDefinitions.Add(this.map[i, j]);
                }

            return hexDefinitions
                .Distinct()
                .Select((x, i) => new HexReference { Id = i, Definition = x });
        }

        private IEnumerable<XElement> GetRowsXml(List<HexReference> hexReferences)
        {
            for (var i = 0; i < this.MapSize.Width; i++)
            {
                var row = new int[this.MapSize.Height];
                for (var j = 0; j < this.MapSize.Height; j++)
                {
                    row[j] = hexReferences.Single(x => x.Definition == this.map[i, j]).Id;
                }

                yield return new XElement("Row", string.Join(", ", row));
            }
        }

        public static HexMap FromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var mapSize = MathUtil.ParseSize(mapElement.Element("MapSize").Value);
            var hexSize = MathUtil.ParseSize(mapElement.Element("HexSize").Value);
            var hexReferences = GetHexReferences(gameResourceManager, mapElement.Element("HexDefinitionReferences")).ToList();
            var hexes = GetRowsFromXml(mapElement.Element("Hexes"));

            var map = new HexMap(mapSize, hexSize);
            map.BaseFromXml(mapElement);

            int x = 0;
            foreach (var row in hexes)
            {
                int y = 0;
                foreach (var element in row)
                {
                    map[x, y++] = hexReferences.Single(r => r.Id == element).Definition;
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

        private static IEnumerable<HexReference> GetHexReferences(GameResourceManager gameResourceManager, XElement hexReferencesElement)
        {
            return hexReferencesElement.Elements()
                .Select(x => new HexReference
                {
                    Id = int.Parse(x.Attribute("id").Value),
                    Definition = gameResourceManager
                        .GetHexSheet(x.Attribute("sheetName").Value)
                        .Definitions[x.Attribute("name").Value]
                });
        }

        private struct HexReference
        {
            public int Id { get; set; }

            public HexDefinition Definition { get; set; }
        }
    }
}