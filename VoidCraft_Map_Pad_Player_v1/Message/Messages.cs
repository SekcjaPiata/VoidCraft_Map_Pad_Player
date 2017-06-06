using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;

namespace Message {
    public class Messages {
        private List<Message> MessagesList;
        private List<Message> IndependentMessagesList;

        public Rectangle MessagesPosition;
        TouchCollection tl;
        public SpriteFont DefaultFont;
        public Texture2D MessageTexturBg;
        int MessagesLimit;

        Rectangle Buffor;

        public Messages(ContentManager Content, Rectangle MessagesPosition, int MessagesLimit) {
            Load(Content, MessagesPosition);
            ChangeMessageLimit(MessagesLimit);
        }

        public Messages(ContentManager Content, Rectangle MessagesPosition) {
            Load(Content, MessagesPosition);
            ChangeMessageLimit(0);
        }

        private void Load(ContentManager Content, Rectangle MessagesPosition) {
            Buffor = new Rectangle();
            this.MessagesPosition = MessagesPosition;

            MessageTexturBg = Content.Load<Texture2D>("UI\\MessageWnd");
            DefaultFont = Content.Load<SpriteFont>("SpriteFontPL");
            MessagesList = new List<Message>();
            IndependentMessagesList = new List<Message>();
        }

        public void AddMessage(Texture2D MessageTexture, Rectangle MessagesPosition) {
            MessagesList.Reverse();
            if (MessagesList.Count >= MessagesLimit && MessagesLimit > 0)
                MessagesList.RemoveAt(0);


            Buffor.X = MessagesPosition.X;
            Buffor.Y = (MessagesList.Count > 0 ?
                MessagesList [MessagesList.Count - 1].MessagesPosition.Height + 10 + MessagesList [MessagesList.Count - 1].MessagesPosition.Y

                : MessagesPosition.Y);

            Buffor.Width = MessagesPosition.Width;
            Buffor.Height = MessagesPosition.Height;
            MessagesList.Add(new Message(Buffor, MessageTexture));
            MessagesList.Reverse();
        }

        public void AddMessage(string MessageText, Rectangle MessagesPosition) {
            

            Buffor.X = MessagesPosition.X;
            Buffor.Y = (MessagesList.Count > 0 ?
                MessagesList [MessagesList.Count - 1].MessagesPosition.Height + 10 + MessagesList [MessagesList.Count - 1].MessagesPosition.Y

                : MessagesPosition.Y);

            Buffor.Width = MessagesPosition.Width;
            Buffor.Height = MessagesPosition.Height;
            MessagesList.Reverse();
            if (MessagesList.Count >= MessagesLimit && MessagesLimit > 0)
                MessagesList.RemoveAt(0);
            MessagesList.Add(new Message(Buffor, MessageText, DefaultFont, MessageTexturBg));
            MessagesList.Reverse();
        }

        public void CreateIndependentMessage(string MessageText, Rectangle MessagesPosition) /* Ymmmm Derp ... Do Not Use :/  */{
            IndependentMessagesList.Add(new Message(MessagesPosition, MessageText, DefaultFont, MessageTexturBg));
        }

        public void ChangeMessageLimit(int MessagesLimit) {
            this.MessagesLimit = MessagesLimit;
        }

        Rectangle Buf;

        public void Update() {
            tl = TouchPanel.GetState();

            List<Message> M;

            foreach (TouchLocation T in tl) {

                M = MessagesList.Where(x => x.MessagesPosition.Contains(T.Position)).ToList();

                if (M.Count > 0 && !Buf.Contains(T.Position)) {
                    Buf.X = (int)T.Position.X - 5;
                    Buf.Y = (int)T.Position.Y - 5;
                    Buf.Width = (int)T.Position.X + 5;
                    Buf.Height = (int)T.Position.Y + 5;
                    MessagesList.Remove(M [0]);
                    break;
                }
            }
            RepositionMessages();


            foreach (TouchLocation T in tl) {

                M = IndependentMessagesList.Where(x => x.MessagesPosition.Contains(T.Position)).ToList();

                if (M.Count > 0 && !Buf.Contains(T.Position)) {
                    Buf.X = (int)T.Position.X - 5;
                    Buf.Y = (int)T.Position.Y - 5;
                    Buf.Width = (int)T.Position.X + 5;
                    Buf.Height = (int)T.Position.Y + 5;
                    IndependentMessagesList.Remove(M [0]);
                    break;
                }
            }
        }

        public void DrawMessages(SpriteBatch spriteBatch) {

            foreach (Message M in MessagesList) {
                M.Draw(spriteBatch);
            }
            foreach (Message M in IndependentMessagesList) {
                M.Draw(spriteBatch);
            }
        }

        private void RepositionMessages() {
            for (int i = 0; i < MessagesList.Count; i++) {

                Buffor.X = MessagesList [i].MessagesPosition.X;

                Buffor.Y = (i > 0 ? MessagesList [i - 1].MessagesPosition.Height + 10 + MessagesList [i - 1].MessagesPosition.Y : MessagesPosition.Y);

                Buffor.Width = MessagesList [i].MessagesPosition.Width;
                Buffor.Height = MessagesList [i].MessagesPosition.Height;

                MessagesList [i].MessagesPosition = Buffor;
            }
        }

    }
}