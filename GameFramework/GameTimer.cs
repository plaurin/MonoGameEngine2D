using System;

namespace GameFramework
{
    public class GameTimer : IGameTiming
    {
        private float elapseTime;

        private long frameCounter;

        public float ElapsedSeconds { get; set; }

        public float TotalSeconds { get; set; }

        public long Fps { get; private set; }

        public void Update(TimeSpan elapsedGameTime, TimeSpan totalGameTime)
        {
            this.ElapsedSeconds = (float)elapsedGameTime.TotalSeconds;
            this.TotalSeconds = (float)totalGameTime.TotalSeconds;

            this.elapseTime += (float)elapsedGameTime.TotalSeconds;
            this.frameCounter++;

            if (this.elapseTime > 1)
            {
                this.Fps = this.frameCounter;
                this.frameCounter = 0;
                this.elapseTime = 0;
            }
        }
    }

    public interface IGameTiming
    {
        float ElapsedSeconds { get; set; }
        float TotalSeconds { get; set; }
        long Fps { get; }
    }
}
