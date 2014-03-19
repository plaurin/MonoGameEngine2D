namespace GameFramework.Tiles
{
    public class ScalableTileLayer : TileLayer, IPreDrawable
    {
        public ScalableTileLayer(string name, Size mapSize, Size tileSize, TileDefinition defaultTileDefinition = null)
            : base(name, mapSize, tileSize, defaultTileDefinition)
        {
        }

        public void PreDraw(IDrawContext drawContext)
        {
            if (this.CameraMode == GameFramework.Cameras.CameraMode.Follow
                && drawContext.Camera.ZoomFactor > 1)
            {
                drawContext.SetRenderTarget(this, drawContext.Camera.SceneViewport.Size);

                var cameraTranslation = drawContext.Camera.SceneViewport.Positon.RoundTo(0);

                for (var i = 0; i < this.MapSize.Width; i++)
                    for (var j = 0; j < this.MapSize.Height; j++)
                    {
                        if (this[i, j].ShouldDraw)
                        {
                            var destination = new Rectangle(
                                this.Offset.X + i * this.TileSize.Width,
                                this.Offset.Y + j * this.TileSize.Height,
                                this.TileSize.Width,
                                this.TileSize.Height);

                            if (drawContext.Camera.SceneViewport.IsVisible(destination))
                            {
                                var adjustedDestination = destination.Translate(-cameraTranslation);

                                this[i, j].Draw(drawContext, adjustedDestination);
                            }
                        }
                    }

                drawContext.FlushRenderTarget();
            }
        }

        public override int Draw(IDrawContext drawContext, Transform transform)
        {
            if (this.CameraMode == GameFramework.Cameras.CameraMode.Follow
                && drawContext.Camera.ZoomFactor > 1)
            {
                drawContext.UseLinearSampler();
                drawContext.DrawPreDrawn(this);
                return 0;
            }

            return base.Draw(drawContext, transform);
        }
    }
}