using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ClassLibrary.Hexes
{
    public class HexSheet
    {
        private readonly Dictionary<string, HexDefinition> definitions;

        public HexSheet(Texture texture, string name, Size hexSize)
        {
            this.Name = name;
            this.HexSize = hexSize;
            this.Texture = texture;

            this.definitions = new Dictionary<string, HexDefinition>();
        }

        public string Name { get; set; }

        public Texture Texture { get; private set; }

        public Size HexSize { get; set; }

        public Dictionary<string, HexDefinition> Definitions
        {
            get
            {
                return this.definitions;
            }
        }

        public HexDefinition CreateHexDefinition(string hexName, Point hexPosition)
        {
            var rectangle = new Rectangle(hexPosition.X, hexPosition.Y, this.HexSize.Width, this.HexSize.Height);
            //var hexDefinition = this.CreateHexDefinition(this, hexName, rectangle);
            var hexDefinition = new HexDefinition(this, hexName, rectangle);

            this.Definitions.Add(hexName, hexDefinition);
            return hexDefinition;
        }

        public void AddHexDefinition(HexDefinition hexDefinition)
        {
            this.Definitions.Add(hexDefinition.Name, hexDefinition);
        }

        //public abstract void Draw(DrawContext drawContext, HexDefinition hexDefinition, Rectangle destination);
        public virtual void Draw(DrawContext drawContext, HexDefinition hexDefinition, Rectangle destination)
        {
            //spriteBatch.Draw(this.texture, destination, hexDefinition.Rectangle, Color.White);
            drawContext.DrawImage(this.Texture, hexDefinition.Rectangle, destination);
        }

        public XElement GetXml()
        {
            return new XElement("HexSheet",
                new XAttribute("name", this.Name),
                new XElement("Texture", this.Texture.Name),
                new XElement("HexSize", this.HexSize),
                new XElement("Definitions", this.Definitions.Select(d => d.Value.GetXml())));
        }

        //protected abstract HexDefinition CreateHexDefinition(HexSheet hexSheet, string hexName, Rectangle rectangle);
    }
}
