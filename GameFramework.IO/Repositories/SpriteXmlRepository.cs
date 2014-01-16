using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GameFramework.Sprites;

namespace GameFramework.IO.Repositories
{
    public static class SpriteXmlRepository
    {
        public static XElement ToXml(SpriteLayer spriteLayer)
        {
            return new XElement("SpriteLayer",
                XmlRepository.LayerBaseToXml(spriteLayer),
                new XElement("Sprites", spriteLayer.Sprites.OfType<Sprite>().Select(GetXml)));
        }

        public static SpriteLayer SpriteLayerFromXml(GameResourceManager gameResourceManager, XElement layerElement)
        {
            var layerName = layerElement.Attribute("name").Value;
            var layer = new SpriteLayer(layerName);
            XmlRepository.BaseFromXml(layer, layerElement);

            foreach (var element in layerElement.Element("Sprites").Elements())
            {
                var sheetName = element.Attribute("sheetName").Value;
                var name = element.Attribute("name").Value;
                var position = element.Attribute("position").Value;

                var sprite = new Sprite(gameResourceManager.GetSpriteSheet(sheetName), name)
                {
                    Position = MathUtil.ParseVector(position)
                };

                layer.AddSprite(sprite);
            }

            return layer;
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