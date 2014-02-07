using System;
using GameFramework.Cameras;

namespace GameFramework.Layers
{
    public class ColorLayer : LayerBase
    {
        public ColorLayer(string name, Color color)
            : base(name)
        {
            this.Color = color;
        }

        public Color Color { get; set; }

        public override int TotalElements
        {
            get { return 1; }
        }

        public override int DrawnElementsLastFrame
        {
            get { return this.TotalElements; }
        }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            drawContext.FillColor(this.Color);
        }
    }
}