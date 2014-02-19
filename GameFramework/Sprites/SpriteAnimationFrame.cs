using System.Collections.Generic;

namespace GameFramework.Sprites
{
    public class SpriteAnimationFrame : IComposite, INavigatorMetadataProvider
    {
        internal SpriteAnimationFrame(SpriteBase frameSprite, float duration, SpriteTransform transform = null)
        {
            this.FrameSprite = frameSprite;
            this.Duration = duration;
            this.Transform = transform;
        }

        public SpriteBase FrameSprite { get; private set; }

        public float Duration { get; set; }

        public SpriteTransform Transform { get; internal set; }

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