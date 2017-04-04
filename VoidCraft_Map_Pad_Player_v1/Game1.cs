using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MapControler;
using PadControler;
using System;
using System.Collections.Generic;
using PlayerControler;


            /////////////////////////////////////////////////////////////
            //////////////////                          /////////////////
            //////////////////     VERSION 0.05         /////////////////
            //////////////////                          /////////////////
            /////////////////////////////////////////////////////////////


namespace VoidCraft_Map_Pad_Player_v1 {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont sf;
        Map map;
        GameControler Pad;
        Direction WalkingDirection;
        double Speed = 0.05;
        int LicznikPachPach = 0;

        //John
        public List<Texture2D> PlayerMoveTexture; // Tworzenie Listy na teksturyPlayera
        private Player Gracz; // Tworzenie istancji
        private SpriteFont font; // Napis
        private int IloscKlatek = 4; // ilosc klatek w danej animacji  


        public int ScreenX { get; private set; }
        public int ScreenY { get; private set; }
        bool A = false;
        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            WalkingDirection = Direction.Idle_Down;
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

        }

        protected override void Initialize() {
            ScreenX = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            ScreenY = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            sf = Content.Load<SpriteFont>("SpriteFontPL");

            PlayerMoveTexture = new List<Texture2D>();
            //map = new Map(GraphicsDevice, "ProjektTestowy", ScreenX, ScreenY);
            // map = new Map(GraphicsDevice, "JohnnoweTekstury", ScreenX, ScreenY);
            //map = new Map(GraphicsDevice, "NoweTeksturyV4", ScreenX, ScreenY);
            map = new Map(GraphicsDevice, "MalaMapa", ScreenX, ScreenY);
            map.SetPosition(22, 20);
            Pad = new GameControler(GraphicsDevice, ScreenX, ScreenY);

            font = Content.Load<SpriteFont>("File"); // Use the name of your sprite font file

            // Wczytywanie tekstur Animacji i tworzenie instancji Player
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Right_140"));        //0
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Left_140"));         //1
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Back_140"));         //2
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Front_140"));        //3 
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Idle_140"));         //4
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Left_Idle_140"));    //5
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Right_Idle_140"));   //6
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Back_Idle"));        //7

            // Przekazuje teksture do postaci i ilosc klatek w danej animacji
            Gracz = new Player(PlayerMoveTexture[4], 1, IloscKlatek, 10, 600); 


            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent() { }

        GamePadStatus buff = GamePadStatus.None;

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            //Gracz.gin(gameTime);
            Gracz.Update(gameTime);

            List<GamePadStatus> ButtonPressed = Pad.GamePadState();


            if (ButtonPressed.Contains(GamePadStatus.DirNone)) {


                if (buff == GamePadStatus.Up) {
                        Gracz.Move(Direction.Idle_Back, PlayerMoveTexture);
                } else if (buff == GamePadStatus.Down) {
                        Gracz.Move(Direction.Idle_Down, PlayerMoveTexture);

                } else if (buff == GamePadStatus.Right) {
                        Gracz.Move(Direction.Idle_Right, PlayerMoveTexture);

                } else if (buff == GamePadStatus.Left) {
                        Gracz.Move(Direction.Idle_Left, PlayerMoveTexture);

                }
            } else
            if (ButtonPressed.Contains(GamePadStatus.Up)) {
                if (map.GetObjectType(3, Direction.Up) != 1) {
                    WalkingDirection = Direction.Up;
                    buff = GamePadStatus.Up;
                    Gracz.Move(Direction.Up, PlayerMoveTexture);
                    map.MoveMap(0, -Speed);
                }
            } else
            if (ButtonPressed.Contains(GamePadStatus.Down)) {
                if (map.GetObjectType(3, Direction.Down) != 1) {
                    WalkingDirection = Direction.Down;
                    buff = GamePadStatus.Down;
                    Gracz.Move(Direction.Down, PlayerMoveTexture);
                    map.MoveMap(0, Speed);
                }
            } else
            if (ButtonPressed.Contains(GamePadStatus.Right)) {

                if (map.GetObjectType(3, Direction.Right) != 1) {
                    WalkingDirection = Direction.Right;
                    buff = GamePadStatus.Right;
                    Gracz.Move(Direction.Right, PlayerMoveTexture);
                    map.MoveMap(Speed, 0);
                }
            } else
            if (ButtonPressed.Contains(GamePadStatus.Left)) {

                if (map.GetObjectType(3, Direction.Left) != 1) {
                    WalkingDirection = Direction.Left;
                    buff = GamePadStatus.Left;
                    Gracz.Move(Direction.Left, PlayerMoveTexture);
                    map.MoveMap(-Speed, 0);
                }
            }
            {

                //if (map.MapOfsetX.ToString().Contains(".")) {
                //    // 3
                //    // 3.54
                //    if (buff == GamePadStatus.Right) {
                //        WalkingDirection = Direction.Right;
                //        buff = GamePadStatus.Right;
                //        Gracz.Move(Direction.Right, PlayerMoveTexture);
                //        map.MoveMap(Speed, 0);
                //    } else
                //    if (buff == GamePadStatus.Left) {
                //        WalkingDirection = Direction.Left;
                //        buff = GamePadStatus.Left;
                //        Gracz.Move(Direction.Left, PlayerMoveTexture);
                //        map.MoveMap(-Speed, 0);
                //    }
                //}
                //if (map.MapOfsetY.ToString().Contains(".")) {

                //    if (buff == GamePadStatus.Up) {
                //        WalkingDirection = Direction.Up;
                //        buff = GamePadStatus.Up;
                //        Gracz.Move(Direction.Up, PlayerMoveTexture);
                //        map.MoveMap(0, -Speed);
                //    } else
                //if (buff == GamePadStatus.Down) {
                //        WalkingDirection = Direction.Down;
                //        buff = GamePadStatus.Down;
                //        Gracz.Move(Direction.Down, PlayerMoveTexture);
                //        map.MoveMap(0, Speed);
                //    }
                //}


            }



            if (ButtonPressed.Contains(GamePadStatus.A)) {
                if (map.GetObjectType(3, WalkingDirection) == 2) { // Tak se misja // 2 -> Id skrzynek
                    map.Message("I pach pach w krzynke\n\n             OK", Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 700, 400));

                }
            } else
            if (ButtonPressed.Contains(GamePadStatus.B)) {
                ///
            }

            if (map.GetPosition().X == 25 && map.GetPosition().Y == 20) { // Tak se misja 
                map.Message("Bla bla bla\n\n                  OK", Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 700, 200));
            }


            if (Pad.GamePadState().Contains(GamePadStatus.A)) {
                map.Message("I pach pach poraz " + LicznikPachPach, Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));
                if (!A) {
                    LicznikPachPach++;
                }
                A = true;

            } else {
                A = false;
            }

            if (map.GetObjectType(3, WalkingDirection) == 2) { // Tak se misja // 2 -> Id skrzynek
                map.Message("Ooo skrzyneczka  WALNIJ JA (A) :D   \nna pozycji: \nX= " + map.GetPosition().X + "  \nY= " + map.GetPosition().Y + "\n   WALNIJ JA", Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 700, 400));
            }

            map.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Green);

            spriteBatch.Begin();

            // Rysowanie Pierwszych 2 warstw.
            map.Draw(spriteBatch, 0, false);
            map.Draw(spriteBatch, 1, false);
            
            spriteBatch.DrawString(sf, "X: " + map.GetPosition().X, new Vector2(50, 50), Color.Firebrick);
            spriteBatch.DrawString(sf, "Y: " + map.GetPosition().Y, new Vector2(50, 100), Color.Firebrick);
            spriteBatch.DrawString(sf, "Dir: " + WalkingDirection.ToString(), new Vector2(50, 150), Color.Firebrick);
            spriteBatch.DrawString(sf, "...: " + "", new Vector2(50, 200), Color.Red);

            //Wyswietlanie Poziomu HP na Ekranie
            if (Gracz.HP != 0) { spriteBatch.DrawString(font, "HP: " + Gracz.HP, new Vector2(100, 100), Color.Black); }
            else { spriteBatch.DrawString(font, "HP: " + Gracz.HP + " YOU DIED!", new Vector2(100, 100), Color.Black); }

            // Rysowanie Gracza
            Gracz.Draw(spriteBatch, new Rectangle(
                ((ScreenX / 2) - (map.GetZoomValue() / 2)), ((ScreenY / 2)) - 42, map.GetZoomValue(), map.GetZoomValue())
                );

            // Rysowanie 3 Warstwy.
            map.Draw(spriteBatch, 2, true);
            
            // Rysowanie Przyiskow
            Pad.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
