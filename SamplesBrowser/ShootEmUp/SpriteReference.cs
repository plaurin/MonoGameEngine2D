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

        public override void Draw(DrawContext drawContext, Camera camera, Point mapOffset, Vector parallaxScrollingVector)
        {
            if (this.ReferencedSprite != null)
            {
                var referenceOffset = mapOffset.Translate(this.Position);
                this.ReferencedSprite.Draw(drawContext, camera, referenceOffset, parallaxScrollingVector);
            }
        }

        public override HitBase GetHit(Point position, Camera camera, Point mapOffset, Vector parallaxScrollingVector)
        {
            throw new NotImplementedException();
        }
    }
}