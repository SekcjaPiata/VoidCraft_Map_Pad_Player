using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Sounds {
    // Klasa obsługująca muzyke tła
    class BackgroundSongs {
        Song song; // Muzyka
        public string Name { get; } // Nazwa piosenki
        ContentManager Content;

        // Dodanie nowej piosenki
        public BackgroundSongs(Song song, ContentManager Content, bool IsRepeating, float Volume, string Name) {
            this.song = song;
            Repeating(IsRepeating);
            ChangeVolume(Volume);
            this.Name = Name;
        }

        // Wczyanie nowej piosenki z pliku
        public BackgroundSongs(string SongPath, ContentManager Content, bool IsRepeating, float Volume, string Name) {
            this.Content = Content;
            this.song = Content.Load<Song>(SongPath);

            Repeating(IsRepeating);
            ChangeVolume(Volume);
            this.Name = Name;
        }

        // Zmiana piosenki
        public void ChangeSong(string SongPath) {
            Stop();
            this.song = Content.Load<Song>(SongPath);
            Play();
        }

        // Czy piosenka ma być powatarzana
        public void Repeating(bool Repeating) {
            MediaPlayer.IsRepeating = Repeating;
        }

        // Głośność piosenki
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