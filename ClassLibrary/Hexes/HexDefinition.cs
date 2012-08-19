using System;
using System.Xml.Linq;

namespace ClassLibrary.Hexes
{
    public abstract class HexDefinition
    {
        protected HexDefinition(HexSheet sheet, string name, Rectangle rectangle)
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

        //public abstract void Draw(DrawContext drawContext, Rectangle destination);
        public void Draw(DrawContext drawContext, Rectangle destination)
        {
            this.Sheet.Draw(drawContext, this, destination);
        }

        public object GetXml()
        {
            return new XElement("HexDefinition",
                new XAttribute("name", this.Name),
                new XAttribute("rectangle", this.Rectangle));
        }
    }
}