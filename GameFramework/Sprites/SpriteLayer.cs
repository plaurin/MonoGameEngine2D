using System;
using System.Collections.Generic;
using System.Linq;

using GameFramework.Cameras;
using GameFramework.Layers;
using GameFramework.Scenes;

namespace GameFramework.Sprites
{
    public class SpriteLayer : LayerBase, IComposite, IHitTarget
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

            SpriteTransform parallaxTransform;
            if (this.CameraMode == CameraMode.Follow)
            {
                parallaxTransform = new SpriteTransform(scale: drawContext.Camera.ZoomFactor,
                    translation: drawContext.Camera.GetSceneTranslationVector(this.ParallaxScrollingVector));
            }
            else
                parallaxTransform = SpriteTransform.Identity;

            var transform = new SpriteTransform(parallaxTransform, this.Offset);

            foreach (var sprite in this.Sprites.Where(s => s.IsVisible))
            {
                this.drawnElementsLastFrame += sprite.Draw(drawContext, transform);
            }

            return this.drawnElementsLastFrame;
        }

        public IEnumerable<object> Children
        {
            get { return this.Sprites; }
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
