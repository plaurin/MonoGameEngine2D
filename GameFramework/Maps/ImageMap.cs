using System;
using System.Xml.Linq;

using GameFramework.Cameras;

namespace GameFramework.Maps
{
    public class ImageMap : MapBase
    {
        public ImageMap(string name, Texture texture, Rectangle rectangle)
            : base(name)
        {
            this.Texture = texture;
            this.Rectangle = rectangle;
        }

        public Rectangle Rectangle { get; set; }

        protected Texture Texture { get; private set; }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            drawContext.DrawImage(this.Texture, this.Rectangle);
        }

        public override XElement ToXml()
        {
            return new XElement("ImageMap",
                new XAttribute("name", this.Name),
                new XElement("Texture", this.Texture.Name),
                new XElement("Rectangle", this.Rectangle));
        }

        public static ImageMap FromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var name = mapElement.Attribute("name").Value;
            var textureName = mapElement.Element("Texture").Value;
            var rectangleValue = mapElement.Element("Rectangle").Value;
            
            return new ImageMap(name, gameResourceManager.GetTexture(textureName), MathUtil.ParseRectangle(rectangleValue));
        }
    }
}