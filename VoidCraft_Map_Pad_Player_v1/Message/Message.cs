using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Message {
    // Pojedyńcza wiadomość
    class Message {
        public Rectangle MessagesPosition; // Pozycja wiadomości
        Texture2D MessageTexture; // Tło wiadomośći
        SpriteFont Font; // Czcionka wiadomości
        string MessageText; // Tekst wiadomości
        bool MessageType = true; // Czy wiadomość jest teksowa czy z tekstury

        // Wiadomość z tekstury
        public Message(Rectangle MessagesPosition, Texture2D MessageTexture) {
            this.MessagesPosition = MessagesPosition;
            this.MessageTexture = MessageTexture;
            MessageType = true;
        }

        // Wiadomość tekstowa
        public Message(Rectangle MessagesPosition, string MessageText, SpriteFont Font, Texture2D MessageBackground) {
            this.MessagesPosition = MessagesPosition;
            this.MessageTexture = MessageBackground;
            this.MessageText = MessageText;
            this.Font = Font;
            MessageType = false;
        }

        // Rysowanie wiadomości
        public void Draw(SpriteBatch spriteBatch) {
            if (MessageType)
                spriteBatch.Draw(MessageTexture, MessagesPosition, Color.Black);
            else {
                spriteBatch.Draw(MessageTexture, MessagesPosition, Color.Black);
                spriteBatch.DrawString(Font, MessageText, new Vector2(MessagesPosition.X + 20, MessagesPosition.Y + 20), Color.White);
            }
        }
    }
}