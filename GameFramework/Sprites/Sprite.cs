using System;

using GameFramework.Cameras;
using GameFramework.Scenes;

namespace GameFramework.Sprites
{
    public class Sprite : SpriteBase, IHitTarget
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

        public HitBase GetHit(Vector position, ICamera camera, WorldTransform worldTransform)
        {
            return this.SpriteSheet.GetHit(position, camera, worldTransform, this);
        }
    }
}
