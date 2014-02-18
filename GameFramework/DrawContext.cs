using System;

using GameFramework.Cameras;
using GameFramework.Drawing;

namespace GameFramework
{
    public interface IDrawImplementation
    {
        void DrawString(DrawStringParams param);

        void DrawLine(DrawLineParams param);

        void DrawImage(DrawImageParams param);

        void FillColor(Color color);
    }

    public interface IDrawContext : IDrawImplementation
    {
        ICamera Camera { get; }
    }

    public class DrawContext : IDrawContext
    {
        private readonly IDrawImplementation drawImplementation;

        public DrawContext(IDrawImplementation drawImplementation)
        {
            this.drawImplementation = drawImplementation;
        }

        public ICamera Camera { get; set; }

        public void DrawString(DrawStringParams param)
        {
            this.drawImplementation.DrawString(param);
        }

        public void DrawLine(DrawLineParams param)
        {
            this.drawImplementation.DrawLine(param); 
        }

        public void DrawImage(DrawImageParams param)
        {
            this.drawImplementation.DrawImage(param);
        }

        public void FillColor(Color color)
        {
            this.drawImplementation.FillColor(color);
        }
    }

    public struct DrawLineParams
    {
        public Vector VectorFrom { get; set; }

        public Vector VectorTo { get; set; }

        public float Width { get; set; }

        public Color Color { get; set; }
    }

    public class DrawImageParams
    {
        public DrawImageParams()
        {
            this.Origin = Vector.Zero;
            this.Color = Color.White;
            this.ImageEffect = ImageEffect.None;
        }

        public Texture Texture { get; set; }

        public Rectangle Destination { get; set; }

        public RectangleInt? Source { get; set; }

        public float Rotation { get; set; }

        public Vector Origin { get; set; }

        public Color Color { get; set; }

        public ImageEffect ImageEffect { get; set; }
    }

    [Flags]
    public enum ImageEffect
    {
        None = 0,
        FlipHorizontally = 1,
        FlipVertically = 2,
    }

    public struct DrawStringParams
    {
        public string Text { get; set; }

        public Vector Vector { get; set; }

        public float ZoomFactor { get; set; }

        public DrawingFont DrawingFont { get; set; }

        public Color Color { get; set; }
    }
}
