using System;

using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Tiles;

namespace GameFramework
{
    public interface IDrawImplementation
    {
        void DrawString(DrawStringParams param);

        void DrawLine(DrawLineParams param);

        void DrawImage(DrawImageParams param);

        void DrawRectangle(DrawLineParams param);

        void FillRectangle(DrawLineParams param);

        void FillColor(Color color);

        void UseLinearSampler();

        void UsePointSampler();

        void SetRenderTarget(IPreDrawable painter, Vector size);

        void FlushRenderTarget();

        void DrawPreDrawn(IPreDrawable painter);
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

        public void SetRenderTarget(IPreDrawable painter, Vector size)
        {
            this.drawImplementation.SetRenderTarget(painter, size);
        }

        public void FlushRenderTarget()
        {
            this.drawImplementation.FlushRenderTarget();
        }

        public void DrawPreDrawn(IPreDrawable painter)
        {
            this.drawImplementation.DrawPreDrawn(painter);
        }

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

        public void DrawRectangle(DrawLineParams param)
        {
            this.drawImplementation.DrawRectangle(param);
        }

        public void FillRectangle(DrawLineParams param)
        {
            this.drawImplementation.FillRectangle(param);
        }
        
        public void FillColor(Color color)
        {
            this.drawImplementation.FillColor(color);
        }

        public void UseLinearSampler()
        {
            this.drawImplementation.UseLinearSampler();
        }

        public void UsePointSampler()
        {
            this.drawImplementation.UsePointSampler();
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
