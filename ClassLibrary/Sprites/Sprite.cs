using System;
using System.Xml.Linq;

using ClassLibrary.Cameras;

namespace ClassLibrary.Sprites
{
    public class Sprite
    {
        public Sprite(SpriteSheet spriteSheet, string spriteName)
        {
            this.SpriteSheet = spriteSheet;
            this.SpriteName = spriteName;
        }

        public SpriteSheet SpriteSheet { get; private set; }

        public string SpriteName { get; private set; }

        public Point Position { get; set; }

        public void Draw(DrawContext drawContext, Camera camera, Vector parallaxScrollingVector)
        {
            this.SpriteSheet.Draw(drawContext, camera, parallaxScrollingVector, this);
        }

        public XElement GetXml()
        {
            return new XElement("Sprite",
                new XAttribute("sheetName", this.SpriteSheet.Name),
                new XAttribute("name", this.SpriteName),
                new XAttribute("position", this.Position));
        }
    }
}
