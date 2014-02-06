using System;

using GameFramework.Cameras;
using GameFramework.Drawing;

namespace GameFramework
{
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
        public Texture Texture { get; set; }

        public Rectangle Destination { get; set; }

        public Rectangle? Source { get; set; }

        public float Rotation { get; set; }
    }
}
