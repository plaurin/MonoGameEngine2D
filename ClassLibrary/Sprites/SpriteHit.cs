using System;

using GameFramework.Scenes;

namespace GameFramework.Sprites
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
            return "Sprite: " + this.Sprite.SpriteName;
        }
    }
}