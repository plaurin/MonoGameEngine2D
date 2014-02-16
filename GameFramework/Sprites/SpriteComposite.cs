using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework.Cameras;

namespace GameFramework.Sprites
{
    public class SpriteComposite : SpriteBase, IUpdatable
    {
        private readonly List<SpriteBase> spritesT;

        public SpriteComposite(string name, IEnumerable<SpriteBase> spritesT)
        {
            this.SpriteName = name;

            this.spritesT = new List<SpriteBase>(spritesT);
        }

        public IEnumerable<SpriteBase> Children
        {
            get { return this.spritesT; }
        }

        public override int Draw(IDrawContext drawContext, Vector layerOffset, Vector parallaxScrollingVector, CameraMode cameraMode)
        {
            var count = 0;

            // TODO: Fix this with CompositeSprite that adjust underlying sprite position and rotation
            var compositeOffset = layerOffset.Translate(this.Position);

            foreach (var sprite in this.Children)
                count += sprite.Draw(drawContext, compositeOffset, parallaxScrollingVector, cameraMode);

            return count;
        }

        public void Update(IGameTiming gameTiming)
        {
            foreach (var updatable in this.spritesT.OfType<IUpdatable>())
                updatable.Update(gameTiming);
        }
    }
}