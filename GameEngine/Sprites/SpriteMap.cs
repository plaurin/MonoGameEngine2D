using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;
using WindowsGame1.Maps;

namespace WindowsGame1.Sprites
{
    public class SpriteMap : MapBase
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

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (var sprite in this.sprites)
            {
                sprite.Draw(spriteBatch, camera, this.ParallaxScrollingVector);
            }
        }
    }
}
