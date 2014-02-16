using System;
using System.Linq;
using System.Xml.Linq;
using GameFramework.Drawing;

namespace GameFramework.IO.Repositories
{
    public class DrawingXmlRepository
    {
        public static XElement ToXml(DrawingLayer drawingLayer)
        {
            return new XElement("DrawingLayer", XmlRepository.LayerBaseToXml(drawingLayer),
                new XElement("Elements",
                    drawingLayer.Elements.Select(ToXml)));
        }

        public static DrawingLayer DrawingLayerFromXml(XElement layerElement)
        {
            var name = layerElement.Attribute("name").Value;
            var layer = new DrawingLayer(name);
            XmlRepository.BaseFromXml(layer, layerElement);

            foreach (var element in layerElement.Element("Elements").Elements())
            {
                switch (element.Name.ToString())
                {
                    case "TextElement":
                        layer.AddElement(TextElementFromXml(element));
                        break;
                    case "LineElement":
                        layer.AddElement(LineElementFromXml(element));
                        break;
                    case "PolygonElement":
                        layer.AddElement(PolygonElementFromXml(element));
                        break;
                    default:
                        throw new NotImplementedException(element.Name + " is not implemented yet.");
                }
            }

            return layer;
        }

        private static XElement ToXml(DrawingElementBase drawingElement)
        {
            var lineElement = drawingElement as LineElement;
            if (lineElement != null) return ToXml(lineElement);

            var textElement = drawingElement as TextElement;
            if (textElement != null) return ToXml(textElement);

            var polygoneElement = drawingElement as PolygonElement;
            if (polygoneElement != null) return ToXml(polygoneElement);

            throw new NotSupportedException(drawingElement.GetType().Name + " is not supported");
        }

        private static XElement ToXml(LineElement lineElement)
        {
            return new XElement("LineElement",
                new XAttribute("from", lineElement.FromVector),
                new XAttribute("to", lineElement.ToVector),
                new XAttribute("width", lineElement.Width),
                new XAttribute("color", lineElement.Color));
        }

        private static LineElement LineElementFromXml(XElement element)
        {
            var from = element.Attribute("from").Value;
            var to = element.Attribute("to").Value;
            var width = element.Attribute("width").Value;
            var color = element.Attribute("color").Value;

            return new LineElement(MathUtil.ParseVector(from), MathUtil.ParseVector(to), int.Parse(width), MathUtil.ParseColor(color));
        }

        private static XElement ToXml(TextElement textElement)
        {
            return new XElement("TextElement",
                new XAttribute("fontName", textElement.DrawingFont.Name),
                new XAttribute("text", textElement.Text),
                new XAttribute("vector", textElement.Position),
                new XAttribute("color", textElement.Color));
        }

        private static TextElement TextElementFromXml(XElement element)
        {
            var fontName = element.Attribute("fontName").Value;
            var text = element.Attribute("text").Value;
            var vector = element.Attribute("vector").Value;
            var color = element.Attribute("color").Value;

            //var font = gameResourceManager.GetDrawingFont(fontName); OnlyXNA
            var font = new DrawingFont { Name = fontName };
            return new TextElement(font, text, MathUtil.ParseVector(vector), MathUtil.ParseColor(color));
        }

        private static XElement ToXml(PolygonElement polygonElement)
        {
            return new XElement("PolygonElement",
                new XAttribute("width", polygonElement.Width),
                new XAttribute("color", polygonElement.Color),
                new XElement("Vertices", string.Join(", ", polygonElement.Vertices)));
        }

        private static PolygonElement PolygonElementFromXml(XElement element)
        {
            var width = element.Attribute("width").Value;
            var color = element.Attribute("color").Value;
            var vertices = element.Element("Vertices").Value.Split(',').Select(x => MathUtil.ParseVector(x.Trim()));

            return new PolygonElement(vertices, int.Parse(width), MathUtil.ParseColor(color));
        }
    }
}