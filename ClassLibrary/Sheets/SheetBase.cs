using System;
using System.Collections.Generic;
using System.Xml.Linq;

using ClassLibrary.Hexes;
using ClassLibrary.Sprites;
using ClassLibrary.Tiles;

namespace ClassLibrary.Sheets
{
    public abstract class SheetBase
    {
        protected SheetBase(Texture texture, string name)
        {
            this.Texture = texture;
            this.Name = name;
        }

        public Texture Texture { get; private set; }

        public string Name { get; private set; }

        public void Save(string path)
        {
            var document = new XDocument();

            document.Add(this.ToXml());

            document.Save(path);
        }

        public XElement ToXml()
        {
            return new XElement(this.GetType().Name,
                new XAttribute("name", this.Name),
                new XElement("Texture", this.Texture.Name),
                this.GetXml());
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
                    return SpriteSheet.FromXml(sheetElement, name, texture) as T;
                case "TileSheet":
                    return TileSheet.FromXml(sheetElement, name, texture) as T;
                case "HexSheet":
                    return HexSheet.FromXml(sheetElement, name, texture) as T;
                default:
                    throw new InvalidOperationException(sheetElement.Name + " is not a valid sheet type.");
            }
        }
        
        protected abstract IEnumerable<object> GetXml();
    }
}