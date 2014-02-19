using System;
using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Sprites
{
    public class SpriteAnimation : SpriteBase, IComposite, IUpdatable
    {
        private readonly List<SpriteAnimationFrame> frames;
        private readonly float totalAnimationTime;

        private float animationStartTime;
        private SpriteAnimationFrame currentFrame;
        private AnimationState animationState = AnimationState.Running;

        private enum AnimationState
        {
            //Stopped,
            Starting,
            Running
        }

        public SpriteAnimation(string name, params SpriteAnimationFrame[] frames)
        {
            this.SpriteName = name;
            this.frames = new List<SpriteAnimationFrame>();

            this.IsVisible = true;

            if (frames != null && frames.Length > 0)
                this.frames.AddRange(frames);

            this.totalAnimationTime = this.frames.Sum(f => f.Duration);
        }

        public float AnimationTime { get; private set; }

        public bool HasCompleted
        {
            get { return this.AnimationTime > this.totalAnimationTime; }
        }

        public IEnumerable<object> Children
        {
            get { return this.frames; }
        }

        public void StartAnimation()
        {
            this.animationState = AnimationState.Starting;
        }

        public void Update(IGameTiming gameTiming)
        {
            if (this.animationState == AnimationState.Starting)
            {
                this.animationStartTime = gameTiming.TotalSeconds;
                this.animationState = AnimationState.Running;
            }

            this.AnimationTime = gameTiming.TotalSeconds - this.animationStartTime;

            this.currentFrame = this.GetCurrentAnimationFrame(this.AnimationTime % this.totalAnimationTime);
        }

        public override int Draw(IDrawContext drawContext, SpriteTransform transform)
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