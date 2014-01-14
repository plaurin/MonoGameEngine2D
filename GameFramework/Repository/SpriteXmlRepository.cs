using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GameFramework.Sprites;

namespace GameFramework.Repository
{
    public static class SpriteXmlRepository
    {
        public static XElement ToXml(SpriteMap spriteMap)
        {
            return new XElement("SpriteMap",
                XmlRepository.MapBaseToXml(spriteMap),
                new XElement("Sprites", spriteMap.Sprites.OfType<Sprite>().Select(GetXml)));
        }

        public static SpriteMap SpriteMapFromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var mapName = mapElement.Attribute("name").Value;
            var map = new SpriteMap(mapName);
            XmlRepository.BaseFromXml(map, mapElement);

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

        public static IEnumerable<object> GetXml(SpriteSheet spriteSheet)
        {
            yield return new XElement("Definitions", spriteSheet.Definitions.Select(d =>
                new XElement("Definition",
                    new XAttribute("name", d.Key),
                    new XAttribute("rectangle", d.Value))));
        }

        public static SpriteSheet FromXml(XElement sheetElement, string name, Texture texture)
        {
            var spriteSheet = new SpriteSheet(texture, name);

            foreach (var definitionElement in sheetElement.Elements("Definitions").Elements())
            {
                spriteSheet.CreateSpriteDefinition(
                    definitionElement.Attribute("name").Value,
                    MathUtil.ParseRectangle(definitionElement.Attribute("rectangle").Value));
            }

            return spriteSheet;
        }

        private static XElement GetXml(Sprite sprite)
        {
            return new XElement("Sprite",
                new XAttribute("sheetName", sprite.SpriteSheet.Name),
                new XAttribute("name", sprite.SpriteName),
                new XAttribute("position", sprite.Position));
        }
    }
}