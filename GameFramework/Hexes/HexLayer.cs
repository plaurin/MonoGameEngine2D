using System;

using GameFramework.Cameras;
using GameFramework.Layers;
using GameFramework.Scenes;

namespace GameFramework.Hexes
{
    public class HexLayer : LayerBase
    {
        private readonly HexDefinition[,] map;

        private readonly int topEdgeLength;

        public HexLayer(string name, Size mapSize, Size hexSize, int topEdgeLength, HexDefinition defaultHexDefinition = null)
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

        public int TopEdgeLength
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

        public override int TotalElements
        {
            get { return this.MapSize.Width * this.MapSize.Height; }
        }

        public override int DrawnElementsLastFrame
        {
            get { return this.TotalElements; }
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

        public override HitBase GetHit(Vector position, Camera camera)
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
                        new Vector(rectangle.X + x1, rectangle.Y),
                        new Vector(rectangle.X + x2, rectangle.Y),
                        new Vector(rectangle.X + this.HexSize.Width, rectangle.Y + halfHeight),
                        new Vector(rectangle.X + x2, rectangle.Y + this.HexSize.Height),
                        new Vector(rectangle.X + x1, rectangle.Y + this.HexSize.Height),
                        new Vector(rectangle.X, rectangle.Y + halfHeight)
                    };

                    if (MathUtil.IsHitPolygone(polygone, mapPosition))
                        return new HexHit(new Point(i, j));
                }

            return null;
        }
    }
}