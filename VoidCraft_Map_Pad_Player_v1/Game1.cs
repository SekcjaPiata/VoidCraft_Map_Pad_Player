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
using Tools;
using Menu;
using System.Text;
using EpicQuests;


/////////////////////////////////////////////////////////////    J: Kurde nie da sie na timerze zrobic zbierania bo Button A sprawdza tylko warunek przy pierwszym nacisnieciu...
//////////////////      2.06.2017r          /////////////////   
//////////////////      17:29               /////////////////    A: Johnny dodaj tekstury drzewa ,kamienia ,wody itd do 4 warstwy
//////////////////     VERSION 0.047        /////////////////    P: Juan, trzeba zrobiæ projekt mapy albo tekstury do toolsow
/////////////////////////////////////////////////////////////    A: ... coœ tam wa¿nego :/   



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
        MainMenu main = new MainMenu();
        Texture2D back;

        int GameHour, GameMinute;
        float timer = 1;

        bool DebugMode = true;

        //John'owicz
        public List<Texture2D> PlayerMoveTexture; // Tworzenie Listy na teksturyPlayera
        private Player Gracz; // Tworzenie istancji
        private SpriteFont font; // Napis
        private int IloscKlatek = 4; // ilosc klatek w danej animacji
        bool PAC = false;
        bool kierun_Left = false;
        bool kierun_Right = false;
        bool Zbierz = false;
        DirectionPAC PACDirection;
        double PacTimer = 0;

        double DayCycleTimer = 0; // Timer dla systemu dnia i nocy
        public List<Texture2D> DayCycleTexture;  // Lista na Textury Nocy
        int DayCycle = 0;



        public int ScreenX { get; private set; }
        public int ScreenY { get; private set; }
        public int LicznikPachPach { get; private set; }
        GamePadStatus buff = GamePadStatus.None;

        //static bool running = false;
        public static bool Running { get; set; }



        /// -----------------------------------------------------------------------------------------------------

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            WalkingDirection = Direction.Idle_Down;
            PACDirection = DirectionPAC.Pac_Left;
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
            double Starting_posX = 26;
            double Starting_posY = 34;
            map.SetPosition(Starting_posX, Starting_posY);

            GameHour = 17;
            GameMinute = 55;

            Pad = new GameControler(GraphicsDevice, ScreenX, ScreenY);

            font = Content.Load<SpriteFont>("File"); // Use the name of your sprite font file

            // Wczytywanie tekstur Cyklu Dnia
            String DayFoldName = "NightFolder\\Night_";
            for (int i = 0; i < 25; i++)  {  DayCycleTexture.Add(Content.Load<Texture2D>(DayFoldName + (i+1)));   }

            //// Wczytywanie tekstur Animacji i tworzenie instancji Player
            String CharFoldName = "Characters\\NewChar_";
            for (int i = 0; i < 10; i++) {  PlayerMoveTexture.Add(Content.Load<Texture2D>(CharFoldName + (i))); }
            
            // Przekazuje teksture do postaci i ilosc klatek w danej animacji
            Gracz = new Player(GrassWalk, PlayerMoveTexture [4], 1, IloscKlatek, 10, 600);

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
                Gracz.PosY = map.GetPosition().Y;
                Gracz.PosX = map.GetPosition().X;

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    Exit();

                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timer < 0)
                {
                    GameTimeControl();
                    timer = 1;   //Reset Timer
                }

                Gracz.gin(gameTime);
                Gracz.Update(gameTime);


                /// Zmiana predkosci animacji przy Pacnieciu
                if(PAC == false)
                { Gracz.milliseconsuPerFrame = 140;} 
                else{ Gracz.milliseconsuPerFrame = 60;}

               
                ///---------- STEROWANIE POSTACIA ----------------

                if (Pad.IsButtonPresed(GamePadStatus.DirNone))
                {
                    PAC = false;
                    if (buff == GamePadStatus.Up)
                    {
                        Gracz.Move(Direction.Idle_Back, PlayerMoveTexture);
                    } else if (buff == GamePadStatus.Down)
                    {
                        Gracz.Move(Direction.Idle_Down, PlayerMoveTexture);
                    } else if (buff == GamePadStatus.Right)
                    {
                        Gracz.Move(Direction.Idle_Right, PlayerMoveTexture);
                    } else if (buff == GamePadStatus.Left)
                    {
                        Gracz.Move(Direction.Idle_Left, PlayerMoveTexture);
                    }
                }

                

                if (Pad.IsButtonPresed(GamePadStatus.Up))                   ///---------- UP ----------------
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
                } else
                if (Pad.IsButtonPresed(GamePadStatus.Down))                 ///---------- DOWN ----------------
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
                } else
                if (Pad.IsButtonPresed(GamePadStatus.Right))                ///---------- RIGHT ----------------
                {

                    if (map.GetObjectType(3, Direction.On) == 0)
                    {
                        WalkingDirection = Direction.Right;
                        buff = GamePadStatus.Right;
                        Gracz.Move(Direction.Right, PlayerMoveTexture);
                        map.MoveMap(Speed, 0);
                        if (map.GetObjectType(3, Direction.On) != 0)
                            map.MoveMap(-Speed, 0);
                        kierun_Right = true;
                        kierun_Left = false;
                    }
                } else
                if (Pad.IsButtonPresed(GamePadStatus.Left))                 ///---------- LEFT ----------------
                {

                    if (map.GetObjectType(3, Direction.On) == 0)
                    {
                        WalkingDirection = Direction.Left;
                        buff = GamePadStatus.Left;
                        Gracz.Move(Direction.Left, PlayerMoveTexture);
                        map.MoveMap(-Speed, 0);
                        if (map.GetObjectType(3, Direction.On) != 0)
                            map.MoveMap(Speed, 0);
                        kierun_Left = true;
                        kierun_Right = false;

                    }
                }


                if (Pad.IsButtonClicked(GamePadStatus.A))                   ///---------- BUTTON A ----------------
                {
                   

                    if (kierun_Right == true)                               ///---------- Animacja Zbierania ----------------
                    {
                        PAC = true;
                        PACDirection = DirectionPAC.Pac_Right;
                        Gracz.PAC_PAC(DirectionPAC.Pac_Right, PlayerMoveTexture);
                    }

                    if(kierun_Left == true)
                    {
                        PAC = true;
                        PACDirection = DirectionPAC.Pac_Left;
                        Gracz.PAC_PAC(DirectionPAC.Pac_Left, PlayerMoveTexture);
                    }

                    
                 ///-------------------------------------------------------------   WYKRYWANIE KOLIZJI   -------------------------//

                    if (map.GetObjectType(3, WalkingDirection) == 2)
                    { // Drewno

                        if (Gracz.Tools.Find(x => x.ToolName == "Axe").IsOwned == true)
                        {
                            //linq
                            Gracz.Materials.Wood += 2;
                            Gracz.Materials.Lianas += 2;
                            String message = "Zebrano drewno siekiera\r\n ilosc drewna: " + Gracz.Materials.Wood + "\r\n Ilosc lian:" + Gracz.Materials.Lianas;
                            map.Message(message, Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 200));

                        }
                        else
                        {
                            Gracz.Materials.Wood++;
                            Gracz.Materials.Lianas++;
                            String message = "Zebrano drewno i liany\r\n ilosc drewna: " + Gracz.Materials.Wood + "\r\n Ilosc lian: " + Gracz.Materials.Lianas;
                            map.Message(message, Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 200));

                        }

                        // Zmiana ID kafelek mapy
                        Vector2 TreeBase = map.ChangeID(WalkingDirection, 1, 30); // Zmiana podstawy drzewa na pieniek
                        map.ChangeID(WalkingDirection, 3, 1); // Zmiana ID podstawy na BLOKADA
                        map.ChangeID((int)TreeBase.X, (int)TreeBase.Y - 1, 2, 0); // Usuniêcie korony drzewa (1 wy¿ej ni¿ podstawa)

                    }
                    else if (map.GetObjectType(3, WalkingDirection) == 3)
                    { // Jerzynki
                        Gracz.Materials.Food++;
                        String message = "Zebrano jedzenie, ilosc jedzenia: " + Gracz.Materials.Food;
                        map.Message(message, Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));

                        Vector2 TreeBase = map.ChangeID(WalkingDirection, 1, 2); // Zmiana krzaka je¿ynkowego na zwyk³y
                        map.ChangeID(WalkingDirection, 3, 1); // Zmiana ID ...

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
                        //  map.Message("I pach pach poraz " + (++LicznikPachPach), Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));
                    }
                }
                
                //-------------------------------------------- BUTTON B ---------------------------------------//

                if (Pad.IsButtonClicked(GamePadStatus.B))
                {
                    // map.Message("I pach pach poraz " + (--LicznikPachPach), Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));
                    string dairy="";
                    foreach (string m in Gracz.Player_Dairy.dairy_notes)
                    {
                        dairy += m;
                    }
                    map.Message(dairy, Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 1000, 1000));
                }

                //if (map.GetCurrentID(4) != 0)
                //{
                //    map.Message("Oooo misja :/  ID:" + map.GetMissionID(4), Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));
                //}

                if (Gracz.ActiveGuest >= Gracz.Quests.Count)
                {
                    map.Message("\r\n Brawo! Zebrales potrzebne materialy\r\n aby stworzyc schronienie i przetrwac\r\n nadchodzaca NOC \r\n \r\n Ukonczono fabule prologu!", Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(100, 100, 600, 600));
                    // .. map.
                    map.MessageActive = 2;
                    
                }
               else if (!Gracz.Quests[Gracz.ActiveGuest].Activated)
                {
                    map.Message( Gracz.Quests[Gracz.ActiveGuest].Name, Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(50, 20, 400, 100));
                    Gracz.Quests[Gracz.ActiveGuest].Activated = true;
                    Gracz.Player_Dairy.dairy_notes.Add(Gracz.Quests[Gracz.ActiveGuest].Quest_message);

                }
                else if (Gracz.Quests[Gracz.ActiveGuest].IsFinished(Gracz.Materials))
                {
                    map.Message("Ukonczono misje:\r\n"+Gracz.Quests[Gracz.ActiveGuest].Name, Content.Load<SpriteFont>("SpriteFontPL"), new Rectangle(100, 100, 600, 600));
                    switch(Gracz.ActiveGuest)
                    {
                        case 0:
                            Gracz.Tools.Find(x => x.ToolName == "Hammer").IsOwned = true;
                            Gracz.Tools.Find(x => x.ToolName == "Hammer").Craft(Gracz.Materials);
                            break;
                        case 1:
                            Gracz.Tools.Find(x => x.ToolName == "Axe").IsOwned = true;
                            Gracz.Tools.Find(x => x.ToolName == "Axe").Craft(Gracz.Materials);
                            break;
                    }
                    Gracz.Player_Dairy.dairy_notes.Add("\r\nUkonczono misje : \r\n"+Gracz.Quests[Gracz.ActiveGuest].Name);
                    Gracz.ActiveGuest++;
                }

                map.Update();
            } else
                main.Update();
            base.Update(gameTime);
        }





        /// ------------------------------   FUNKCJE RYSUJACE  ------------------------------------





        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            spriteBatch.Begin();
            if (Running == true)
            {
                // Rysowanie Pierwszych 2 warstw.
                map.Draw(spriteBatch, 0, false);
                map.Draw(spriteBatch, 1, false);


                // Rysowanie Gracza
                Gracz.Draw(spriteBatch, new Rectangle(
                    ((ScreenX / 2) - (map.GetZoomValue() / 2)),
                    ((ScreenY / 2)) - map.GetZoomValue()+40,
                    map.GetZoomValue()-20, map.GetZoomValue()-20)
                    );

                // Rysowanie 3 Warstwy.
                map.Draw(spriteBatch, 2, true);


                // Dzien i noc
                if (GameHour >= 6 && GameHour <= 17) // DZIEN
                {
                    while (DayCycleTimer % 300 == 0)
                    {
                        if (DayCycle == 0)
                            break;
                        DayCycleTimer++;
                        DayCycle--;
                    }
                } else if (GameHour >= 18 || GameHour <= 5) // NOC
                {
                    while (DayCycleTimer % 300 == 0)
                    {
                        if (DayCycle == 24)
                            break;
                        DayCycleTimer++;
                        DayCycle++;
                    }
                }

                
                spriteBatch.Draw(DayCycleTexture [DayCycle], new Rectangle(0, 0, ScreenX, ScreenY), Color.White);      // Rysowanie Tekstury Nocy



                // Rysowanie Przyciskow i informacji pomocniczych.

                Pad.Draw(spriteBatch);
                if (DebugMode)
                {
                    spriteBatch.DrawString(sf, "X: " + map.GetPosition().X, new Vector2(50, 50), Color.Red);
                    spriteBatch.DrawString(sf, "Y: " + map.GetPosition().Y, new Vector2(50, 100), Color.Red);
                    spriteBatch.DrawString(sf, "Dir: " + WalkingDirection.ToString(), new Vector2(50, 150), Color.Red);
                    spriteBatch.DrawString(sf, "Square size: " + map.GetZoomValue(), new Vector2(50, 200), Color.Red);
                    spriteBatch.DrawString(sf, "Game time: " + GameHour + ":" + GameMinute, new Vector2(50, 250), Color.Red);
                    spriteBatch.DrawString(sf, "Resolution: " + ScreenX + "x" + ScreenY, new Vector2(50, 300), Color.Red);
                    spriteBatch.DrawString(sf, "Square size: " + map.GetZoomValue(), new Vector2(50, 350), Color.Red);
                    spriteBatch.DrawString(sf, "Player Pos: " + (((ScreenX / 2) - (map.GetZoomValue() / 2))) + "x" + (((ScreenY / 2)) - map.GetZoomValue()), new Vector2(50, 400), Color.Red);
                    spriteBatch.DrawString(sf, "Player Pos: " + (((ScreenX / 2) - (map.GetZoomValue() / 2))) / 18 + "x" + (((ScreenY / 2)) - map.GetZoomValue()) / 11, new Vector2(50, 450), Color.Red);
                    spriteBatch.DrawString(sf, "DayCycleTimer: " + DayCycleTimer, new Vector2(50, 500), Color.Red); // DayCycle TEST PacTimer
                    spriteBatch.DrawString(sf, "PacTimer: " + PacTimer, new Vector2(50, 550), Color.White);
                    spriteBatch.DrawString(sf, "Zbierz: " + Zbierz, new Vector2(50, 600), Color.White);

                    spriteBatch.DrawString(sf, "Woda: " + Gracz.Materials.Water, new Vector2(1600, 50), Color.White);
                    spriteBatch.DrawString(sf, "Jedzenie: " + Gracz.Materials.Food, new Vector2(1600, 100), Color.White);
                    spriteBatch.DrawString(sf, "Drewno: " + Gracz.Materials.Wood, new Vector2(1600, 150), Color.White);
                    spriteBatch.DrawString(sf, "Liany: " + Gracz.Materials.Lianas, new Vector2(1600, 200), Color.White);
                    spriteBatch.DrawString(sf, "Kamien: " + Gracz.Materials.Stone, new Vector2(1600, 250), Color.White);

                    spriteBatch.DrawString(sf, "Zdrowie: " + Gracz.HP, new Vector2(400, 50), Color.LightGreen);
                    spriteBatch.DrawString(sf, "Woda: " + Gracz.WODA, new Vector2(400, 100), Color.LightGreen);
                    spriteBatch.DrawString(sf, "Jedzenie: " + Gracz.GLOD, new Vector2(400, 150), Color.LightGreen);
                    spriteBatch.DrawString(sf, "Strach: " + Gracz.STRACH, new Vector2(400, 200), Color.LightGreen);

                    /*
                    if (Gracz.HP != 0)
                    { spriteBatch.DrawString(font, "HP: " + Gracz.HP, new Vector2(0, 20), Color.Black); } //Wyswietlanie Poziomu HP na Ekranie
                    else 
                    { spriteBatch.DrawString(font, "HP: " + Gracz.HP + " YOU DIED!", new Vector2(100, 100), Color.Black); }
                    */
                }


                // Timer Cyklu Dnia
                DayCycleTimer++;
                if (DayCycleTimer >= 100000)
                { DayCycleTimer = 0; }



            } else
                main.Draw(spriteBatch);
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
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            back = Content.Load<Texture2D>("Menu\\M_BACK");
            main.LoadContent(Content);
        }
        protected override void UnloadContent() { }
    }
}

