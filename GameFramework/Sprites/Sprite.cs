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

        public override void Draw(DrawContext drawContext, Camera camera, Point mapOffset, Vector parallaxScrollingVector)
        {
            this.SpriteSheet.Draw(drawContext, camera, mapOffset, parallaxScrollingVector, this);
        }

        public override HitBase GetHit(Vector position, Camera camera, Point mapOffset, Vector parallaxScrollingVector)
        {
            return this.SpriteSheet.GetHit(position, camera, mapOffset, parallaxScrollingVector, this);
        }
    }
}
