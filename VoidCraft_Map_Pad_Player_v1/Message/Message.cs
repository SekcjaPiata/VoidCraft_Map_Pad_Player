using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;

namespace Message {
    class Message {

        public Rectangle MessagesPosition;
        Texture2D MessageTexture;
        SpriteFont Font;
        string MessageText;
        bool MessageType = true;


        public Message(Rectangle MessagesPosition, Texture2D MessageTexture) {
            this.MessagesPosition = MessagesPosition;
            this.MessageTexture = MessageTexture;
            MessageType = true;
        }

        public Message(Rectangle MessagesPosition, string MessageText, SpriteFont Font, Texture2D MessageBackground) {
            this.MessagesPosition = MessagesPosition;
            this.MessageTexture = MessageBackground;
            this.MessageText = MessageText;
            this.Font = Font;
            MessageType = false;
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (MessageType)
                spriteBatch.Draw(MessageTexture, MessagesPosition, Color.Black);
            else {
                spriteBatch.Draw(MessageTexture, MessagesPosition, Color.Black);
                spriteBatch.DrawString(Font, MessageText, new Vector2(MessagesPosition.X + 20, MessagesPosition.Y + 20), Color.White);
            }
        }

        internal void Draw(SpriteBatch spriteBatch, Rectangle buffor) {
            if (MessageType)
                spriteBatch.Draw(MessageTexture, buffor, Color.Black);
            else {
                spriteBatch.Draw(MessageTexture, buffor, Color.Black);
                spriteBatch.DrawString(Font, MessageText, new Vector2(buffor.X + 20, buffor.Y + 20), Color.White);
            }
        }
    }
}