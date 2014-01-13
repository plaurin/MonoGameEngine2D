using System;
using System.Linq;
using System.Xml.Linq;
using GameFramework.Drawing;

namespace GameFramework.Repository
{
    public class DrawingXmlRepository
    {
        public static XElement ToXml(DrawingMap drawingMap)
        {
            return new XElement("DrawingMap", XmlRepository.MapBaseToXml(drawingMap),
                new XElement("Elements",
                    drawingMap.Elements.Select(e => e.ToXml())));
        }

        public static DrawingMap DrawingMapFromXml(GameResourceManager gameResourceManager, XElement mapElement)
        {
            var name = mapElement.Attribute("name").Value;
            var map = new DrawingMap(name, gameResourceManager);
            map.BaseFromXml(mapElement);

            foreach (var element in mapElement.Element("Elements").Elements())
            {
                switch (element.Name.ToString())
                {
                    case "TextElement":
                        map.AddElement(TextElement.FromXml(gameResourceManager, element));
                        break;
                    case "LineElement":
                        map.AddElement(LineElement.FromXml(gameResourceManager, element));
                        break;
                    case "PolygonElement":
                        map.AddElement(PolygonElement.FromXml(gameResourceManager, element));
                        break;
                    default:
                        throw new NotImplementedException(element.Name + " is not implemented yet.");
                }
            }

            return map;
        }
    }
}