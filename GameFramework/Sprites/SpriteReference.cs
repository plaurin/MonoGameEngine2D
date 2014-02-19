namespace GameFramework.Sprites
{
    public class SpriteReference : SpriteBase, IUpdatable
    {
        public SpriteBase Sprite { get; set; }

        public override int Draw(IDrawContext drawContext, SpriteTransform transform)
        {
            if (this.Sprite != null && this.IsVisible)
            {
                var newTransform = new SpriteTransform(transform, this.Position, this.Rotation, this.Scale, this.Color);

                return this.Sprite.Draw(drawContext, newTransform);
            }

            return 0;
        }

        public void Update(IGameTiming gameTiming)
        {
            if (this.Sprite != null)
            {
                var updatable = this.Sprite as IUpdatable;
                if (updatable != null) updatable.Update(gameTiming);
            }
        }
    }
}