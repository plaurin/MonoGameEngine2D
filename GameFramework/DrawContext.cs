using System;

using GameFramework.Cameras;
using GameFramework.Drawing;

namespace GameFramework
{
    // TODO: Remove this temp class after cleanup with DrawContext, Camera, etc. for Composites IDrawable
    public class DrawContextWithCamera : DrawContext
    {
        private readonly DrawContext innerContext;

        public DrawContextWithCamera(DrawContext innerContext, Camera camera)
        {
            this.innerContext = innerContext;
            this.Camera = camera;
        }

        public Camera Camera { get; private set; }

        public override void DrawString(DrawContext drawContext, Camera camera, string text, Vector vector, float zoomFactor,
            DrawingFont drawingFont, Color color)
        {
            this.innerContext.DrawString(drawContext, camera, text, vector, zoomFactor, drawingFont, color);
        }

        public override void DrawLine(Vector vectorFrom, Vector vectorTo, float width, Color color)
        {
            this.innerContext.DrawLine(vectorFrom, vectorTo, width, color);
        }

        public override void DrawImage(DrawImageParams param)
        {
            this.innerContext.DrawImage(param);
        }

        public override void FillColor(Color color)
        {
            this.innerContext.FillColor(color);
        }
    }

    public abstract class DrawContext
    {
        public abstract void DrawString(
            DrawContext drawContext,
            Camera camera,
            string text,
            Vector vector,
            float zoomFactor,
            DrawingFont drawingFont,
            Color color);

        public abstract void DrawLine(Vector vectorFrom, Vector vectorTo, float width, Color color);

        public abstract void DrawImage(DrawImageParams param);

        public abstract void FillColor(Color color);
    }

    public class DrawImageParams
    {
        public DrawImageParams()
        {
            this.Origin = Vector.Zero;
        }

        public Texture Texture { get; set; }

        public Rectangle Destination { get; set; }

        public Rectangle? Source { get; set; }

        public float Rotation { get; set; }

        public Vector Origin { get; set; }
    }
}
