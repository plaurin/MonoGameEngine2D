namespace GameFramework.Audio
{
    public abstract class SoundInstance
    {
        public abstract bool IsLooped { get; set; }

        public abstract float Pan { get; set; }

        public abstract float Pitch { get; set; }

        public abstract float Volume { get; set; }

        public abstract bool IsPlaying { get; }

        public abstract bool IsPaused { get; }

        public abstract bool IsStopped { get; }

        public abstract void Play();

        public abstract void Pause();

        public abstract void Stop();

        public abstract void Resume();
    }
}