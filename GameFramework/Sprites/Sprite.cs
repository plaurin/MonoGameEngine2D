using System;

using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Sprites
{
    public class Sprite : SpriteBase
    {
        public Sprite(SpriteSheet spriteSheet, string spriteName)
        {
            this.SpriteSheet = spriteSheet;
            this.SpriteName = spriteName;
        }

        public SpriteSheet SpriteSheet { get; private set; }

        public override void Draw(DrawContext drawContext, Camera camera, Vector layerOffset, Vector parallaxScrollingVector)
        {
            this.SpriteSheet.Draw(drawContext, camera, layerOffset, parallaxScrollingVector, this);
        }

        public override HitBase GetHit(Vector position, Camera camera, Vector layerOffset, Vector parallaxScrollingVector)
        {
            return this.SpriteSheet.GetHit(position, camera, layerOffset, parallaxScrollingVector, this);
        }
    }
}
