using System.Collections.Generic;

namespace GameFramework.Sprites
{
    internal class SpriteAnimationFrameTemplate : IComposite, INavigatorMetadataProvider
    {
        internal SpriteAnimationFrameTemplate(SpriteDefinition frameSprite, float duration)
        {
            this.FrameSprite = frameSprite;
            this.Duration = duration;
        }

        public SpriteDefinition FrameSprite { get; private set; }

        public float Duration { get; private set; }

        public SpriteAnimationFrame CreateInstance()
        {
            return new SpriteAnimationFrame(this.FrameSprite.CreateInstance(), this.Duration);
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