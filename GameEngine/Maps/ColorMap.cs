using System;
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

        public override XElement GetXml()
        {
            return new XElement("ColorMap",
                new XElement("Color", this.Color.PackedValue));
        }
    }
}