using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework.Cameras;

namespace GameFramework.Sprites
{
    public class SpriteComposite : SpriteBase, IComposite, IUpdatable
    {
        private readonly List<SpriteBase> sprites;

        public SpriteComposite(string name, IEnumerable<SpriteBase> spritesT)
        {
            this.SpriteName = name;

            this.sprites = new List<SpriteBase>(spritesT);
        }

        public IEnumerable<object> Children
        {
            get { return this.sprites; }
        }

        public override int Draw(IDrawContext drawContext, Vector layerOffset, Vector parallaxScrollingVector, CameraMode cameraMode)
        {
            var count = 0;

            // TODO: Fix this with CompositeSprite that adjust underlying sprite position and rotation
            var compositeOffset = layerOffset.Translate(this.Position);

            foreach (SpriteBase sprite in this.Children)
                count += sprite.Draw(drawContext, compositeOffset, parallaxScrollingVector, cameraMode);

            return count;
        }

        public void Update(IGameTiming gameTiming)
        {
            foreach (var updatable in this.sprites.OfType<IUpdatable>())
                updatable.Update(gameTiming);
        }
    }
}