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

        public override void Draw(DrawContext drawContext, Camera camera, Vector layerOffset, Vector parallaxScrollingVector,
            CameraMode cameraMode)
        {
            if (this.ReferencedSprite != null)
            {
                var referenceOffset = layerOffset.Translate(this.Position);
                this.ReferencedSprite.Draw(drawContext, camera, referenceOffset, parallaxScrollingVector, cameraMode);
            }
        }

        public override HitBase GetHit(Vector position, Camera camera, Vector layerOffset, Vector parallaxScrollingVector)
        {
            throw new NotImplementedException();
        }
    }
}