using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Message {
    // Klasa wyświetlająca komuniakty
    public class Messages {
        private List<Message> MessagesList; // Lista komunikatów
        private List<Message> IndependentMessagesList; // Lista komunikatów które nie są wyrównywane według poprzednich

        public Rectangle MessagesPosition; // Pozycja wiadomości weług której mają się wyrównywać kolejne
        TouchCollection tl; // Buffor akcji wykonywanych na ekranie
        public SpriteFont DefaultFont; // Czcionka dla wiadomości
        public Texture2D MessageTexturBg; // Tło wiadomości
        int MessagesLimit; // Maksymalna ilość wiadomości 

        Rectangle Buffor; // Bufor dla kolejnych pozycji

        // Konstruktor z limitem wiadomości 
        public Messages(ContentManager Content, Rectangle MessagesPosition, int MessagesLimit) {
            Load(Content, MessagesPosition);
            ChangeMessageLimit(MessagesLimit);
        }

        // Konstruktor bez limitu wiadomości 
        public Messages(ContentManager Content, Rectangle MessagesPosition) {
            Load(Content, MessagesPosition);
            ChangeMessageLimit(0);
        }

        // Inicjalizacja 
        private void Load(ContentManager Content, Rectangle MessagesPosition) {
            Buffor = new Rectangle();
            this.MessagesPosition = MessagesPosition;

            MessageTexturBg = Content.Load<Texture2D>("UI\\MessageWnd");
            DefaultFont = Content.Load<SpriteFont>("SpriteFontPL");
            MessagesList = new List<Message>();
            IndependentMessagesList = new List<Message>();
        }

        // Dodawanie nowej wiadomości z tekstury
        public void AddMessage(Texture2D MessageTexture, Rectangle MessagesPosition) {
            MessagesList.Reverse();
            if (MessagesList.Count >= MessagesLimit && MessagesLimit > 0)
                MessagesList.RemoveAt(0);

            Buffor.X = MessagesPosition.X;
            Buffor.Y = (MessagesList.Count > 0 ? MessagesList [MessagesList.Count - 1].MessagesPosition.Height + 10 + MessagesList [MessagesList.Count - 1].MessagesPosition.Y : MessagesPosition.Y);

            Buffor.Width = MessagesPosition.Width;
            Buffor.Height = MessagesPosition.Height;
            MessagesList.Add(new Message(Buffor, MessageTexture));
            MessagesList.Reverse();
        }

        // Dodawanie nowej wiadomości z właznym tekstem
        public void AddMessage(string MessageText, Rectangle MessagesPosition) {
            Buffor.X = MessagesPosition.X;
            Buffor.Y = (MessagesList.Count > 0 ?
                MessagesList [MessagesList.Count - 1].MessagesPosition.Height + 10 + MessagesList [MessagesList.Count - 1].MessagesPosition.Y : MessagesPosition.Y);

            Buffor.Width = MessagesPosition.Width;
            Buffor.Height = MessagesPosition.Height;
            MessagesList.Reverse();
            if (MessagesList.Count >= MessagesLimit && MessagesLimit > 0)
                MessagesList.RemoveAt(0);
            MessagesList.Add(new Message(Buffor, MessageText, DefaultFont, MessageTexturBg));
            MessagesList.Reverse();
        }

        // Tworzenie nowej wiadomości na zadanej pozycji
        public void CreateIndependentMessage(string MessageText, Rectangle MessagesPosition) {
            IndependentMessagesList.Add(new Message(MessagesPosition, MessageText, DefaultFont, MessageTexturBg));
        }

        // Zmiana maksymalnej ilości wiadomości
        public void ChangeMessageLimit(int MessagesLimit) {
            this.MessagesLimit = MessagesLimit;
        }
        
        // Ukrywanie wiadomości po naciśnięciu na nią
        public void Update() {
            tl = TouchPanel.GetState();

            List<Message> M;

            foreach (TouchLocation T in tl) {
                M = MessagesList.Where(x => x.MessagesPosition.Contains(T.Position)).ToList();

                if (M.Count > 0 && !Buffor.Contains(T.Position)) {
                    Buffor.X = (int)T.Position.X - 5;
                    Buffor.Y = (int)T.Position.Y - 5;
                    Buffor.Width = (int)T.Position.X + 5;
                    Buffor.Height = (int)T.Position.Y + 5;
                    MessagesList.Remove(M [0]);
                    break;
                }
            }
            RepositionMessages();
            
            foreach (TouchLocation T in tl) {
                M = IndependentMessagesList.Where(x => x.MessagesPosition.Contains(T.Position)).ToList();

                if (M.Count > 0 && !Buffor.Contains(T.Position)) {
                    Buffor.X = (int)T.Position.X - 5;
                    Buffor.Y = (int)T.Position.Y - 5;
                    Buffor.Width = (int)T.Position.X + 5;
                    Buffor.Height = (int)T.Position.Y + 5;
                    IndependentMessagesList.Remove(M [0]);
                    break;
                }
            }
        }

        // Wyświetlanie wiadomości
        public void DrawMessages(SpriteBatch spriteBatch) {
            foreach (Message M in MessagesList) {
                M.Draw(spriteBatch);
            }
            foreach (Message M in IndependentMessagesList) {
                M.Draw(spriteBatch);
            }
        }

        // Zmiana pozycji wiadomości które mają być wyrównywane 
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