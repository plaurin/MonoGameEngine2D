using System;
using System.Xml.Linq;

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

        public override XElement ToXml()
        {
            return new XElement("ColorMap",
                new XAttribute("name", this.Name),
                new XElement("Color", this.Color));
        }

        public static ColorMap FromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var name = mapElement.Attribute("name").Value;
            var colorValue = mapElement.Element("Color").Value;

            return new ColorMap(name, MathUtil.ParseColor(colorValue));
        }
    }
}