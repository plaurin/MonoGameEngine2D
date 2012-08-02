using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Sprites
{
    public class Sprite
    {
        public Sprite(SpriteSheet spriteSheet, string spriteName)
        {
            this.SpriteSheet = spriteSheet;
            this.SpriteName = spriteName;
        }

        public SpriteSheet SpriteSheet { get; private set; }

        public string SpriteName { get; private set; }

        public Point Position { get; set; }

        //public Size Size { get; set; }

        //public Rectangle Rectangle
        //{
        //    get { return new Rectangle(this.Position.X, this.Position.Y, this.Size.Width, this.Size.Height); }
        //}

        public void Draw(SpriteBatch spriteBatch, float scaling)
        {
            this.SpriteSheet.Draw(spriteBatch, this, scaling);
        }
    }
}
