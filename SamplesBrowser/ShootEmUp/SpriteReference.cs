using System;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Scenes;
using GameFramework.Sprites;

namespace SamplesBrowser.ShootEmUp
{
    public class SpriteReference : SpriteBase
    {
        public SpriteReference()
        {
        }

        public SpriteReference(SpriteBase sprite)
        {
            this.ReferencedSprite = sprite;
        }

        public SpriteBase ReferencedSprite { get; set; }

        public override int Draw(IDrawContext drawContext, Vector layerOffset, Vector parallaxScrollingVector,
            CameraMode cameraMode)
        {
            if (this.ReferencedSprite != null)
            {
                var referenceOffset = layerOffset.Translate(this.Position);
                return this.ReferencedSprite.Draw(drawContext, referenceOffset, parallaxScrollingVector, cameraMode);
            }

            return 0;
        }

        public override HitBase GetHit(Vector position, Camera camera, Vector layerOffset, Vector parallaxScrollingVector)
        {
            throw new NotImplementedException();
        }
    }
}