using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoidCraft_Map_Pad_Player_v1 {
    class Player {
        /// <summary>
        /// Statystyki Postaci
        /// </summary>

        // HP
        public int HP { get; set; }
        private int HP_Czas;
        private int HP_Predkosc = 70;

        // Woda
        public int WODA { get; set; }
        private int Woda_Czas;
        private int Woda_Predkosc = 70;

        // Glod
        public int GLOD { get; set; }
        private int Glod_Czas;
        private int Glod_Predkosc = 70;

        // Strach
        public int STRACH { get; set; }
        private int Strach_Czas;
        private int Strach_Predkosc = 70;


        /// <summary>
        /// Zmienne Odpowiedzialne Za Poprawne Wyœwietlanie Postaci
        /// </summary>
        public Texture2D Texture { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }

        public int Rows { get; set; }
        public int Columns { get; set; }

        private int currentFrame;
        private int totalFrames;


        /// <summary>
        /// Prêdkoœæ Wyœwietlania Animacji
        /// </summary>
        private int timeSinceLastFrame = 0;
        private int milliseconsuPerFrame = 160;



        /// <summary>
        /// Konstruktor Parametryczny Postaci
        /// </summary>
        public Player(Texture2D texture, int rows, int columns, int posX, int posY) {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            HP = 100;
            WODA = 100;
            GLOD = 100;
            STRACH = 100;
            this.PosX = posX;
            this.PosY = posY;
        }

        /////////////////////////// METODY ///////////////////////////////////////



        public void Spadek_HP(GameTime gameTime) {
            HP_Czas += gameTime.ElapsedGameTime.Milliseconds;

            if (HP_Czas > HP_Predkosc) {
                HP_Czas -= HP_Predkosc;
                if (HP != 0) { HP -= 1; }
                HP_Czas = 0;
            }
        }

        public void Spadek_Wody(GameTime gameTime) {
            Woda_Czas += gameTime.ElapsedGameTime.Milliseconds;

            if (Woda_Czas > Woda_Predkosc) {
                Woda_Czas -= Woda_Predkosc;
                if (WODA != 0) { WODA -= 1; }
                Woda_Czas = 0;
            }
        }

        public void Spadek_Glod(GameTime gameTime) {
            Glod_Czas += gameTime.ElapsedGameTime.Milliseconds;

            if (Glod_Czas > Glod_Predkosc) {
                Glod_Czas -= Glod_Predkosc;
                if (GLOD != 0) { GLOD -= 1; }
                Glod_Czas = 0;
            }
        }

        public void Spadek_Strach(GameTime gameTime) {
            Strach_Czas += gameTime.ElapsedGameTime.Milliseconds;

            if (Strach_Czas > Strach_Predkosc) {
                Strach_Czas -= Strach_Predkosc;
                if (STRACH != 0) { STRACH -= 1; }
                Strach_Czas = 0;
            }
        }



        /// <summary>
        /// Wyœwietlanie Animacji
        /// </summary>
        public void Update(GameTime gameTime) {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > milliseconsuPerFrame) {
                timeSinceLastFrame -= milliseconsuPerFrame;
                currentFrame++;
                timeSinceLastFrame = 0;

                if (currentFrame == totalFrames) { currentFrame = 0; }
            }
        }


        /// <summary>
        /// Rysowanie Postaci
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Vector2 location) {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);

            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);


            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);

        }

        /// <summary>
        /// Sterowanie
        /// </summary>
        public void Move(int direction, List<Texture2D> tx) {
            switch (direction) {
                case 0: { Texture = tx[4]; break; }
                case 1: { Texture = tx[0]; break; }
                case 2: { Texture = tx[1]; break; }
                case 3: { Texture = tx[2]; break; }
                case 4: { Texture = tx[3]; break; }
                default: break;
            }
        }

        // TESTOWE
        public void gin(GameTime gameTime) {
            //Spadek_HP(gameTime);
            Spadek_Wody(gameTime);
            Spadek_Glod(gameTime);
            //  Spadek_Strach(gameTime);
        }

    }
}