using System;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;

namespace WindowsGame1.Maps
{
    public class ImageMap : MapBase
    {
        private readonly Texture2D texture;

        public ImageMap(string name, Texture2D texture, Rectangle rectangle)
            : base(name)
        {
            this.texture = texture;
            this.Rectangle = rectangle;
        }

        public Rectangle Rectangle { get; set; }

        public static ImageMap CreateFillScreenImageMap(string name, GraphicsDevice device, Texture2D texture)
        {
            return new ImageMap(name, texture, device.Viewport.Bounds);
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(this.texture, this.Rectangle, Color.White);
        }

        public override XElement ToXml()
        {
            return new XElement("ImageMap",
                new XAttribute("name", this.Name),
                new XElement("Texture", this.texture.Name),
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