using System;
using System.Collections.Generic;
using System.Linq;

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

        public override int Draw(IDrawContext drawContext, SpriteTransform transform)
        {
            var newTransform = new SpriteTransform(transform, this.Position, this.Rotation, this.Scale, this.Color);

            return this.Children.Cast<SpriteBase>().Sum(sprite => sprite.Draw(drawContext, newTransform));
        }

        public void Update(IGameTiming gameTiming)
        {
            foreach (var updatable in this.sprites.OfType<IUpdatable>())
                updatable.Update(gameTiming);
        }
    }
}