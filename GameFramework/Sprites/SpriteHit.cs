using System;

using GameFramework.Scenes;

namespace GameFramework.Sprites
{
    public class SpriteHit : HitBase
    {
        public SpriteHit(SpriteBase sprite)
        {
            this.Sprite = sprite;
        }

        public SpriteBase Sprite { get; set; }

        public override string ToString()
        {
            return "Sprite: " + this.Sprite.SpriteName;
        }
    }
}