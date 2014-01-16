using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Scenes;
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

        public override void Draw(DrawContext drawContext, Camera camera, Vector layerOffset, Vector parallaxScrollingVector)
        {
            var compositeOffset = layerOffset.Translate(this.Position);
            foreach (var sprite in this.sprites)
            {
                sprite.Draw(drawContext, camera, compositeOffset, parallaxScrollingVector);
            }
        }

        public override HitBase GetHit(Vector position, Camera camera, Vector layerOffset, Vector parallaxScrollingVector)
        {
            throw new NotImplementedException();
        }
    }
}