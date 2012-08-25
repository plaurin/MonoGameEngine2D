using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using ClassLibrary.Sheets;

namespace ClassLibrary.Hexes
{
    public class HexSheet : SheetBase
    {
        private readonly Dictionary<string, HexDefinition> definitions;

        public HexSheet(Texture texture, string name, Size hexSize)
            : base(texture, name)
        {
            this.HexSize = hexSize;

            this.definitions = new Dictionary<string, HexDefinition>();
        }

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
            var hexDefinition = new HexDefinition(this, hexName, rectangle);

            this.Definitions.Add(hexName, hexDefinition);
            return hexDefinition;
        }

        public void AddHexDefinition(HexDefinition hexDefinition)
        {
            this.Definitions.Add(hexDefinition.Name, hexDefinition);
        }

        public virtual void Draw(DrawContext drawContext, HexDefinition hexDefinition, Rectangle destination)
        {
            drawContext.DrawImage(this.Texture, hexDefinition.Rectangle, destination);
        }

        protected override IEnumerable<object> GetXml()
        {
            yield return new XElement("HexSize", this.HexSize);
            yield return new XElement("Definitions", this.Definitions.Select(d => d.Value.GetXml()));
        }

        public static SheetBase FromXml(XElement sheetElement, string name, Texture texture)
        {
            var hexSize = MathUtil.ParseSize(sheetElement.Element("HexSize").Value);
            var hexSheet = new HexSheet(texture, name, hexSize);

            foreach (var definitionElement in sheetElement.Elements("Definitions").Elements())
            {
                var hexDefinition = HexDefinition.FromXml(definitionElement, hexSheet);
                hexSheet.AddHexDefinition(hexDefinition);
            }

            return hexSheet;
        }
    }
}
