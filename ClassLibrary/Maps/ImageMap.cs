using System;
using System.Xml.Linq;

using ClassLibrary.Cameras;

namespace ClassLibrary.Maps
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

        //public static ImageMap CreateFillScreenImageMap(string name, GraphicsDevice device, Texture2D texture)
        //{
        //    return new ImageMap(name, texture, device.Viewport.Bounds);
        //}

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            drawContext.DrawImage(this.Texture, this.Rectangle);
            //spriteBatch.Draw(this.texture, this.Rectangle, Color.White);
        }

        public override XElement ToXml()
        {
            return new XElement("ImageMap",
                new XAttribute("name", this.Name),
                new XElement("Texture", this.Texture.Name),
                new XElement("Rectangle", this.Rectangle));
        }

        public static ImageMap FromXml(Factory factory, GameResourceManager gameResourceManager, XElement mapElement)
        {
            var name = mapElement.Attribute("name").Value;
            var textureName = mapElement.Element("Texture").Value;
            var rectangleValue = mapElement.Element("Rectangle").Value;
            
            return new ImageMap(name, gameResourceManager.GetTexture(textureName), MathUtil.ParseRectangle(rectangleValue));
            //return factory.CreateImageMap(
            //    name, gameResourceManager.GetTexture(textureName), MathUtil.ParseRectangle(rectangleValue));
        }
    }
}