using System;

using GameFramework.Cameras;
using GameFramework.Layers;
using GameFramework.Scenes;

namespace GameFramework.Tiles
{
    public class TileLayer : LayerBase
    {
        private readonly TileDefinition[,] map;

        public TileLayer(string name, Size mapSize, Size tileSize, TileDefinition defaultTileDefinition = null)
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

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            for (var i = 0; i < this.MapSize.Width; i++)
                for (var j = 0; j < this.MapSize.Height; j++)
                {
                    var destination =
                        new Rectangle(
                            this.Offset.X + i * this.TileSize.Width, 
                            this.Offset.Y + j * this.TileSize.Height, 
                            this.TileSize.Width, 
                            this.TileSize.Height)
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
                    var tileRectangle =
                        new Rectangle(
                            this.Offset.X + i * this.TileSize.Width,
                            this.Offset.Y + j * this.TileSize.Height, 
                            this.TileSize.Width, 
                            this.TileSize.Height)
                        .Scale(camera.ZoomFactor)
                        .Translate(camera.GetSceneTranslationVector(this.ParallaxScrollingVector));

                    if (tileRectangle.Intercept(position))
                        return new TileHit(new Point(i, j));
                }

            return null;
        }
    }
}