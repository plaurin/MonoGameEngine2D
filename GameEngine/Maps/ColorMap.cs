using System;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;

namespace WindowsGame1.Maps
{
    public class ColorMap : MapBase
    {
        private readonly Texture2D texture;

        public ColorMap(string name, Texture2D texture, Color color)
            : base(name)
        {
            this.Color = color;
            this.texture = texture;
        }

        public Color Color { get; set; }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(this.texture, camera.Viewport.Bounds, this.Color);
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

            var texture = gameResourceManager.GetTexture("WhitePixel");
            return new ColorMap(name, texture, MathUtil.ParseColor(colorValue));
        }
    }
}