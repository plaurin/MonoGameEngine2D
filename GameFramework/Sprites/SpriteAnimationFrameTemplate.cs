namespace GameFramework.Sprites
{
    public class SpriteAnimationFrameTemplate
    {
        public SpriteAnimationFrameTemplate(SpriteDefinition frameSprite, float duration)
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
    }
}