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

        public ColorMap(Texture2D texture, Color color)
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
                new XElement("Color",
                    string.Format("{0}, {1}, {2}, {3}", this.Color.R, this.Color.G, this.Color.B, this.Color.A)));
        }

        public static ColorMap FromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var colorValue = mapElement.Element("Color").Value
                .Split(',')
                .Select(x => int.Parse(x.Trim()))
                .ToArray();

            var texture = gameResourceManager.GetTexture("WhitePixel");
            return new ColorMap(texture, new Color(colorValue[0], colorValue[1], colorValue[2], colorValue[3]));
        }
    }
}