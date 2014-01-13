using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using GameFramework.Sheets;

namespace GameFramework.Hexes
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
    }
}
