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

        public override void Draw(DrawContext drawContext, Camera camera, DrawingLayer drawingLayer)
        {
            var finalFrom = drawingLayer.CameraMode == CameraMode.Fix ? this.FromVector : this.FromVector
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(drawingLayer.ParallaxScrollingVector));

            var finalTo = drawingLayer.CameraMode == CameraMode.Fix ? this.ToVector : this.ToVector
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(drawingLayer.ParallaxScrollingVector));

            var zoom = drawingLayer.CameraMode == CameraMode.Fix ? 1.0f : this.Width * camera.ZoomFactor;

            //var finalWidth = this.width * camera.ZoomFactor;
            //var angle = (float)Math.Atan2(finalTo.Y - finalFrom.Y, finalTo.X - finalFrom.X);
            //var length = Vector.Distance(finalFrom, finalTo);

            drawContext.DrawLine(finalFrom, finalTo, zoom, this.Color);
            //spriteBatch.Draw(this.blank, finalFrom, null, this.color, angle, Vector.Zero,
            //    new Vector(length, finalWidth), SpriteEffects.None, 0);
        }

        public override HitBase GetHit(Vector position, Camera camera, Point layerOffset, Vector parallaxScrollingVector)
        {
            return null;
        }
    }
}