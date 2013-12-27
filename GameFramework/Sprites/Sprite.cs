using System;
using System.Xml.Linq;

using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Sprites
{
    public class Sprite : SpriteBase
    {
        public Sprite(SpriteSheet spriteSheet, string spriteName)
        {
            this.SpriteSheet = spriteSheet;
            this.SpriteName = spriteName;
        }

        public SpriteSheet SpriteSheet { get; private set; }

        //public string SpriteName { get; private set; }

        //public Point Position { get; set; }

        public override void Draw(DrawContext drawContext, Camera camera, Point mapOffset, Vector parallaxScrollingVector)
        {
            this.SpriteSheet.Draw(drawContext, camera, mapOffset, parallaxScrollingVector, this);
        }

        public override HitBase GetHit(Point position, Camera camera, Point mapOffset, Vector parallaxScrollingVector)
        {
            return this.SpriteSheet.GetHit(position, camera, mapOffset, parallaxScrollingVector, this);
        }

        public XElement GetXml()
        {
            return new XElement("Sprite",
                new XAttribute("sheetName", this.SpriteSheet.Name),
                new XAttribute("name", this.SpriteName),
                new XAttribute("position", this.Position));
        }
    }

    public abstract class SpriteBase
    {
        public string SpriteName { get; set; }

        public Point Position { get; set; }

        public abstract void Draw(DrawContext drawContext, Camera camera, Point mapOffset, Vector parallaxScrollingVector);

        public abstract HitBase GetHit(Point position, Camera camera, Point mapOffset, Vector parallaxScrollingVector);
    }
}
