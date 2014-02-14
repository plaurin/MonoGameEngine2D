using System;
using System.Collections.Generic;
using System.Linq;

using GameFramework.Cameras;
using GameFramework.Layers;
using GameFramework.Scenes;

namespace GameFramework.Sprites
{
    public class SpriteLayer : LayerBase, IHitTarget
    {
        private readonly List<SpriteBase> sprites;
        private int drawnElementsLastFrame;

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

        public override int TotalElements
        {
            // TODO: Fix for CompositeSprite which is more than 1!
            get { return this.sprites.Count; }
        }

        public override int DrawnElementsLastFrame
        {
            get { return this.drawnElementsLastFrame; }
        }

        public override int Draw(IDrawContext drawContext)
        {
            this.drawnElementsLastFrame = 0;

            foreach (var sprite in this.Sprites)
            {
                this.drawnElementsLastFrame += 
                    sprite.Draw(drawContext, this.Offset, this.ParallaxScrollingVector, this.CameraMode);
            }

            return this.drawnElementsLastFrame;
        }

        public HitBase GetHit(Vector position, ICamera camera, WorldTransform worldTransform)
        {
            var newTransform = worldTransform.Compose(this.Offset, this.ParallaxScrollingVector);

            return this.Sprites.OfType<IHitTarget>()
                .Select(sprite => sprite.GetHit(position, camera, newTransform))
                .FirstOrDefault(spriteHit => spriteHit != null);
        }
    }
}
