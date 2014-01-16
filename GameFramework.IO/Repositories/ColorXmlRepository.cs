using System.Xml.Linq;
using GameFramework.Layers;

namespace GameFramework.IO.Repositories
{
    public class ColorXmlRepository
    {
        public static XElement ToXml(ColorLayer colorLayer)
        {
            return new XElement("ColorLayer",
                new XAttribute("name", colorLayer.Name),
                new XElement("Color", colorLayer.Color));
        }

        public static ColorLayer ColorLayerFromXml(XElement mapElement)
        {
            var name = mapElement.Attribute("name").Value;
            var colorValue = mapElement.Element("Color").Value;

            return new ColorLayer(name, MathUtil.ParseColor(colorValue));
        }
    }
}