using System;
using GameFramework.Audio;
using Microsoft.Xna.Framework.Audio;

namespace MonoGameImplementation.EngineImplementation
{
    internal class XnaSoundEffect : Sound
    {
        private readonly SoundEffect soundEffect;

        public XnaSoundEffect(string name, SoundEffect soundEffect) 
            : base(name)
        {
            this.soundEffect = soundEffect;
        }

        public override TimeSpan Duration
        {
            get { return this.soundEffect.Duration; }
        }

        public override void Play(float volume = 1.0f, float pitch = 0.0f, float pan = 0.0f)
        {
            this.soundEffect.Play(volume, pitch, pan);
        }

        public override SoundInstance CreateInstance()
        {
            return new XnaSoundEffectInstance(this.soundEffect.CreateInstance());
        }

        private class XnaSoundEffectInstance : SoundInstance
        {
            private readonly SoundEffectInstance soundEffectInstance;

            public XnaSoundEffectInstance(SoundEffectInstance soundEffectInstance)
            {
                this.soundEffectInstance = soundEffectInstance;
            }

            public override bool IsLooped
            {
                get { return this.soundEffectInstance.IsLooped; }
                set { this.soundEffectInstance.IsLooped = value; }
            }

            public override bool IsPlaying
            {
                get { return this.soundEffectInstance.State == SoundState.Playing; }
            }

            public override bool IsPaused
            {
                get { return this.soundEffectInstance.State == SoundState.Paused; }
            }

            public override bool IsStopped
            {
                get { return this.soundEffectInstance.State == SoundState.Stopped; }
            }

            public override float Pan
            {
                get { return this.soundEffectInstance.Pan; }
                set { this.soundEffectInstance.Pan = value; }
            }

            public override float Pitch
            {
                get { return this.soundEffectInstance.Pitch; }
                set { this.soundEffectInstance.Pitch = value; }
            }

            public override float Volume
            {
                get { return this.soundEffectInstance.Volume; }
                set { this.soundEffectInstance.Volume = value; }
            }

            public override void Play()
            {
                this.soundEffectInstance.Play();
            }

            public override void Pause()
            {
                this.soundEffectInstance.Pause();
            }

            public override void Stop()
            {
                this.soundEffectInstance.Stop();
            }

            public override void Resume()
            {
                this.soundEffectInstance.Resume();
            }
        }
    }
}