using System;

using GameFramework.Cameras;
using GameFramework.Layers;
using GameFramework.Scenes;

namespace GameFramework.Tiles
{
    public class TileLayer : LayerBase, IHitTarget
    {
        private readonly TileDefinition[,] map;
        private int drawnElementsLastFrame;

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

        public override int TotalElements
        {
            get { return this.MapSize.Width * this.MapSize.Height; }
        }

        public override int DrawnElementsLastFrame
        {
            get { return this.drawnElementsLastFrame; }
        }

        public override int Draw(IDrawContext drawContext, Transform transform)
        {
            this.SetSampler(drawContext);

            this.drawnElementsLastFrame = 0;

            for (var i = 0; i < this.MapSize.Width; i++)
                for (var j = 0; j < this.MapSize.Height; j++)
                {
                    if (this.map[i, j].ShouldDraw)
                    {
                        var destination = new Rectangle(
                            this.Offset.X + i * this.TileSize.Width, 
                            this.Offset.Y + j * this.TileSize.Height, 
                            this.TileSize.Width, 
                            this.TileSize.Height);

                        if (this.CameraMode == CameraMode.Follow)
                        {
                            destination = destination
                                .Scale(drawContext.Camera.ZoomFactor)
                                .Translate(drawContext.Camera.GetSceneTranslationVector(this.ParallaxScrollingVector));
                        }

                        if (drawContext.Camera.Viewport.IsVisible(destination))
                        {
                            this.map[i, j].Draw(drawContext, destination);
                            this.drawnElementsLastFrame++;
                        }
                    }
                }
            
            return this.drawnElementsLastFrame;
        }

        public HitBase GetHit(Vector position, ICamera camera, WorldTransform worldTransform)
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