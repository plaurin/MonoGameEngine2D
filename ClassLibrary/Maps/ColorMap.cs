using System;
using System.Xml.Linq;

using ClassLibrary.Cameras;

namespace ClassLibrary.Maps
{
    public class ColorMap : MapBase
    {
        //private readonly Texture texture; Only XNA

        public ColorMap(string name, /*Texture texture,*/ Color color)
            : base(name)
        {
            this.Color = color;
            //this.texture = texture;
        }

        public Color Color { get; set; }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            drawContext.FillColor(this.Color);
            //spriteBatch.Draw(this.texture, camera.Viewport.Bounds, this.Color);
        }

        public override XElement ToXml()
        {
            return new XElement("ColorMap",
                new XAttribute("name", this.Name),
                new XElement("Color", this.Color));
        }

        public static ColorMap FromXml(Factory factory, GameResourceManager gameResourceManager, XElement mapElement)
        {
            var name = mapElement.Attribute("name").Value;
            var colorValue = mapElement.Element("Color").Value;

            //var texture = gameResourceManager.GetTexture("WhitePixel"); OnlyXNA
            return new ColorMap(name, MathUtil.ParseColor(colorValue));
            //return factory.CreateColorMap(name, MathUtil.ParseColor(colorValue));
        }
    }
}