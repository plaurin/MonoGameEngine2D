using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Sprites
{
    public class SpriteMap
    {
        private readonly List<Sprite> sprites;

        public SpriteMap()
        {
            this.sprites = new List<Sprite>();
        }

        public void AddSprite(Sprite sprite)
        {
            this.sprites.Add(sprite);
        }

        public void RemoveSprite(Sprite sprite)
        {
            this.sprites.Remove(sprite);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var sprite in sprites)
            {
                sprite.Draw(spriteBatch);
            }
        }
    }
}
