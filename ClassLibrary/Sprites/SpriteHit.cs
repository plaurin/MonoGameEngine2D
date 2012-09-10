using System;

using ClassLibrary.Scenes;

namespace ClassLibrary.Sprites
{
    public class SpriteHit : HitBase
    {
        public SpriteHit(Sprite sprite)
        {
            this.Sprite = sprite;
        }

        public Sprite Sprite { get; set; }

        public override string ToString()
        {
            return "Sprite: " + Sprite.SpriteName;
        }
    }
}