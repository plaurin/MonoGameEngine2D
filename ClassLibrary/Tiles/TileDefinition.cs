using System;
using System.Xml.Linq;

namespace ClassLibrary.Tiles
{
    public class TileDefinition
    {
        public TileDefinition(TileSheet sheet, string name, Rectangle rectangle)
        {
            this.Sheet = sheet;
            this.Name = name;
            this.Rectangle = rectangle;
        }

        public TileSheet Sheet { get; private set; }

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
        //public virtual void Draw(DrawContext drawContext, Rectangle destination)
        //{
        //    //this.Sheet.Draw(spriteBatch, this, destination);
        //}

        public XElement GetXml()
        {
            return new XElement("TileDefinition",
                new XAttribute("name", this.Name),
                new XAttribute("rectangle", this.Rectangle));
        }
    }
}