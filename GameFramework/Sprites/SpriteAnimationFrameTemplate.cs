using System.Collections.Generic;

namespace GameFramework.Sprites
{
    internal class SpriteAnimationFrameTemplate : IComposite, INavigatorMetadataProvider
    {
        internal SpriteAnimationFrameTemplate(SpriteDefinition frameSprite, float duration, 
            Vector? position = null, float rotation = 0.0f, Color? color = null)
        {
            this.FrameSprite = frameSprite;
            this.Duration = duration;
            this.Position = position.HasValue ? position.Value : Vector.Zero;
            this.Rotation = rotation;
            this.Color = color.HasValue ? color.Value : Color.White;
        }

        public SpriteDefinition FrameSprite { get; private set; }

        public float Duration { get; private set; }

        public Vector Position { get; private set; }

        public float Rotation { get; private set; }

        public Color Color { get; private set; }

        public SpriteAnimationFrame CreateInstance()
        {
            return new SpriteAnimationFrame(this.FrameSprite.CreateInstance(), this.Duration, 
                this.Position, this.Rotation, this.Color);
        }

        public IEnumerable<object> Children
        {
            get { return new[] { this.FrameSprite }; }
        }

        public NavigatorMetadata GetMetadata()
        {
            return new NavigatorMetadata("Frame", NodeKind.Entity);
        }
    }
}