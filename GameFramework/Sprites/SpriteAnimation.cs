using System;
using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Sprites
{
    public class SpriteAnimation : SpriteBase, IComposite, IUpdatable
    {
        private readonly List<SpriteAnimationFrame> frames;

        private float animationStartTime;
        private SpriteAnimationFrame currentFrame;

        public SpriteAnimation(string name, params SpriteAnimationFrame[] frames)
        {
            this.SpriteName = name;
            this.frames = new List<SpriteAnimationFrame>();

            this.IsVisible = true;

            if (frames != null && frames.Length > 0)
                this.frames.AddRange(frames);

            this.TotalAnimationTime = this.frames.Sum(f => f.Duration);
            this.AnimationState = AnimationState.Running;
        }

        public float AnimationTime { get; private set; }

        public float TotalAnimationTime { get; private set; }

        public AnimationState AnimationState { get; private set; }

        public bool HasCompleted
        {
            get { return this.AnimationTime > this.TotalAnimationTime; }
        }

        public IEnumerable<object> Children
        {
            get { return this.frames; }
        }

        public void StartAnimation()
        {
            this.AnimationState = AnimationState.Starting;
        }

        public void Update(IGameTiming gameTiming)
        {
            if (this.AnimationState == AnimationState.Starting)
            {
                this.animationStartTime = gameTiming.TotalSeconds;
                this.AnimationState = AnimationState.Running;
            }

            this.AnimationTime = gameTiming.TotalSeconds - this.animationStartTime;

            this.currentFrame = this.GetCurrentAnimationFrame(this.AnimationTime % this.TotalAnimationTime);
        }

        public override int Draw(IDrawContext drawContext, Transform transform)
        {
            if (this.currentFrame != null && this.IsVisible)
            {
                var newTransform = new SpriteTransform(transform, this.Position, this.Rotation, this.Scale, this.Color);

                return this.currentFrame.FrameSprite.Draw(drawContext, newTransform);
            }

            return 1;
        }

        private SpriteAnimationFrame GetCurrentAnimationFrame(float animationTime)
        {
            float frameStartTime = 0;
            foreach (var frame in this.frames)
            {
                var frameEndTime = frameStartTime + frame.Duration;

                if (animationTime < frameEndTime)
                    return frame;

                frameStartTime = frameEndTime;
            }

            throw new InvalidOperationException("Animation time " + animationTime + " is over the total animation duration!?!");
        }
    }
}