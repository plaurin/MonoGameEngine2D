using System;

using GameFramework.Cameras;

namespace GameFramework.Maps
{
    public class ColorMap : MapBase
    {
        //private readonly Texture texture; Only XNA

        public ColorMap(string name, Color color)
            : base(name)
        {
            this.Color = color;
        }

        public Color Color { get; set; }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            drawContext.FillColor(this.Color);
        }

        //public override XElement ToXml()
        //{
        //    return XmlRepository.ToXml(this);
        //}
    }
}