using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;
using WindowsGame1.Maps;
using WindowsGame1.Sprites;
using WindowsGame1.Tiles;

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

        public override XElement GetXml()
        {
            var hexReferences = this.CreateHexReferences().ToList();

            return new XElement("HexMap",
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

        private struct HexReference
        {
            public int Id { get; set; }

            public HexDefinition Definition { get; set; }
        }
    }
}