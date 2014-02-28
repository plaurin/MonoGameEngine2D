using System;

namespace GameFramework.Audio
{
    public abstract class Sound
    {
        protected Sound(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public abstract TimeSpan Duration { get; }

        public abstract void Play(float volume = 1.0f, float pitch = 0.0f, float pan = 0.0f);

        public abstract SoundInstance CreateInstance();
    }
}