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

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            drawContext.DrawImage(this.Texture, this.Rectangle);
        }
    }
}