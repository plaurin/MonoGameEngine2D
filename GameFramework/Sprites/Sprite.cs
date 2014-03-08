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
            this.ShouldDrawPixelPrecision = false;
        }

        public SpriteSheet SpriteSheet { get; private set; }

        /// <summary>
        /// <remarks>Set only to override the SpriteDefinition Origin</remarks>
        /// </summary>
        public Vector? Origin { get; set; }

        public bool FlipHorizontally { get; set; }

        public bool FlipVertically { get; set; }

        public bool ShouldDrawPixelPrecision { get; set; }

        public override int Draw(IDrawContext drawContext, Transform transform)
        {
            return this.SpriteSheet.Draw(drawContext, this, transform);
        }

        public HitBase GetHit(Vector position, ICamera camera, WorldTransform worldTransform)
        {
            return this.SpriteSheet.GetHit(position, camera, worldTransform, this);
        }
    }
}
