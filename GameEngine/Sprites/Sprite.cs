using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;

namespace WindowsGame1.Sprites
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

        public void Draw(SpriteBatch spriteBatch, Camera camera, Vector2 parallaxScrollingVector)
        {
            this.SpriteSheet.Draw(spriteBatch, camera, parallaxScrollingVector, this);
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
