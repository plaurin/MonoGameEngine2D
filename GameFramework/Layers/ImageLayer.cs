using System;
using GameFramework.Cameras;

namespace GameFramework.Layers
{
    public class ImageLayer : LayerBase
    {
        public ImageLayer(string name, Texture texture, RectangleInt rectangle)
            : base(name)
        {
            this.Texture = texture;
            this.Rectangle = rectangle;
        }

        public RectangleInt Rectangle { get; set; }

        public Texture Texture { get; private set; }

        public override int TotalElements
        {
            get { return 1; }
        }

        public override int DrawnElementsLastFrame
        {
            get { return this.TotalElements; }
        }

        public override int Draw(IDrawContext drawContext, Transform transform)
        {
            drawContext.DrawImage(new DrawImageParams
            {
                Texture = this.Texture,
                Destination = new Rectangle(this.Rectangle)
            });

            return 1;
        }
    }
}