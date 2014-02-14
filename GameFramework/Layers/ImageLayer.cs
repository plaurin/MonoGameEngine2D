using System;
using GameFramework.Cameras;

namespace GameFramework.Layers
{
    public class ImageLayer : LayerBase
    {
        public ImageLayer(string name, Texture texture, Rectangle rectangle)
            : base(name)
        {
            this.Texture = texture;
            this.Rectangle = rectangle;
        }

        public Rectangle Rectangle { get; set; }

        public Texture Texture { get; private set; }

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
            drawContext.DrawImage(new DrawImageParams
            {
                Texture = this.Texture,
                Destination = this.Rectangle
            });

            return 1;
        }
    }
}