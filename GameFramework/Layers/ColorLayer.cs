using System;

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

        public override int Draw(IDrawContext drawContext)
        {
            drawContext.FillColor(this.Color);

            return 1;
        }
    }
}