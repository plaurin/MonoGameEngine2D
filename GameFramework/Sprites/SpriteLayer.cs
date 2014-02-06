using System;
using System.Collections.Generic;
using System.Linq;

using GameFramework.Cameras;
using GameFramework.Layers;
using GameFramework.Scenes;

namespace GameFramework.Sprites
{
    public class SpriteLayer : LayerBase
    {
        private readonly List<SpriteBase> sprites;

        public SpriteLayer(string name)
            : base(name)
        {
            this.sprites = new List<SpriteBase>();
        }

        public IEnumerable<SpriteBase> Sprites
        {
            get { return this.sprites; }
        }

        public void AddSprite(SpriteBase sprite)
        {
            this.sprites.Add(sprite);
        }

        public void RemoveSprite(SpriteBase sprite)
        {
            this.sprites.Remove(sprite);
        }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            foreach (var sprite in this.Sprites)
            {
                sprite.Draw(drawContext, camera, this.Offset, this.ParallaxScrollingVector, this.CameraMode);
            }
        }

        public override HitBase GetHit(Vector position, Camera camera)
        {
            return this.Sprites
                .Select(sprite => sprite.GetHit(position, camera, this.Offset, this.ParallaxScrollingVector))
                .FirstOrDefault(spriteHit => spriteHit != null);
        }
    }
}
