using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using ClassLibrary.Cameras;
using ClassLibrary.Maps;
using ClassLibrary.Scenes;

namespace ClassLibrary.Hexes
{
    public class HexMap : MapBase
    {
        private readonly HexDefinition[,] map;

        private readonly int topEdgeLength;

        public HexMap(string name, Size mapSize, Size hexSize, int topEdgeLength, HexDefinition defaultHexDefinition = null)
            : base(name)
        {
            this.MapSize = mapSize;
            this.HexSize = hexSize;
            this.topEdgeLength = topEdgeLength;

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

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            for (var i = 0; i < this.MapSize.Width; i++)
                for (var j = 0; j < this.MapSize.Height; j++)
                {
                    var hexDistance = this.HexSize.Width - (this.HexSize.Width - this.topEdgeLength) / 2;
                    var halfHeight = this.HexSize.Height / 2;

                    var rectangle = new Rectangle(
                        i * hexDistance,
                        j * this.HexSize.Height + (i % 2 == 1 ? halfHeight : 0),
                        this.HexSize.Width, this.HexSize.Height);

                    var destination = rectangle
                        .Scale(camera.ZoomFactor)
                        .Translate(camera.GetSceneTranslationVector(this.ParallaxScrollingVector));

                    this.map[i, j].Draw(drawContext, destination);
                }
        }

        public override HitBase GetHit(Point position, Camera camera)
        {
            for (var i = 0; i < this.MapSize.Width; i++)
                for (var j = 0; j < this.MapSize.Height; j++)
                {
                    var hexDistance = this.HexSize.Width - (this.HexSize.Width - this.topEdgeLength) / 2;
                    var halfHeight = this.HexSize.Height / 2;

                    var rectangle = new Rectangle(
                        i * hexDistance,
                        j * this.HexSize.Height + (i % 2 == 1 ? halfHeight : 0),
                        this.HexSize.Width, this.HexSize.Height);

                    var mapPosition = position
                        .Translate(-camera.GetSceneTranslationVector(this.ParallaxScrollingVector))
                        .Scale(1.0f / camera.ZoomFactor);

                    var x1 = (this.HexSize.Width - this.topEdgeLength) / 2;
                    var x2 = x1 + this.topEdgeLength;

                    var polygone = new[]
                    {
                        new Point(rectangle.X + x1, rectangle.Y), new Point(rectangle.X + x2, rectangle.Y),
                        new Point(rectangle.X + this.HexSize.Width, rectangle.Y + halfHeight),
                        new Point(rectangle.X + x2, rectangle.Y + this.HexSize.Height),
                        new Point(rectangle.X + x1, rectangle.Y + this.HexSize.Height),
                        new Point(rectangle.X, rectangle.Y + halfHeight)
                    };

                    if (MathUtil.IsHitPolygone(polygone, mapPosition))
                        return new HexHit(new Point(i, j));
                }

            return null;
        }

        public override XElement ToXml()
        {
            var hexReferences = this.CreateHexReferences().ToList();

            return new XElement("HexMap",
                this.BaseToXml(),
                new XElement("MapSize", this.MapSize),
                new XElement("HexSize", this.HexSize),
                new XElement("EdgeLength", this.topEdgeLength),
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
            var name = mapElement.Attribute("name").Value;
            var mapSize = MathUtil.ParseSize(mapElement.Element("MapSize").Value);
            var hexSize = MathUtil.ParseSize(mapElement.Element("HexSize").Value);
            var edgeLength = int.Parse(mapElement.Element("EdgeLength").Value);
            var hexReferences = GetHexReferences(gameResourceManager, mapElement.Element("HexDefinitionReferences")).ToList();
            var hexes = GetRowsFromXml(mapElement.Element("Hexes"));

            var map = new HexMap(name, mapSize, hexSize, edgeLength);
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