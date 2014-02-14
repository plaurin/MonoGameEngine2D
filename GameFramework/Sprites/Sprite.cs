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

        public override int Draw(IDrawContext drawContext, Vector layerOffset, Vector parallaxScrollingVector,
            CameraMode cameraMode)
        {
            return this.SpriteSheet.Draw(drawContext, layerOffset, parallaxScrollingVector, this, cameraMode);
        }

        public override HitBase GetHit(Vector position, Camera camera, Vector layerOffset, Vector parallaxScrollingVector)
        {
            return this.SpriteSheet.GetHit(position, camera, layerOffset, parallaxScrollingVector, this);
        }
    }
}
