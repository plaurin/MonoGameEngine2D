using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Sprites
{
    public class SpriteAnimationTemplate : ISpriteTemplate<SpriteAnimation>
    {
        private readonly string name;
        private readonly List<SpriteAnimationFrameTemplate> frames;

        public SpriteAnimationTemplate(string name, params SpriteAnimationFrameTemplate[] frames)
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

        public SpriteAnimationTemplate AddFrame(SpriteDefinition frameSprite, float duration)
        {
            this.frames.Add(new SpriteAnimationFrameTemplate(frameSprite, duration));
            return this;
        }

        public SpriteAnimation CreateInstance()
        {
            return new SpriteAnimation(this.name, this.frames.Select(f => f.CreateInstance()).ToArray());
        }
    }
}