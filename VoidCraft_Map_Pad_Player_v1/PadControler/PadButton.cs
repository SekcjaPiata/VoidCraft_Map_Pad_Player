using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PadControler {
    // Przycisk pada
    class PadButton {
        public Rectangle Position { get; set; } // Pozycja i rozmiar 
        public Texture2D Bitmap { get; set; } // Obraz przycisku
        public bool Pressed { get; set; } // Czy jest wciœniêty
        public GamePadStatus ButonType { get; set; }// Rodzaj przycisku

        // Konstruktor przycisku
        public PadButton(GamePadStatus ButtonType, GraphicsDevice graphicsDevice, string Path, Rectangle Position) {
            this.ButonType = ButtonType;
            this.Position = Position;
            this.Pressed = false;
            using (var stream = TitleContainer.OpenStream(Path)) {
                Bitmap = Texture2D.FromStream(graphicsDevice, stream);
            }
        }
    }
}