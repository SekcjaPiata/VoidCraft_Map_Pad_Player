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
using Microsoft.Xna.Framework.Audio;
using VoidCraft_Map_Pad_Player_v1.Tools;
using System.Text;

///////////////////////////////////////////////////////////// A: Johnny dodaj tekstury drzewa ,kamienia ,wody itd do 4 warstwy
//////////////////                          ///////////////// P: Juan, trzeba zrobiæ projekt mapy albo tekstury do toolsow
//////////////////     VERSION 0.023        /////////////////   
//////////////////                          /////////////////   
/////////////////////////////////////////////////////////////   


namespace VoidCraft_Map_Pad_Player_v1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont sf;
        Map map;
        GameControler Pad;
        Direction WalkingDirection;
        double Speed = 0.05;
        Song song;
        SoundEffect GrassWalk;
        static bool running = false;
        MainMenu main = new MainMenu();
        Texture2D back;

        int GameHour, GameMinute;
        float timer = 1;
        //Texture2D Night;

        //John'owicz
        public List<Texture2D> PlayerMoveTexture; // Tworzenie Listy na teksturyPlayera
        private Player Gracz; // Tworzenie istancji
        private SpriteFont font; // Napis
        private int IloscKlatek = 4; // ilosc klatek w danej animacji

        double DayCycleTimer = 0; // Timer dla systemu dnia i nocy
        public List<Texture2D> DayCycleTexture;  // Lista na Textury Nocy
        int DayCycle = 0;



        public int ScreenX { get; private set; }
        public int ScreenY { get; private set; }
        public int LicznikPachPach { get; private set; }
        public static bool Running { get => running; set => running = value; }
      

        GamePadStatus buff = GamePadStatus.None;

        /// -----------------------------------------------------------------------------------------------------

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            WalkingDirection = Direction.Idle_Down;
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// -----------------------------------------------------------------------------------------------------

        protected override void Initialize()
        {
         

            ScreenX = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            ScreenY = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            sf = Content.Load<SpriteFont>("SpriteFontPL");

            //Night = Content.Load<Texture2D>("Night");

            PlayerMoveTexture = new List<Texture2D>();
            DayCycleTexture = new List<Texture2D>();

            song = Content.Load<Song>("BgMusic");
            GrassWalk = Content.Load<SoundEffect>("GrassStep");

            //map = new Map(GraphicsDevice, "ProjektTestowy", ScreenX, ScreenY);
            //map = new Map(GraphicsDevice, "JohnnoweTekstury", ScreenX, ScreenY);
            //map = new Map(GraphicsDevice, "NoweTeksturyV4", ScreenX, ScreenY);
            //map = new Map(GraphicsDevice, "MalaMapa", ScreenX, ScreenY);
            //map = new Map(GraphicsDevice, "POLIGON", ScreenX, ScreenY);
            map = new Map(GraphicsDevice, "PiecioWarstwowy", ScreenX, ScreenY);

            //map = new Map(GraphicsDevice, "VoidMap", ScreenX, ScreenY); // 6.04.2017r
            map.SetPosition(26, 34);

            GameHour = 17;
            GameMinute = 55;

            Pad = new GameControler(GraphicsDevice, ScreenX, ScreenY);

            font = Content.Load<SpriteFont>("File"); // Use the name of your sprite font file

            // Wczytywanie tekstur Cyklu Dnia
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_1"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_2"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_3"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_4"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_5"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_6"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_7"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_8")); // TAK WIEM ZE CHUJOWO I IDZIE ZROBIC LEPIEJ
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_9")); // ALE TO TEST ;)
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_10"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_11"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_12"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_13"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_14"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_15"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_16"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_17"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_18"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_19"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_20"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_21"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_22"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_23"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_24"));
            DayCycleTexture.Add(Content.Load<Texture2D>("NightFolder\\Night_25"));


            //// Wczytywanie tekstur Animacji i tworzenie instancji Player
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Characters\\NewChar_Right"));        //0
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Characters\\NewChar_Left"));         //1
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Characters\\NewChar_Back"));         //2
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Characters\\NewChar_Front"));        //3 
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Characters\\NewChar_Idle_Front"));   //4
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Characters\\NewChar_Idle_Left"));    //5
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Characters\\NewChar_Idle_Right"));   //6
            PlayerMoveTexture.Add(Content.Load<Texture2D>("Characters\\NewChar_Idle_Back"));    //7

            // Przekazuje teksture do postaci i ilosc klatek w danej animacji
            Gracz = new Player(GrassWalk, PlayerMoveTexture[4], 1, IloscKlatek, 10, 600);

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.70f;
            MediaPlayer.Play(song);


            base.Initialize();
        }

        /// -----------------------------------------------------------------------------------------------------

        protected override void Update(GameTime gameTime)
        {


            if (Running == true)
            {


                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    Exit();



                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timer < 0)
                {
                    GameTimeControl();
                    timer = 1;   //Reset Timer
                }


                //Gracz.gin(gameTime);
                Gracz.Update(gameTime);

                if (Pad.IsButtonPresed(GamePadStatus.DirNone))
                {
                    if (buff == GamePadStatus.Up)
                    {
                        Gracz.Move(Direction.Idle_Back, PlayerMoveTexture);
                    }
                    else if (buff == GamePadStatus.Down)
                    {
                        Gracz.Move(Direction.Idle_Down, PlayerMoveTexture);
                    }
                    else if (buff == GamePadStatus.Right)
                    {
                        Gracz.Move(Direction.Idle_Right, PlayerMoveTexture);
                    }
                    else if (buff == GamePadStatus.Left)
                    {
                        Gracz.Move(Direction.Idle_Left, PlayerMoveTexture);
                    }
                }


                if (Pad.IsButtonPresed(GamePadStatus.Up))
                {
                    if (map.GetObjectType(3, Direction.On) == 0)
                    {
                        WalkingDirection = Direction.Up;
                        buff = GamePadStatus.Up;
                        Gracz.Move(Direction.Up, PlayerMoveTexture);
                        map.MoveMap(0, -Speed);

                        if (map.GetObjectType(3, Direction.On) != 0)
                            map.MoveMap(0, Speed);
                    }
                }
                else
                if (Pad.IsButtonPresed(GamePadStatus.Down))
                {
                    if (map.GetObjectType(3, Direction.On) == 0)
                    {
                        WalkingDirection = Direction.Down;
                        buff = GamePadStatus.Down;
                        Gracz.Move(Direction.Down, PlayerMoveTexture);
                        map.MoveMap(0, Speed);

                        if (map.GetObjectType(3, Direction.On) != 0)
                            map.MoveMap(0, -Speed);
                    }
                }
                else
                if (Pad.IsButtonPresed(GamePadStatus.Right))
                {

                    if (map.GetObjectType(3, Direction.On) == 0)
                    {
                        WalkingDirection = Direction.Right;
                        buff = GamePadStatus.Right;
                        Gracz.Move(Direction.Right, PlayerMoveTexture);
                        map.MoveMap(Speed, 0);
                        if (map.GetObjectType(3, Direction.On) != 0)
                            map.MoveMap(-Speed, 0);
                    }
                }
                else
                if (Pad.IsButtonPresed(GamePadStatus.Left))
                {

                    if (map.GetObjectType(3, Direction.On) == 0)
                    {
                        WalkingDirection = Direction.Left;
                        buff = GamePadStatus.Left;
                        Gracz.Move(Direction.Left, PlayerMoveTexture);
                        map.MoveMap(-Speed, 0);
                        if (map.GetObjectType(3, Direction.On) != 0)
                            map.MoveMap(Speed, 0);
                    }
                }

                if (Pad.IsButtonClicked(GamePadStatus.A))
                {
                    if (map.GetObjectType(3, WalkingDirection) == 2)
                    { // Drewno
                        if (Gracz.Tools.Find(x => x.ToolName == "Axe").IsOwned == true)
                        {
                            //linq w chuj
                            Gracz.Materials.Wood += 2;
                            String message = "Zebrano drewno siekiera, ilosc drewna: " + Gracz.Materials.Wood;
                            map.Message(message, Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));

                        }
                        else
                        {
                            Gracz.Materials.Wood++;
                            String message = "Zebrano drewno, ilosc drewna: " + Gracz.Materials.Wood;
                            map.Message(message, Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));
                        }
                    }
                    else if (map.GetObjectType(3, WalkingDirection) == 3)
                    { // Jerzynki
                        Gracz.Materials.Food++;
                        String message = "Zebrano jedzenie, ilosc jedzenia: " + Gracz.Materials.Food;
                        map.Message(message, Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));

                    }
                    else if (map.GetObjectType(3, WalkingDirection) == 4)
                    { // Kamien
                        Gracz.Materials.Stone++;
                        String message = "Zebrano kamien, ilosc kamienia: " + Gracz.Materials.Stone;
                        map.Message(message, Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));

                    }
                    else if (map.GetObjectType(3, WalkingDirection) == 5)
                    { // Woda
                        Gracz.Materials.Water++;
                        String message = "Zebrano wode ilosc wody: " + Gracz.Materials.Water;
                        map.Message(message, Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));
                    }
                    else
                    {
                        map.Message("I pach pach poraz " + (++LicznikPachPach), Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));
                    }

                }
                else if (Pad.IsButtonClicked(GamePadStatus.B))
                {
                    map.Message("I pach pach poraz " + (--LicznikPachPach), Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));
                }

            if (map.GetMissionID(4) != 0) {
                map.Message("Oooo misja :/  ID:" + map.GetMissionID(4), Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));
            }



                map.Update();
            }

            else main.Update();
            base.Update(gameTime);
        }

        /// -----------------------------------------------------------------------------------------------------

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            spriteBatch.Begin();
            if (running == true)
            {
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
                if (GameHour >= 6 && GameHour <= 17) // DZIEN
                {
                    while (DayCycleTimer % 300 == 0)
                    {
                        if (DayCycle == 0) break;
                        DayCycleTimer++;
                        DayCycle--;
                    }
                }
                else if (GameHour >= 18 || GameHour <= 5) // NOC
                {
                    while (DayCycleTimer % 300 == 0)
                    {
                        if (DayCycle == 24) break;
                        DayCycleTimer++;
                        DayCycle++;
                    }
                }

                // Rysowanie Tekstury Nocy
                spriteBatch.Draw(DayCycleTexture[DayCycle], new Rectangle(0, 0, ScreenX, ScreenY), Color.White);


                // Rysowanie Przyciskow
                Pad.Draw(spriteBatch);

                spriteBatch.DrawString(sf, "X: " + map.GetPosition().X, new Vector2(50, 50), Color.Red);
                spriteBatch.DrawString(sf, "Y: " + map.GetPosition().Y, new Vector2(50, 100), Color.Red);
                spriteBatch.DrawString(sf, "Dir: " + WalkingDirection.ToString(), new Vector2(50, 150), Color.Red);
                spriteBatch.DrawString(sf, "Square size: " + map.GetZoomValue(), new Vector2(50, 200), Color.Red);
                spriteBatch.DrawString(sf, "Game time: " + GameHour + ":" + GameMinute, new Vector2(50, 250), Color.Red);
                spriteBatch.DrawString(sf, "Resolution: " + ScreenX + "x" + ScreenY, new Vector2(50, 300), Color.Red);
                spriteBatch.DrawString(sf, "Square size: " + map.GetZoomValue(), new Vector2(50, 350), Color.Red);
                spriteBatch.DrawString(sf, "Player Pos: " + (((ScreenX / 2) - (map.GetZoomValue() / 2))) + "x" + (((ScreenY / 2)) - map.GetZoomValue()), new Vector2(50, 400), Color.Red);
                spriteBatch.DrawString(sf, "Player Pos: " + (((ScreenX / 2) - (map.GetZoomValue() / 2))) / 18 + "x" + (((ScreenY / 2)) - map.GetZoomValue()) / 11, new Vector2(50, 450), Color.Red);
                spriteBatch.DrawString(sf, "DayCycleTimer: " + DayCycleTimer, new Vector2(50, 500), Color.Red); // DayCycle TEST
                DayCycleTimer++;
                if (DayCycleTimer >= 100000) { DayCycleTimer = 0; }


                //Wyswietlanie Poziomu HP na Ekranie
                if (Gracz.HP != 0) { spriteBatch.DrawString(font, "HP: " + Gracz.HP, new Vector2(0, 20), Color.Black); } else { spriteBatch.DrawString(font, "HP: " + Gracz.HP + " YOU DIED!", new Vector2(100, 100), Color.Black); }


            }
            else main.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void GameTimeControl()
        {
            GameMinute++;
            if (GameMinute >= 60)
            {
                GameHour++;
                if (GameHour >= 24)
                {
                    GameHour = 0;
                }
                GameMinute = 0;
            }
        }

        /// -----------------------------------------------------------------------------------------------------
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            back = Content.Load<Texture2D>("Menu\\M_BACK");
            main.LoadContent(Content);

           

        }
        protected override void UnloadContent() { }
    }
}

