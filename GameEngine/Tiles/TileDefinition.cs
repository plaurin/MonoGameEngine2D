using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WindowsGame1.Hexes;

namespace WindowsGame1.Tiles
{
    public class TileDefinition
    {
        private readonly TileSheet sheet;

        public TileDefinition(TileSheet sheet, string name, Rectangle rectangle)
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