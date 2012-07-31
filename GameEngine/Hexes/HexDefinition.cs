using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Hexes
{
    public class HexDefinition
    {
        private readonly HexSheet sheet;

        public HexDefinition(HexSheet sheet, string name, Rectangle rectangle)
        {
            this.sheet = sheet;
            this.Name = name;
            this.Rectangle = rectangle;
        }

        public string Name { get; private set; }

        public Rectangle Rectangle { get; private set; }

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle destination)
        {
            this.sheet.Draw(spriteBatch, this, destination);
        }
    }
}