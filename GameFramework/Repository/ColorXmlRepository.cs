using System.Xml.Linq;
using GameFramework.Maps;

namespace GameFramework.Repository
{
    public class ColorXmlRepository
    {
        public static XElement ToXml(ColorMap colorMap)
        {
            return new XElement("ColorMap",
                new XAttribute("name", colorMap.Name),
                new XElement("Color", colorMap.Color));
        }

        public static ColorMap ColorMapFromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var name = mapElement.Attribute("name").Value;
            var colorValue = mapElement.Element("Color").Value;

            return new ColorMap(name, MathUtil.ParseColor(colorValue));
        }
    }
}