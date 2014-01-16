using System;
using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Drawing
{
    public class RectangleElement : DrawingElementBase
    {
        private readonly float x;
        private readonly float y;
        private readonly float width;
        private readonly float height;

        public RectangleElement(Rectangle rectangle, int lineWidth, Color color)
            : this(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, lineWidth, color)
        {
        }

        public RectangleElement(float x, float y, float width, float height, int lineWidth, Color color)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.LineWidth = lineWidth;
            this.Color = color;
        }

        public int LineWidth { get; set; }

        public Color Color { get; set; }

        public override void Draw(DrawContext drawContext, Camera camera, DrawingLayer drawingLayer)
        {
            var topLeft = new Vector(this.x, this.y);
            var topRight = new Vector(this.x + this.width, this.y);
            var bottomLeft = new Vector(this.x, this.y + this.height);
            var bottomRight = new Vector(this.x + this.width, this.y + this.height);

            this.DrawLine(drawContext, camera, drawingLayer, topLeft, topRight, this.LineWidth, this.Color);
            this.DrawLine(drawContext, camera, drawingLayer, topRight, bottomRight, this.LineWidth, this.Color);
            this.DrawLine(drawContext, camera, drawingLayer, bottomRight, bottomLeft, this.LineWidth, this.Color);
            this.DrawLine(drawContext, camera, drawingLayer, bottomLeft, topLeft, this.LineWidth, this.Color);
        }

        public override HitBase GetHit(Point position, Camera camera, Point layerOffset, Vector parallaxScrollingVector)
        {
            var rectangle =
                new Rectangle(
                    layerOffset.X + (int)this.x,
                    layerOffset.X + (int)this.y,
                    (int)this.width,
                    (int)this.height)
                .Scale(camera.ZoomFactor)
                .Translate(camera.GetSceneTranslationVector(parallaxScrollingVector).ToPoint());

            return rectangle.Intercept(position)
                ? new RectangleHit(this)
                : null;
        }
    }
}