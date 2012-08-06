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

        public ImageMap(Texture2D texture, Rectangle rectangle)
        {
            this.texture = texture;
            this.Rectangle = rectangle;
        }

        public Rectangle Rectangle { get; set; }

        public static ImageMap CreateFillScreenImageMap(GraphicsDevice device, Texture2D texture)
        {
            return new ImageMap(texture, device.Viewport.Bounds);
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(this.texture, this.Rectangle, Color.White);
        }

        public override XElement GetXml()
        {
            return new XElement("ImageMap",
                new XElement("Texture", this.texture.Name),
                new XElement("Rectangle", this.Rectangle));
        }

        public static ImageMap CreateFromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var textureName = mapElement.Element("Texture").Value;
            var rectangleValue = mapElement.Element("Rectangle").Value;
            
            return new ImageMap(
                gameResourceManager.GetTexture(textureName),
                MathUtil.ParseRectangle(rectangleValue));
        }
    }
}