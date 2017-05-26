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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PadControler
{
    class PadButton
    {
        public Rectangle Position { get; set; }
        public Texture2D Bitmap { get; set; }
        public bool Pressed { get; set; }
        public GamePadStatus ButonType { get; set; }

        public PadButton(GamePadStatus ButtonType, GraphicsDevice graphicsDevice, string Path, Rectangle Position)
        {
            this.ButonType = ButtonType;
            this.Position = Position;
            this.Pressed = false;
            using (var stream = TitleContainer.OpenStream(Path))
            {
                Bitmap = Texture2D.FromStream(graphicsDevice, stream);
            }
        }


    }
}