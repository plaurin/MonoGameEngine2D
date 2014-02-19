using System.Collections.Generic;

namespace GameFramework.Sprites
{
    public class SpriteAnimationFrame : IComposite, INavigatorMetadataProvider
    {
        internal SpriteAnimationFrame(SpriteBase frameSprite, float duration,
            Vector position, float rotation, Color color)
        {
            this.FrameSprite = frameSprite;
            this.Duration = duration;
            this.Position = position;
            this.Rotation = rotation;
            this.Color = color;
        }

        public SpriteBase FrameSprite { get; private set; }

        public float Duration { get; set; }

        public Vector Position { get; set; }

        public float Rotation { get; set; }

        public Color Color { get; set; }

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