using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;
using WindowsGame1.Maps;

namespace WindowsGame1.Sprites
{
    public class SpriteMap : MapBase
    {
        private readonly List<Sprite> sprites;

        public SpriteMap()
        {
            this.sprites = new List<Sprite>();
        }

        public void AddSprite(Sprite sprite)
        {
            this.sprites.Add(sprite);
        }

        public void RemoveSprite(Sprite sprite)
        {
            this.sprites.Remove(sprite);
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (var sprite in this.sprites)
            {
                sprite.Draw(spriteBatch, camera, this.ParallaxScrollingVector);
            }
        }

        public override XElement ToXml()
        {
            return new XElement("SpriteMap",
                this.BaseToXml(),
                new XElement("Sprites", this.sprites.Select(s => s.GetXml())));
        }

        public static SpriteMap FromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var map = new SpriteMap();
            map.BaseFromXml(mapElement);

            foreach(var element in mapElement.Element("Sprites").Elements())
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
