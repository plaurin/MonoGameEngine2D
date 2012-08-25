using System;
using System.Xml.Linq;

namespace ClassLibrary.Hexes
{
    public class HexDefinition
    {
        public HexDefinition(HexSheet sheet, string name, Rectangle rectangle)
        {
            this.Sheet = sheet;
            this.Name = name;
            this.Rectangle = rectangle;
        }

        public HexSheet Sheet { get; private set; }

        public string SheetName
        {
            get { return this.Sheet.Name; }
        }
        
        public string Name { get; private set; }

        public Rectangle Rectangle { get; private set; }

        public virtual void Draw(DrawContext drawContext, Rectangle destination)
        {
            this.Sheet.Draw(drawContext, this, destination);
        }

        public object GetXml()
        {
            return new XElement("HexDefinition",
                new XAttribute("name", this.Name),
                new XAttribute("rectangle", this.Rectangle));
        }

        public static HexDefinition FromXml(XElement definitionElement, HexSheet hexSheet)
        {
            var name = definitionElement.Attribute("name").Value;
            var rectangle = MathUtil.ParseRectangle(definitionElement.Attribute("rectangle").Value);

            return new HexDefinition(hexSheet, name, rectangle);
        }
    }
}