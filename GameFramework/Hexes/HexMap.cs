using System;

using GameFramework.Cameras;
using GameFramework.Maps;
using GameFramework.Scenes;

namespace GameFramework.Hexes
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

        internal int TopEdgeLength
        {
            get { return this.topEdgeLength; }
        }

        public HexDefinition this[int x, int y]
        {
            get { return this.map[x, y]; }
            set { this.map[x, y] = value; }
        }

        public HexDefinition this[Point point]
        {
            get { return this.map[point.X, point.Y]; }
            set { this.map[point.X, point.Y] = value; }
        }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            for (var i = 0; i < this.MapSize.Width; i++)
                for (var j = 0; j < this.MapSize.Height; j++)
                {
                    var hexDistance = this.HexSize.Width - (this.HexSize.Width - this.TopEdgeLength) / 2;
                    var halfHeight = this.HexSize.Height / 2;

                    var rectangle = new Rectangle(
                        this.Offset.X + i * hexDistance,
                        this.Offset.Y + j * this.HexSize.Height + (i % 2 == 1 ? halfHeight : 0),
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
                    var hexDistance = this.HexSize.Width - (this.HexSize.Width - this.TopEdgeLength) / 2;
                    var halfHeight = this.HexSize.Height / 2;

                    var rectangle = new Rectangle(
                        this.Offset.X + i * hexDistance,
                        this.Offset.Y + j * this.HexSize.Height + (i % 2 == 1 ? halfHeight : 0),
                        this.HexSize.Width, this.HexSize.Height);

                    var mapPosition = position
                        .Translate(-camera.GetSceneTranslationVector(this.ParallaxScrollingVector))
                        .Scale(1.0f / camera.ZoomFactor);

                    var x1 = (this.HexSize.Width - this.TopEdgeLength) / 2;
                    var x2 = x1 + this.TopEdgeLength;

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

        //public override XElement ToXml()
        //{
        //    return XmlRepository.ToXml(this);
        //    //var hexReferences = this.CreateHexReferences().ToList();

        //    //return new XElement("HexMap",
        //    //    this.BaseToXml(),
        //    //    new XElement("MapSize", this.MapSize),
        //    //    new XElement("HexSize", this.HexSize),
        //    //    new XElement("EdgeLength", this.topEdgeLength),
        //    //    new XElement("HexDefinitionReferences", hexReferences.Select(x =>
        //    //        new XElement("Reference",
        //    //            new XAttribute("id", x.Id),
        //    //            new XAttribute("sheetName", x.Definition.SheetName),
        //    //            new XAttribute("name", x.Definition.Name)))),
        //    //    new XElement("Hexes", this.GetRowsXml(hexReferences)));
        //}

        //internal IEnumerable<HexReference> CreateHexReferences()
        //{
        //    var hexDefinitions = new List<HexDefinition>();
        //    for (var i = 0; i < this.MapSize.Width; i++)
        //        for (var j = 0; j < this.MapSize.Height; j++)
        //        {
        //            hexDefinitions.Add(this.map[i, j]);
        //        }

        //    return hexDefinitions
        //        .Distinct()
        //        .Select((x, i) => new HexReference { Id = i, Definition = x });
        //}

        //internal IEnumerable<XElement> GetRowsXml(List<HexReference> hexReferences)
        //{
        //    for (var i = 0; i < this.MapSize.Width; i++)
        //    {
        //        var row = new int[this.MapSize.Height];
        //        for (var j = 0; j < this.MapSize.Height; j++)
        //        {
        //            row[j] = hexReferences.Single(x => x.Definition == this.map[i, j]).Id;
        //        }

        //        yield return new XElement("Row", string.Join(", ", row));
        //    }
        //}


        internal struct HexReference
        {
            public int Id { get; set; }

            public HexDefinition Definition { get; set; }
        }
    }
}