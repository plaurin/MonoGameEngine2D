using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Sprites
{
    public class SpriteAnimationTemplate : ISpriteTemplate, IComposite, INavigatorMetadataProvider
    {
        private readonly string name;
        private readonly List<SpriteAnimationFrameTemplate> frames;

        internal SpriteAnimationTemplate(string name, params SpriteAnimationFrameTemplate[] frames)
        {
            this.name = name;
            this.frames = new List<SpriteAnimationFrameTemplate>();

            if (frames != null && frames.Length > 0)
                this.frames.AddRange(frames);
        }

        public float TotalAnimationTime
        {
            get { return this.frames.Sum(f => f.Duration); }
        }

        public IEnumerable<object> Children
        {
            get { return this.frames; }
        }

        public SpriteAnimationTemplate AddFrame(SpriteDefinition frameSprite, float duration)
        {
            this.frames.Add(new SpriteAnimationFrameTemplate(frameSprite, duration));
            return this;
        }

        public SpriteBase CreateInstance()
        {
            return new SpriteAnimation(this.name, this.frames.Select(f => f.CreateInstance()).ToArray());
        }

        public NavigatorMetadata GetMetadata()
        {
            // TODO new Kind and Icon
            return new NavigatorMetadata(this.name, NodeKind.Entity);
        }
    }
}