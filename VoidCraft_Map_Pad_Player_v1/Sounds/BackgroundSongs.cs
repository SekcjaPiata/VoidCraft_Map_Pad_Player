using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;

namespace Sounds {
    class BackgroundSongs {
        Song song;
        public string Name { get; }
        ContentManager Content;

        public BackgroundSongs(Song song, ContentManager Content, bool IsRepeating, float Volume, string Name) {
            this.song = song;
            Repeating(IsRepeating);
            ChangeVolume(Volume);
            this.Name = Name;
        }

        public BackgroundSongs(string SongPath, ContentManager Content, bool IsRepeating, float Volume, string Name) {
            this.Content = Content;
            this.song = Content.Load<Song>(SongPath);

            Repeating(IsRepeating);
            ChangeVolume(Volume);
            this.Name = Name;
        }

        public void ChangeSong(string SongPath) {
            Stop();
            this.song = Content.Load<Song>(SongPath);
            Play();
        }

        public void Repeating(bool Repeating) {
            MediaPlayer.IsRepeating = Repeating;
        }
        public void ChangeVolume(float Volume) {
            MediaPlayer.Volume = Volume;
        }

        public void Play() {
            MediaPlayer.Play(song);
        }
        public void Stop() {
            MediaPlayer.Stop();
        }

        public void Resume() {
            MediaPlayer.Resume();
        }
        public void Pause() {
            MediaPlayer.Pause();
        }
    }
}