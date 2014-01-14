using System.Xml.Linq;
using GameFramework.Maps;

namespace GameFramework.Repository
{
    public class ImageXmlRepository
    {
        public static XElement ToXml(ImageMap imageMap)
        {
            return new XElement("ImageMap",
                new XAttribute("name", imageMap.Name),
                new XElement("Texture", imageMap.Texture.Name),
                new XElement("Rectangle", imageMap.Rectangle));
        }

        public static ImageMap ImageMapFromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var name = mapElement.Attribute("name").Value;
            var textureName = mapElement.Element("Texture").Value;
            var rectangleValue = mapElement.Element("Rectangle").Value;

            return new ImageMap(name, gameResourceManager.GetTexture(textureName), MathUtil.ParseRectangle(rectangleValue));
        }
    }
}