namespace GameFramework.Sprites
{
    public class SpriteAnimationFrame
    {
        public SpriteAnimationFrame(SpriteBase frameSprite, float duration)
        {
            this.FrameSprite = frameSprite;
            this.Duration = duration;
        }

        public SpriteBase FrameSprite { get; private set; }

        public float Duration { get; private set; }
    }
}