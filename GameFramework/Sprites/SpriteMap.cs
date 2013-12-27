using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using GameFramework.Cameras;
using GameFramework.Maps;
using GameFramework.Scenes;

namespace GameFramework.Sprites
{
    public class SpriteMap : MapBase
    {
        private readonly List<SpriteBase> sprites;

        public SpriteMap(string name)
            : base(name)
        {
            this.sprites = new List<SpriteBase>();
        }

        public void AddSprite(SpriteBase sprite)
        {
            this.sprites.Add(sprite);
        }

        public void RemoveSprite(SpriteBase sprite)
        {
            this.sprites.Remove(sprite);
        }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            foreach (var sprite in this.sprites)
            {
                sprite.Draw(drawContext, camera, this.Offset, this.ParallaxScrollingVector);
            }
        }

        public override HitBase GetHit(Point position, Camera camera)
        {
            return this.sprites
                .Select(sprite => sprite.GetHit(position, camera, this.Offset, this.ParallaxScrollingVector))
                .FirstOrDefault(spriteHit => spriteHit != null);
        }

        public override XElement ToXml()
        {
            return new XElement("SpriteMap",
                this.BaseToXml(),
                new XElement("Sprites", this.sprites.OfType<Sprite>().Select(s => s.GetXml())));
        }

        public static SpriteMap FromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var mapName = mapElement.Attribute("name").Value;
            var map = new SpriteMap(mapName);
            map.BaseFromXml(mapElement);

            foreach (var element in mapElement.Element("Sprites").Elements())
            {
                var sheetName = element.Attribute("sheetName").Value;
                var name = element.Attribute("name").Value;
                var position = element.Attribute("position").Value;

                var sprite = new Sprite(gameResourceManager.GetSpriteSheet(sheetName), name)
                {
                    Position = MathUtil.ParsePoint(position)
                };

                map.AddSprite(sprite);
            }

            return map;
        }
    }
}
