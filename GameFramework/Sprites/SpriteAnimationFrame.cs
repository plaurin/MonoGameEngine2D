using System.Collections.Generic;

namespace GameFramework.Sprites
{
    public class SpriteAnimationFrame : IComposite, INavigatorMetadataProvider
    {
        public SpriteAnimationFrame(SpriteBase frameSprite, float duration)
        {
            this.FrameSprite = frameSprite;
            this.Duration = duration;
        }

        public SpriteBase FrameSprite { get; private set; }

        public float Duration { get; private set; }

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