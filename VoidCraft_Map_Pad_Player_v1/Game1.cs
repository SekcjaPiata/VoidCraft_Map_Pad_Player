using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MapControler;
using PadControler;
using System;
using System.Collections.Generic;
using PlayerControler;
using Microsoft.Xna.Framework.Media;


/////////////////////////////////////////////////////////////
//////////////////                          /////////////////
//////////////////     VERSION 0.010         /////////////////  a ja zapomnia³em :)
//////////////////                          /////////////////   HyHy
/////////////////////////////////////////////////////////////


namespace VoidCraft_Map_Pad_Player_v1 {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont sf;
        Map map;
        GameControler Pad;
        Direction WalkingDirection;
        double Speed = 1;
        Song song;

        int GameHour, GameMinute;
        float timer = 1;
        Texture2D Night;

        //John'owicz
        public List<Texture2D> PlayerMoveTexture; // Tworzenie Listy na teksturyPlayera
        private Player Gracz; // Tworzenie istancji
        private SpriteFont font; // Napis
        private int IloscKlatek = 4; // ilosc klatek w danej animacji  


        public int ScreenX { get; private set; }
        public int ScreenY { get; private set; }

        GamePadStatus buff = GamePadStatus.None;
        /// -----------------------------------------------------------------------------------------------------

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            WalkingDirection = Direction.Idle_Down;
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }
        /// -----------------------------------------------------------------------------------------------------

        protected override void Initialize() {
            ScreenX = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            ScreenY = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            sf = Content.Load<SpriteFont>("SpriteFontPL");

            Night = Content.Load<Texture2D>("Night");

            PlayerMoveTexture = new List<Texture2D>();

            song = Content.Load<Song>("ZombiesAreComing");

            //map = new Map(GraphicsDevice, "ProjektTestowy", ScreenX, ScreenY);
            //map = new Map(GraphicsDevice, "JohnnoweTekstury", ScreenX, ScreenY);
            //map = new Map(GraphicsDevice, "NoweTeksturyV4", ScreenX, ScreenY);
            //map = new Map(GraphicsDevice, "MalaMapa", ScreenX, ScreenY);
            map = new Map(GraphicsDevice, "POLIGON", ScreenX, ScreenY);

            //map = new Map(GraphicsDevice, "VoidMap", ScreenX, ScreenY); // 6.04.2017r
            map.SetPosition(1, 1);

            GameHour = 0;
            GameMinute = 0;

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
            MediaPlayer.Play(song);

            base.Initialize();
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer < 0) {
                GameTimeControl();
                timer = 1;   //Reset Timer
            }


            //Gracz.gin(gameTime);
            Gracz.Update(gameTime);

            if (Pad.IsButtonPresed(GamePadStatus.DirNone)) {
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
            if (Pad.IsButtonPresed(GamePadStatus.Up)) {
                if (map.GetObjectType(3, Direction.Up) == 0) {
                    WalkingDirection = Direction.Up;
                    buff = GamePadStatus.Up;
                    Gracz.Move(Direction.Up, PlayerMoveTexture);
                    map.MoveMap(0, -Speed);
                }
            } else
            if (Pad.IsButtonPresed(GamePadStatus.Down)) {
                if (map.GetObjectType(3, Direction.Down) == 0) {
                    WalkingDirection = Direction.Down;
                    buff = GamePadStatus.Down;
                    Gracz.Move(Direction.Down, PlayerMoveTexture);
                    map.MoveMap(0, Speed);
                }
            } else
            if (Pad.IsButtonPresed(GamePadStatus.Right)) {

                if (map.GetObjectType(3, Direction.Right) == 0) {
                    WalkingDirection = Direction.Right;
                    buff = GamePadStatus.Right;
                    Gracz.Move(Direction.Right, PlayerMoveTexture);
                    map.MoveMap(Speed, 0);
                }
            } else
            if (Pad.IsButtonPresed(GamePadStatus.Left)) {

                if (map.GetObjectType(3, Direction.Left) == 0) {
                    WalkingDirection = Direction.Left;
                    buff = GamePadStatus.Left;
                    Gracz.Move(Direction.Left, PlayerMoveTexture);
                    map.MoveMap(-Speed, 0);
                }
            }

            if (Pad.IsButtonClicked(GamePadStatus.A)) {
                //map.Message("I pach pach poraz " + (++LicznikPachPach), Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));

                if (map.GetObjectType(3, WalkingDirection) == 2) { // Drewno
                    map.Message("I pach pach w drewno", Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));
                }
                else if(map.GetObjectType(3, WalkingDirection) == 3) { // Jerzynki
                    map.Message("I pach pach w jerzynki", Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));

                } else if (map.GetObjectType(3, WalkingDirection) == 4) { // Kamien
                    map.Message("I pach pach w kamien", Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));

                }
                else if(map.GetObjectType(3, WalkingDirection) == 5) { // Woda
                    map.Message("I pach pach w wode", Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));

                } 

            } else if (Pad.IsButtonClicked(GamePadStatus.B)) {
                //map.Message("I pach pach poraz " + (--LicznikPachPach), Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));
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


            // Rysowanie Gracza
            Gracz.Draw(spriteBatch, new Rectangle(
                ((ScreenX / 2) - (map.GetZoomValue() / 2)),
                ((ScreenY / 2)) - map.GetZoomValue(),
                map.GetZoomValue(), map.GetZoomValue())
                );

            // Rysowanie 3 Warstwy.
            map.Draw(spriteBatch, 2, true);

            // Dzien i noc
            if (GameHour >= 8 && GameHour <= 17) { // Dzien

            } else if (GameHour >= 6 && GameHour <= 7) {// Noc -> Dzien

            } else if (GameHour >= 18 && GameHour <= 19) {// Dzien -> Noc

            } else if (GameHour >= 20 || GameHour <= 5) { // Noc
                spriteBatch.Draw(Night, new Rectangle(0, 0, ScreenX, ScreenY), Color.White);
            }


            // Rysowanie Przyiskow
            Pad.Draw(spriteBatch);

            spriteBatch.DrawString(sf, "X: " + map.GetPosition().X, new Vector2(50, 50), Color.Red);
            spriteBatch.DrawString(sf, "Y: " + map.GetPosition().Y, new Vector2(50, 100), Color.Red);
            spriteBatch.DrawString(sf, "Dir: " + WalkingDirection.ToString(), new Vector2(50, 150), Color.Red);
            spriteBatch.DrawString(sf, "Square size: " + map.GetZoomValue(), new Vector2(50, 200), Color.Red);
            spriteBatch.DrawString(sf, "Game time: " + GameHour + ":" + GameMinute, new Vector2(50, 250), Color.Red);

            //Wyswietlanie Poziomu HP na Ekranie
            if (Gracz.HP != 0) { spriteBatch.DrawString(font, "HP: " + Gracz.HP, new Vector2(0, 20), Color.Black); } else { spriteBatch.DrawString(font, "HP: " + Gracz.HP + " YOU DIED!", new Vector2(100, 100), Color.Black); }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void GameTimeControl() {
            GameMinute++;
            if (GameMinute >= 60) {
                GameHour++;
                if (GameHour >= 24) {
                    GameHour = 0;
                }
                GameMinute = 0;
            }
        }

        /// -----------------------------------------------------------------------------------------------------
        protected override void LoadContent() { spriteBatch = new SpriteBatch(GraphicsDevice); }
        protected override void UnloadContent() { }
    }
}
