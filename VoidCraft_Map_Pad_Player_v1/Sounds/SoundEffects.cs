using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Sounds {
    class SoundEffects {
        public SoundEffect sound { get; private set; }
        public string Name { get; }
        ContentManager Content;

        public SoundEffects(SoundEffect sound, ContentManager Content, string Name) {
            this.sound = sound;
            this.Name = Name;
        }

        public SoundEffects(string SongPath, ContentManager Content, string Name) {
            this.Content = Content;
            this.sound = Content.Load<SoundEffect>(SongPath);
            this.Name = Name;
        }

        public void ChangeSound(string SongPath) {
            this.sound = Content.Load<SoundEffect>(SongPath);
        }

        public void Play() {
            sound.Play();
        }
    }
}