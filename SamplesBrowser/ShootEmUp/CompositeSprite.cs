using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Sprites;

namespace SamplesBrowser.ShootEmUp
{
    public class CompositeSprite : SpriteBase
    {
        private readonly List<SpriteBase> sprites;

        public CompositeSprite()
        {
            this.sprites = new List<SpriteBase>();
        }

        public CompositeSprite(IEnumerable<SpriteBase> sprites)
            : this()
        {
            this.sprites.AddRange(sprites);
        }

        public void AddSprite(SpriteBase sprite)
        {
            this.sprites.Add(sprite);
        }

        public override int Draw(IDrawContext drawContext, Vector layerOffset, Vector parallaxScrollingVector,
            CameraMode cameraMode)
        {
            var drawCount = 0;

            var compositeOffset = layerOffset.Translate(this.Position);
            foreach (var sprite in this.sprites)
            {
                drawCount += sprite.Draw(drawContext, compositeOffset, parallaxScrollingVector, cameraMode);
            }

            return drawCount;
        }
    }
}