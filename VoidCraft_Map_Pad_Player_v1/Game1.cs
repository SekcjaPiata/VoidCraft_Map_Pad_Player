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
using Sounds;
using Message;
using InGameMenuControler;

/////////////////////////////////////////////////////////////    J: Kurde nie da sie na timerze zrobic zbierania bo Button A sprawdza tylko warunek przy pierwszym nacisnieciu...
//////////////////      Juan                /////////////////
//////////////////      9,06.2017r          /////////////////   
//////////////////      19,56               /////////////////    A: Johnny dodaj tekstury drzewa ,kamienia ,wody itd do 4 warstwy
//////////////////      VERSION 0.060       /////////////////    P: Juan, trzeba zrobiæ projekt mapy albo tekstury do toolsow
/////////////////////////////////////////////////////////////    A: ... coœ tam wa¿nego :/ 

// Zrobiona Klasa InGameMenu, nie wrzuca³em jej do maina jeszcze
// Testowy knefel okomentowany i nie bêdzie reagowa³ na kliki

namespace VoidCraft_Map_Pad_Player_v1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;
        SpriteFont DefaultFont;

        Map map;
        GameControler Pad;
        MainMenu main;
        Messages messages;

        BackgroundSongs GameBgAmbient;
        SoundEffects GrassWalk;

        Direction WalkingDirection;

        public int ScreenX { get; private set; }
        public int ScreenY { get; private set; }

        double WalkSpeed = 0.05;
        int GameHour; // U¿ywaæ metody ChangeGameTime(int H,int M);
        int GameMinute;

        double Starting_posX = 26;
        double Starting_posY = 34;

        bool DebugMode = true;
        public static bool GameRunning = false;
        public static int SongPlayed = 0;

        Texture2D back;

        // Texture2D Knefel_EQ; // Guziczek do ingame menu **
        // Texture2D Inventory; // Ekwipunek Ingamemenu **
        bool IsMenuButtonPressed = false; // pomocniczy bool dow wyœwietlania menu **
        public InGameMenu InGameMenuManager;
        public InGameMenuState InGameMenuStateManager;
        public TouchCollection TouchCollectionManager;


        float timer = 1;


        //John'owicz
        public List<Texture2D> PlayerMoveTexture; // Tworzenie Listy na teksturyPlayera
        private Player Gracz; // Tworzenie istancji
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

        GamePadStatus buff = GamePadStatus.None;

        Rectangle Buff;

        // ----------------------------------------------------------------------------------------------------- Konstruktor
        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            Graphics.IsFullScreen = true;
            Graphics.PreferredBackBufferWidth = 800;
            Graphics.PreferredBackBufferHeight = 480;
            Graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

            WalkingDirection = Direction.Idle_Down;
            PACDirection = DirectionPAC.Pac_Left;
        }

        // ----------------------------------------------------------------------------------------------------- Init
        protected override void Initialize()
        {
            Buff = new Rectangle();

            ScreenX = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            ScreenY = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            main = new MainMenu();
            messages = new Messages(Content, new Rectangle(50, 20, 200, 50), 3);

            DefaultFont = Content.Load<SpriteFont>("SpriteFontPL");

            PlayerMoveTexture = new List<Texture2D>();
            DayCycleTexture = new List<Texture2D>();

            ChangeGameTime(5, 0);

            GameBgAmbient = new BackgroundSongs("Takeover", Content, true, 0.80f, "Ambient");
            GrassWalk = new SoundEffects("GrassStep", Content, "GrassWalk");

            //MAPY->  ProjektTestowy  JohnnoweTekstury  NoweTeksturyV4  MalaMapa  POLIGON  VoidMap
            map = new Map(GraphicsDevice, "Map_Final_V2", ScreenX, ScreenY);
            map.SetPosition(Starting_posX, Starting_posY);

            Pad = new GameControler(GraphicsDevice, ScreenX, ScreenY);

            // Wczytywanie tekstur Cyklu Dnia
            String DayFoldName = "NightFolder\\Night_";
            for (int i = 0; i < 25; i++) { DayCycleTexture.Add(Content.Load<Texture2D>(DayFoldName + (i + 1))); }

            //// Wczytywanie tekstur Animacji i tworzenie instancji Player
            String CharFoldName = "Characters\\NewChar_";
            for (int i = 0; i < 11; i++) { PlayerMoveTexture.Add(Content.Load<Texture2D>(CharFoldName + (i))); }

            // Przekazuje teksture do postaci i ilosc klatek w danej animacji
            Gracz = new Player(GrassWalk.sound, PlayerMoveTexture[4], 1, IloscKlatek, 10, 600); // Gdybyœ przekaza³ ContenMenager Content jako parametr to wczytywanie siê nie zmieni ,a bêdzie w klasie ... :P

            // --------------------------------------------------------------------------------------------------------------------------------------------------------

            //   Knefel_EQ = Content.Load<Texture2D>("Knefel_EQ"); // £adowanie tekstury knefla **
            //   Inventory = Content.Load<Texture2D>("UI\\Equipment"); // £adowanie tekstury menu **
            InGameMenuManager = new InGameMenu(Content);
            InGameMenuStateManager = InGameMenuState._Game;
            


            base.Initialize();
        }

        // ----------------------------------------------------------------------------------------------------- Update
        protected override void Update(GameTime gameTime)
        {

            if (GameRunning == true)
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
                if (PAC == false) { Gracz.milliseconsuPerFrame = 140; } else { Gracz.milliseconsuPerFrame = 60; }


                ///---------- STEROWANIE POSTACIA ----------------

                if (Pad.IsButtonPresed(GamePadStatus.DirNone))
                {
                    PAC = false;
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



                if (Pad.IsButtonPresed(GamePadStatus.Up))                   ///---------- UP ----------------
                {
                    if (map.GetNextID(3, Direction.On) == 0)
                    {
                        WalkingDirection = Direction.Up;
                        buff = GamePadStatus.Up;
                        Gracz.Move(Direction.Up, PlayerMoveTexture);
                        map.MoveMap(0, -WalkSpeed);

                        if (map.GetNextID(3, Direction.On) != 0)
                            map.MoveMap(0, WalkSpeed);
                    }
                }
                else
                if (Pad.IsButtonPresed(GamePadStatus.Down))                 ///---------- DOWN ----------------
                {
                    if (map.GetNextID(3, Direction.On) == 0)
                    {
                        WalkingDirection = Direction.Down;
                        buff = GamePadStatus.Down;
                        Gracz.Move(Direction.Down, PlayerMoveTexture);
                        map.MoveMap(0, WalkSpeed);

                        if (map.GetNextID(3, Direction.On) != 0)
                            map.MoveMap(0, -WalkSpeed);
                    }
                }
                else
                if (Pad.IsButtonPresed(GamePadStatus.Right))                ///---------- RIGHT ----------------
                {

                    if (map.GetNextID(3, Direction.On) == 0)
                    {
                        WalkingDirection = Direction.Right;
                        buff = GamePadStatus.Right;
                        Gracz.Move(Direction.Right, PlayerMoveTexture);
                        map.MoveMap(WalkSpeed, 0);
                        if (map.GetNextID(3, Direction.On) != 0)
                            map.MoveMap(-WalkSpeed, 0);
                        kierun_Right = true;
                        kierun_Left = false;
                    }
                }
                else
                if (Pad.IsButtonPresed(GamePadStatus.Left))                 ///---------- LEFT ----------------
                {

                    if (map.GetNextID(3, Direction.On) == 0)
                    {
                        WalkingDirection = Direction.Left;
                        buff = GamePadStatus.Left;
                        Gracz.Move(Direction.Left, PlayerMoveTexture);
                        map.MoveMap(-WalkSpeed, 0);
                        if (map.GetNextID(3, Direction.On) != 0)
                            map.MoveMap(WalkSpeed, 0);
                        kierun_Left = true;
                        kierun_Right = false;

                    }
                }


                if (Pad.IsButtonClicked(GamePadStatus.A))                   ///---------- BUTTON A ----------------
                {


                    if (kierun_Right == true)                               ///---------- Animacja Zbierania ----------------
                    {
                        PAC = true;
                        if (Gracz.Tools.Find(x => x.ToolName == "Axe").IsOwned == true)
                        {
                            PACDirection = DirectionPAC.Pac_R_Axe;
                            Gracz.PAC_PAC(DirectionPAC.Pac_R_Axe, PlayerMoveTexture);
                        }
                        else
                        {
                            PACDirection = DirectionPAC.Pac_Right;
                            Gracz.PAC_PAC(DirectionPAC.Pac_Right, PlayerMoveTexture);
                        }
                    }

                    if (kierun_Left == true)
                    {
                        PAC = true;
                        PACDirection = DirectionPAC.Pac_Left;
                        Gracz.PAC_PAC(DirectionPAC.Pac_Left, PlayerMoveTexture);
                    }


                    ///-------------------------------------------------------------   WYKRYWANIE KOLIZJI   -------------------------//
                    if (map.GetNextID(3, WalkingDirection) == 6)
                    { // skrzyneczka
                        String message = "Oooo skrzyneczka na pozycji " + map.GetNextCords(WalkingDirection).X + "x" + map.GetNextCords(WalkingDirection).Y;
                        //Gracz.Chests.Find(x, y => x.X == map.GetNextCords(WalkingDirection).X &&
                        //x.X == map.GetNextCords(WalkingDirection).Y);
                        //map.Message(message, DefaultFont, new Rectangle(50, 20, 600, 200));
                        messages.AddMessage(message, new Rectangle(50, 20, 600, 200));
                    }


                    if (map.GetNextID(3, WalkingDirection) == 2)
                    { // Drewno

                        if (Gracz.Tools.Find(x => x.ToolName == "Axe").IsOwned == true)
                        {
                            //linq
                            Gracz.Materials.Wood += 2;
                            Gracz.Materials.Lianas += 2;
                            String message = "Zebrano drewno siekiera\r\n ilosc drewna: " + Gracz.Materials.Wood + "\r\n Ilosc lian:" + Gracz.Materials.Lianas;
                            //map.Message(message, DefaultFont, new Rectangle(50, 20, 400, 200));
                            messages.AddMessage(message, new Rectangle(50, 20, 600, 200));
                        }
                        else
                        {
                            Gracz.Materials.Wood++;
                            Gracz.Materials.Lianas++;
                            String message = "Zebrano drewno i liany\r\n ilosc drewna: " + Gracz.Materials.Wood + "\r\n Ilosc lian: " + Gracz.Materials.Lianas;
                            //map.Message(message, DefaultFont, new Rectangle(50, 20, 400, 200));
                            messages.AddMessage(message, new Rectangle(50, 20, 600, 200));
                        }

                        // Zmiana ID kafelek mapy
                        Vector2 TreeBase = map.ChangeID(WalkingDirection, 1, 31); // Zmiana podstawy drzewa na pieniek
                        map.ChangeID(WalkingDirection, 3, 1); // Zmiana ID podstawy na BLOKADA
                        map.ChangeID((int)TreeBase.X, (int)TreeBase.Y - 1, 2, 0); // Usuniêcie korony drzewa (1 wy¿ej ni¿ podstawa)

                    }
                    else if (map.GetNextID(3, WalkingDirection) == 3)
                    { // Jerzynki
                        Gracz.Materials.Food++;
                        String message = "Zebrano jedzenie, ilosc jedzenia: " + Gracz.Materials.Food;
                        //map.Message(message, DefaultFont, new Rectangle(50, 20, 400, 100));
                        messages.AddMessage(message, new Rectangle(50, 20, 600, 200));
                        Vector2 TreeBase = map.ChangeID(WalkingDirection, 1, 2); // Zmiana krzaka je¿ynkowego na zwyk³y
                        map.ChangeID(WalkingDirection, 3, 1); // Zmiana ID ...

                    }
                    else if (map.GetNextID(3, WalkingDirection) == 4)
                    { // Kamien
                        Gracz.Materials.Stone++;
                        String message = "Zebrano kamien, ilosc kamienia: " + Gracz.Materials.Stone;
                        //map.Message(message, DefaultFont, new Rectangle(50, 20, 400, 100));
                        messages.AddMessage(message, new Rectangle(50, 20, 600, 200));
                    }
                    else if (map.GetNextID(3, WalkingDirection) == 5)
                    { // Woda
                        Gracz.Materials.Water++;
                        String message = "Zebrano wode ilosc wody: " + Gracz.Materials.Water;
                        //map.Message(message, DefaultFont, new Rectangle(50, 20, 400, 100));
                        messages.AddMessage(message, new Rectangle(50, 20, 600, 200));
                    }
                    else
                    {

                    }
                }

                //-------------------------------------------- BUTTON B ---------------------------------------//

                if (Pad.IsButtonClicked(GamePadStatus.B))
                {


                    //ChangeGameTime(GameHour + 1, 0);


                    string dairy = "";
                    foreach (string m in Gracz.Player_Dairy.dairy_notes)
                    {
                        dairy += m;
                    }
                    //map.Message(dairy, DefaultFont, new Rectangle(50, 20, 1000, 1000));
                    messages.CreateIndependentMessage(dairy, new Rectangle(50, 20, 1000, 1000));
                }

                //if (map.GetCurrentID(4) != 0)
                //{
                //    map.Message("Oooo misja :/  ID:" + map.GetMissionID(4), DefaultFont, new Rectangle(50, 20, 400, 100));
                //}

                if (Gracz.ActiveGuest >= Gracz.Quests.Count)
                {
                    //map.Message("\r\n Brawo! Zebrales potrzebne materialy\r\n aby stworzyc schronienie i przetrwac\r\n nadchodzaca NOC \r\n \r\n Ukonczono fabule prologu!", DefaultFont, new Rectangle(100, 100, 600, 600));
                    // .. map.
                    map.MessageActive = 2;
                    string message = "\r\n Brawo! Zebrales potrzebne materialy\r\n aby stworzyc schronienie i przetrwac\r\n nadchodzaca NOC \r\n \r\n Ukonczono fabule prologu!";
                    messages.CreateIndependentMessage(message, new Rectangle(50, 20, 1000, 1000));
                }
                else if (!Gracz.Quests[Gracz.ActiveGuest].Activated)
                {
                    //map.Message(Gracz.Quests [Gracz.ActiveGuest].Name, DefaultFont, new Rectangle(50, 20, 400, 100));
                    messages.CreateIndependentMessage(Gracz.Quests[Gracz.ActiveGuest].Name, new Rectangle(50, 20, 1000, 1000));

                    Gracz.Quests[Gracz.ActiveGuest].Activated = true;
                    Gracz.Player_Dairy.dairy_notes.Add(Gracz.Quests[Gracz.ActiveGuest].Quest_message);

                }
                else if (Gracz.Quests[Gracz.ActiveGuest].IsFinished(Gracz.Materials, Gracz.Tools))
                {
                    //map.Message("Ukonczono misje:\r\n" + Gracz.Quests [Gracz.ActiveGuest].Name, DefaultFont, new Rectangle(100, 100, 600, 600));
                    messages.CreateIndependentMessage("Ukonczono misje:\r\n" + Gracz.Quests[Gracz.ActiveGuest].Name, new Rectangle(50, 20, 1000, 1000));
                    switch (Gracz.ActiveGuest)
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
                    Gracz.Player_Dairy.dairy_notes.Add("\r\nUkonczono misje : \r\n" + Gracz.Quests[Gracz.ActiveGuest].Name);
                    Gracz.ActiveGuest++;
                }
                messages.Update();
            }
            else
            {
                main.Update();
            }

            if (SongPlayed == 0)
            {
                GameBgAmbient.ChangeSong("TakeOver");
                SongPlayed = 100;
            }
            else if (SongPlayed == 1)
            { //  true false
                GameBgAmbient.ChangeSong("BgMusic");
                SongPlayed = 100;
            }

            base.Update(gameTime);
        }

        // ----------------------------------------------------------------------------------------------------- Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            spriteBatch.Begin();
            if (GameRunning == true)
            {
                // Rysowanie Pierwszych 2 warstw.
                map.Draw(spriteBatch, 0);
                map.Draw(spriteBatch, 1);


                // Rysowanie Gracza
                Gracz.Draw(spriteBatch, new Rectangle(
                    ((ScreenX / 2) - (map.GetZoomValue() / 2)),
                    ((ScreenY / 2)) - map.GetZoomValue() + 40,
                    map.GetZoomValue() - 20, map.GetZoomValue() - 20)
                    );

                // Rysowanie 3 Warstwy.
                map.Draw(spriteBatch, 2);

                messages.DrawMessages(spriteBatch);

                //  0 - 5 NOC ||  5 - 7 ZMIANA ||  7 - 17 DZIEÑ ||  17 - 19 ZMIANA ||  19 - 23 NOC
                // Dzien i noc
                if (GameHour >= 5 && GameHour <= 7) // ZMIANA W !!!!! DZIEN
                {
                    while (DayCycleTimer % 300 == 0)
                    {
                        if (DayCycle == 0)
                            break;
                        DayCycleTimer++;
                        DayCycle--;
                    }
                }
                else if (GameHour >= 17 && GameHour <= 19) // ZMIANA W !!!!! NOC
                {
                    while (DayCycleTimer % 300 == 0)
                    {
                        if (DayCycle == 24)
                            break;
                        DayCycleTimer++;
                        DayCycle++;
                    }
                }


                spriteBatch.Draw(DayCycleTexture[DayCycle], new Rectangle(0, 0, ScreenX, ScreenY), Color.White);      // Rysowanie Tekstury Nocy


                // \/\/\/\/\/\/\/\/\/\/\/\/\/\/


                InGameMenuManager.DrawInGameMenuButton(spriteBatch);



                //   Rectangle KnefelRect = new Rectangle((int)(ScreenX / 1.125), 50, 50, 50);   // Pozycja knefla do ingame menu **
                //    spriteBatch.Draw(Knefel_EQ, new Rectangle((int)(ScreenX / 1.125), 50, 50, 50), Color.White); // Rysowanie knefla do ingame menu **

                TouchCollection tl = TouchPanel.GetState();



                foreach (TouchLocation T in tl)
                {


                    if (InGameMenuManager.InGameMenuButtonPos.Contains(T.Position) && !Buff.Contains(T.Position))
                    {
                        IsMenuButtonPressed = !IsMenuButtonPressed;
                        InGameMenuStateManager = InGameMenuState._Game;
                        DebugMode = !DebugMode;

                        Buff.X = (int)T.Position.X - 2;
                        Buff.Y = (int)T.Position.Y - 2;
                        Buff.Width = 4;
                        Buff.Height = 4;

                        break;
                    }

                }

                if (IsMenuButtonPressed)
                {
                    if (InGameMenuStateManager == InGameMenuState._Game) InGameMenuStateManager = InGameMenuState._Settings;
                    //   spriteBatch.Draw(Inventory, new Rectangle(100, 100, 800, 400), Color.White); // Rysowanie knefla do ingame menu **

                    InGameMenuManager.DrawInGameMenu(InGameMenuStateManager, spriteBatch);

                    TouchCollectionManager = TouchPanel.GetState();
                    foreach (TouchLocation TC in TouchCollectionManager)
                    {
                        if (InGameMenuManager.SettingsButtonsPos[2].Contains(TC.Position))
                        {
                            InGameMenuStateManager = InGameMenuState._Inventory;
                        }
                    }



                    //spriteBatch.DrawString(DefaultFont, "Woda: " + Gracz.Materials.Water, new Vector2(120, 150), Color.White);
                    //spriteBatch.DrawString(DefaultFont, "Jedzenie: " + Gracz.Materials.Food, new Vector2(120, 200), Color.White);
                    //spriteBatch.DrawString(DefaultFont, "Drewno: " + Gracz.Materials.Wood, new Vector2(120, 250), Color.White);
                    //spriteBatch.DrawString(DefaultFont, "Liany: " + Gracz.Materials.Lianas, new Vector2(120, 300), Color.White);
                    //spriteBatch.DrawString(DefaultFont, "Kamien: " + Gracz.Materials.Stone, new Vector2(120, 350), Color.White);
                    InGameMenuManager.DrawInGameMenuButton(spriteBatch);
                }



                // /\/\/\/\/\/\/\/\/\/\/\/\/\




                // Rysowanie Przyciskow i informacji pomocniczych.

                Pad.Draw(spriteBatch);
                if (DebugMode)
                {
                    spriteBatch.DrawString(DefaultFont, "X: " + map.GetPosition().X, new Vector2(50, 50), Color.Red);
                    spriteBatch.DrawString(DefaultFont, "Y: " + map.GetPosition().Y, new Vector2(50, 100), Color.Red);
                    spriteBatch.DrawString(DefaultFont, "Dir: " + WalkingDirection.ToString(), new Vector2(50, 150), Color.Red);
                    spriteBatch.DrawString(DefaultFont, "Square size: " + map.GetZoomValue(), new Vector2(50, 200), Color.Red);
                    spriteBatch.DrawString(DefaultFont, "Game time: " + GameHour + ":" + GameMinute, new Vector2(50, 250), Color.Red);
                    spriteBatch.DrawString(DefaultFont, "Resolution: " + ScreenX + "x" + ScreenY, new Vector2(50, 300), Color.Red);
                    spriteBatch.DrawString(DefaultFont, "Square size: " + map.GetZoomValue(), new Vector2(50, 350), Color.Red);
                    spriteBatch.DrawString(DefaultFont, "Player Pos: " + (((ScreenX / 2) - (map.GetZoomValue() / 2))) + "x" + (((ScreenY / 2)) - map.GetZoomValue()), new Vector2(50, 400), Color.Red);
                    spriteBatch.DrawString(DefaultFont, "Player Pos: " + (((ScreenX / 2) - (map.GetZoomValue() / 2))) / 18 + "x" + (((ScreenY / 2)) - map.GetZoomValue()) / 11, new Vector2(50, 450), Color.Red);
                    spriteBatch.DrawString(DefaultFont, "DayCycleTimer: " + DayCycleTimer + " ID:" + DayCycle, new Vector2(50, 500), Color.Red); // DayCycle TEST PacTimer
                    spriteBatch.DrawString(DefaultFont, "PacTimer: " + PacTimer, new Vector2(50, 550), Color.Red);
                    spriteBatch.DrawString(DefaultFont, "Zbierz: " + Zbierz, new Vector2(50, 600), Color.Red);

                    //   spriteBatch.DrawString(DefaultFont, "Woda: " + Gracz.Materials.Water, new Vector2(1600, 50), Color.White);
                    //   spriteBatch.DrawString(DefaultFont, "Jedzenie: " + Gracz.Materials.Food, new Vector2(1600, 100), Color.White);
                    //   spriteBatch.DrawString(DefaultFont, "Drewno: " + Gracz.Materials.Wood, new Vector2(1600, 150), Color.White);
                    //   spriteBatch.DrawString(DefaultFont, "Liany: " + Gracz.Materials.Lianas, new Vector2(1600, 200), Color.White);
                    //   spriteBatch.DrawString(DefaultFont, "Kamien: " + Gracz.Materials.Stone, new Vector2(1600, 250), Color.White);

                    //spriteBatch.DrawString(DefaultFont, "Zdrowie: " + Gracz.HP, new Vector2(400, 50), Color.LightGreen);
                    //spriteBatch.DrawString(DefaultFont, "Woda: " + Gracz.WODA, new Vector2(400, 100), Color.LightGreen);
                    //spriteBatch.DrawString(DefaultFont, "Jedzenie: " + Gracz.GLOD, new Vector2(400, 150), Color.LightGreen);
                    //spriteBatch.DrawString(DefaultFont, "Strach: " + Gracz.STRACH, new Vector2(400, 200), Color.LightGreen);

                    /*
                    if (Gracz.HP != 0)
                    { spriteBatch.DrawString(DefaultFont, "HP: " + Gracz.HP, new Vector2(0, 20), Color.Black); } //Wyswietlanie Poziomu HP na Ekranie
                    else 
                    { spriteBatch.DrawString(DefaultFont, "HP: " + Gracz.HP + " YOU DIED!", new Vector2(100, 100), Color.Black); }
                    */
                }


                // Timer Cyklu Dnia
                DayCycleTimer++;
                if (DayCycleTimer >= 100000) { DayCycleTimer = 0; }



            }
            else
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


        // ---------------------------------------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------------------- Jakieœ coœ ....
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            back = Content.Load<Texture2D>("Menu\\M_BACK");
            main.LoadContent(Content);
        }

        void ChangeGameTime(int H, int M)
        {
            GameHour = H;
            GameMinute = M;
            DayCycleTimer = 0;
            if (GameHour >= 5 && GameHour <= 7)
                DayCycle = 24 - ((120 - (((7 - GameHour + 1)) * 60) + ((60 - GameMinute))) / 5);
            else if (GameHour >= 17 && GameHour <= 19)
                DayCycle = ((120 - (((19 - GameHour + 1)) * 60) + ((60 - GameMinute))) / 5);
        }

    }
}

