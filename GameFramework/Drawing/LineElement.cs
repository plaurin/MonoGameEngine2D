using System;

using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Drawing
{
    public class LineElement : DrawingElementBase
    {
        public LineElement(Vector fromVector, Vector toVector, int width, Color color)
        {
            this.FromVector = fromVector;
            this.ToVector = toVector;
            this.Width = width;
            this.Color = color;
        }

        public Vector FromVector { get; private set; }
        
        public Vector ToVector { get; private set; }
        
        public int Width { get; private set; }

        public Color Color { get; private set; }

        public override void Draw(IDrawContext drawContext, DrawingLayer drawingLayer)
        {
            var finalFrom = drawingLayer.CameraMode == CameraMode.Fix ? this.FromVector : this.FromVector
                .Scale(drawContext.Camera.ZoomFactor)
                .Translate(drawContext.Camera.GetSceneTranslationVector(drawingLayer.ParallaxScrollingVector));

            var finalTo = drawingLayer.CameraMode == CameraMode.Fix ? this.ToVector : this.ToVector
                .Scale(drawContext.Camera.ZoomFactor)
                .Translate(drawContext.Camera.GetSceneTranslationVector(drawingLayer.ParallaxScrollingVector));

            var zoom = drawingLayer.CameraMode == CameraMode.Fix ? 1.0f : this.Width * drawContext.Camera.ZoomFactor;

            //var finalWidth = this.width * camera.ZoomFactor;
            //var angle = (float)Math.Atan2(finalTo.Y - finalFrom.Y, finalTo.X - finalFrom.X);
            //var length = Vector.Distance(finalFrom, finalTo);

            var param = new DrawLineParams
            {
                VectorFrom = finalFrom,
                VectorTo = finalTo,
                Width = zoom,
                Color = this.Color
            };

            drawContext.DrawLine(param);

            //spriteBatch.Draw(this.blank, finalFrom, null, this.color, angle, Vector.Zero,
            //    new Vector(length, finalWidth), SpriteEffects.None, 0);
        }

        public override HitBase GetHit(Vector position, Camera camera, Vector layerOffset, Vector parallaxScrollingVector)
        {
            return null;
        }
    }
}