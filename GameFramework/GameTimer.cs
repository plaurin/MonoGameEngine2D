using System;

namespace GameFramework
{
    public class GameTimer : IGameTiming
    {
        private readonly InnerTimer updateTimer;
        private readonly InnerTimer drawTimer;

        public GameTimer()
        {
            this.updateTimer = new InnerTimer();
            this.drawTimer = new InnerTimer();
        }

        public float ElapsedSeconds
        {
            get { return this.updateTimer.ElapsedSeconds; }
        }

        public float TotalSeconds
        {
            get { return this.updateTimer.TotalSeconds; }
        }

        public long UpdateFps
        {
            get { return this.updateTimer.Fps; }
        }

        public long DrawFps
        {
            get { return this.drawTimer.Fps; }
        }

        public void Update(TimeSpan elapsedGameTime, TimeSpan totalGameTime)
        {
            this.updateTimer.Update(elapsedGameTime, totalGameTime);
        }

        public void DrawFrame(TimeSpan elapsedGameTime, TimeSpan totalGameTime)
        {
            this.drawTimer.Update(elapsedGameTime, totalGameTime);
        }

        private class InnerTimer
        {
            private float elapseTime;

            private long frameCounter;

            public float ElapsedSeconds { get; private set; }

            public float TotalSeconds { get; private set; }

            public long Fps { get; private set; }

            public void Update(TimeSpan elapsedGameTime, TimeSpan totalGameTime)
            {
                this.ElapsedSeconds = (float)elapsedGameTime.TotalSeconds;
                this.TotalSeconds = (float)totalGameTime.TotalSeconds;

                this.elapseTime += (float)elapsedGameTime.TotalSeconds;

                if (this.elapseTime >= 1)
                {
                    this.Fps = this.frameCounter;
                    this.frameCounter = 0;
                    this.elapseTime -= 1;
                }

                this.frameCounter++;
            }
        }
    }

    public interface IGameTiming
    {
        float ElapsedSeconds { get; }

        float TotalSeconds { get; }

        long UpdateFps { get; }

        long DrawFps { get; }
    }
}
