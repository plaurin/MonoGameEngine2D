using System;
using System.Collections.Generic;
using System.Xml.Linq;
using GameFramework.Hexes;
using GameFramework.Sheets;
using GameFramework.Sprites;
using GameFramework.Tiles;

namespace GameFramework.Repository
{
    public static class SheetXmlRepository
    {
        public static XElement ToXml(SheetBase sheetBase)
        {
            return new XElement(sheetBase.GetType().Name,
                new XAttribute("name", sheetBase.Name),
                new XElement("Texture", sheetBase.Texture.Name),
                GetXml(sheetBase));
        }

        public static T LoadFrom<T>(GameResourceManager gameResourceManager, string path) where T : SheetBase
        {
            var document = XDocument.Load(path);
            var sheetElement = document.Root;

            var name = sheetElement.Attribute("name").Value;
            var textureName = sheetElement.Element("Texture").Value;

            var texture = gameResourceManager.GetTexture(textureName);

            switch (sheetElement.Name.ToString())
            {
                case "SpriteSheet":
                    return SpriteXmlRepository.FromXml(sheetElement, name, texture) as T;
                case "TileSheet":
                    return TileXmlRepository.FromXml(sheetElement, name, texture) as T;
                case "HexSheet":
                    return HexXmlRepository.FromXml(sheetElement, name, texture) as T;
                default:
                    throw new InvalidOperationException(sheetElement.Name + " is not a valid sheet type.");
            }
        }

        private static IEnumerable<object> GetXml(SheetBase sheetBase)
        {
            var tileSheet = sheetBase as TileSheet;
            if (tileSheet != null) return TileXmlRepository.GetXml(tileSheet);

            var spriteSheet = sheetBase as SpriteSheet;
            if (spriteSheet != null) return SpriteXmlRepository.GetXml(spriteSheet);

            var hexSheet = sheetBase as HexSheet;
            if (hexSheet != null) return HexXmlRepository.GetXml(hexSheet);

            throw new NotSupportedException(sheetBase.GetType().Name + " is not supported");
        }
    }
}