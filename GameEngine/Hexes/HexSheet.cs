using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Sprites;

namespace WindowsGame1.Hexes
{
    public class HexSheet
    {
        private readonly Texture2D texture;
        private readonly Dictionary<string, HexDefinition> definitions;

        public HexSheet(Texture2D texture, string name, Size hexSize)
        {
            this.Name = name;
            this.HexSize = hexSize;
            this.texture = texture;

            this.definitions = new Dictionary<string, HexDefinition>();
        }

        public string Name { get; set; }

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

        public void Draw(SpriteBatch spriteBatch, HexDefinition hexDefinition, Rectangle destination)
        {
            spriteBatch.Draw(this.texture, destination, hexDefinition.Rectangle, Color.White);
        }

        public XElement GetXml()
        {
            return new XElement("HexSheet",
                new XAttribute("name", this.Name),
                new XElement("Texture", this.texture.Name),
                new XElement("HexSize", this.HexSize),
                new XElement("Definitions", this.Definitions.Select(d => d.Value.GetXml())));
        }
    }
}
