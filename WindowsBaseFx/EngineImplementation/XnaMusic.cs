using System;
using GameFramework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MonoGameImplementation.EngineImplementation
{
    public class XnaMusic : Music
    {
        private readonly Song song;

        public XnaMusic(string assetName, Song song)
            : base(assetName)
        {
            this.song = song;
        }

        public override TimeSpan Duration
        {
            get { return this.song.Duration; }
        }

        public override bool IsRepeating
        {
            get { return MediaPlayer.IsRepeating; }
            set { MediaPlayer.IsRepeating = value; }
        }

        public override bool IsPlaying
        {
            get { return MediaPlayer.State == MediaState.Playing; }
        }

        public override bool IsPaused
        {
            get { return MediaPlayer.State == MediaState.Paused; }
        }

        public override bool IsStopped
        {
            get { return MediaPlayer.State == MediaState.Stopped; }
        }

        public override TimeSpan PlayPosition
        {
            get { return MediaPlayer.PlayPosition; }
        }

        public override float Volume
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = value; }
        }

        public override void Play()
        {
            MediaPlayer.Play(this.song);
        }

        public override void Pause()
        {
            MediaPlayer.Pause();
        }

        public override void Resume()
        {
            MediaPlayer.Resume();
        }

        public override void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}