using System;

namespace GameFramework.Audio
{
    public abstract class Music
    {
        protected Music(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public abstract TimeSpan Duration { get; }

        public abstract bool IsRepeating { get; set; }
        
        public abstract bool IsPlaying { get; }
        
        public abstract bool IsPaused { get; }
        
        public abstract bool IsStopped { get; }
        
        public abstract TimeSpan PlayPosition { get; }
        
        public abstract float Volume { get; set; }
        
        public abstract void Play();
        
        public abstract void Pause();
        
        public abstract void Resume();
        
        public abstract void Stop();
    }
}