using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MapControler;
using Microsoft.Xna.Framework.Audio;
using MonoGame;

using Tools;
using Raw_Materials_C;
using EpicQuests;


namespace PlayerControler
{
    

    class Player
    {
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


        //Surowce posiadane
        private RawMaterials materials;

        public RawMaterials Materials
        {
            get { return materials; }
            set { materials = value; }
        }
        //posiadane toolsy
        private List<Tool> tools;

        public List<Tool> Tools
        {
            get { return tools; }
            set { tools = value; }
        }
        //lista wszystkich questów, które planujemy dla playera aktualnie
        private List<Quest> quests;


        public List<Quest> Quests
        {
            get { return quests; }
            set { quests = value; }
        }

        public int ActiveGuest { get; set; }

        //S³u¿y do wyœwietlania aktualnych postêpów fabularnych gracza, na razie bêdzie pod przyciskiem B.
        public Dairy Player_Dairy { get; set; }


        /// <summary>
        /// Zmienne Odpowiedzialne Za Poprawne Wyœwietlanie Postaci
        /// </summary>
        public Texture2D Texture { get; set; }

        public float PosX { get; set; }
        public float PosY { get; set; }

        public int Rows { get; set; }
        public int Columns { get; set; }

        private int currentFrame;
        private int totalFrames;

        bool IsMoving = false;

        /// <summary>
        /// Prêdkoœæ Wyœwietlania Animacji
        /// </summary>
        private int timeSinceLastFrame = 0;
        public int milliseconsuPerFrame = 140;
          



        SoundEffect Grass;

        /// <summary>
        /// Konstruktor Parametryczny Postaci
        /// </summary>
        public Player(SoundEffect Grass, Texture2D texture, int rows, int columns, int posX, int posY)
        {
            this.Grass = Grass;
           
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

            materials = new RawMaterials();
            tools = new List<Tool>();
            quests = new List<Quest>();

            Player_Dairy = new Dairy();
            //dodawanie Toolsów do listy, dodaæ tutaj tekstury w miejsce "texture" w konstruktorze!
            //M³otek  (1 drewna, 3 liany, 1 kamieñ) 
            tools.Add(new Tool(texture, "Hammer", 1, 1, 3, 0, 0, 0));
            //Topór Siekiera (1 m³otek,3 drewna, 3 liany, 3 kamieñ) -> Jeœli jest w eq to daje wiêcej drewna po œciêciu drzewa
            tools.Add(new Tool(texture, "Axe", 3, 3, 3, 0, 0, 0, new Tool(texture, "Hammer", 1, 1, 3, 0, 0, 0)));
            //3.Kilof (1 m³otek, 5 drewna, 5 liany, 5 kamieñ) ->pozwala wydobywaæ metal 
            tools.Add(new Tool(texture, "Pick", 5, 5, 5, 0, 0, 0, new Tool(texture, "Hammer", 1, 1, 3, 0, 0, 0)));
            //Saw 4.Pi³a (1 m³otek, 3 drewna, 5 metal, 5 liany)
            tools.Add(new Tool(texture, "Saw", 3, 0, 5, 5, 0, 0, new Tool(texture, "Hammer", 1, 1, 3, 0, 0, 0)));

            //test z posiadan¹ siekier¹
            //tools.Find(x => x.ToolName == "Axe").IsOwned = true;

            //Dodajemy questy dla playera, tutaj dawajcie opisy tychze questow
            //misja startowa, zaczyna siê wraz z pojawieniem siê na wyspie
            //x26 y34
            ActiveGuest = 0;
            //Pocz¹tek, zcrafæ m³otek
            string plot1 = "Nie wiem co sie stalo... \r\n Musze zbudowac jakies schronienie przed noca\r\nMusze zaczac od budowy mlotka\r\nPotrzeba: 1 wood\r\n1 stone\r\n3 lianas";
            quests.Add(new Quest("1.Stworz mlotek",new Vector2(26,34),new RawMaterials(), plot1,new Tool(texture, "Hammer", 1, 1, 3, 0, 0, 0)));
            //quests[0].Activated = true;
            //Druga misja-scrafciæ Axe
            string plot2 = "\r\n\r\n Udalo mi sie stworzyc mlotek" +
                "\r\nMusze szybciej zdobywac drewno\r\n"
                + "trzeba zrobic siekiere"
            +"\r\n Potrzeba: 3 wood \r\n 3 stone \r\n 3 lianas\r\n";
            quests.Add(new Quest("2.Craft Axe", new Vector2(26, 34), new RawMaterials(), plot2, new Tool(texture, "Axe", 3, 3, 3, 0, 0, 0, new Tool(texture, "Hammer", 1, 1, 3, 0, 0, 0))));
            //Trzecia misja - stworzenie szeltera przed noc¹
            string plot3 = "\r\nZbliza sie noc, musze zbudowac schronienie!"+
                "Potrzebne : 20 wood \r\n 20 stone \r\n 20 lianas \r\n";
            quests.Add(new Quest("3.Build shelter", new Vector2(26, 34), new RawMaterials(20,20,20,0,0,0), plot3));




        }


        /////////////////////////// METODY ///////////////////////////////////////

        public void Spadek_HP(GameTime gameTime)
        {
            HP_Czas += gameTime.ElapsedGameTime.Milliseconds;

            if (HP_Czas > HP_Predkosc)
            {
                HP_Czas -= HP_Predkosc;
                if (HP != 0)
                { HP -= 1; }
                HP_Czas = 0;
            }
        }

        public void Spadek_Wody(GameTime gameTime)
        {
            Woda_Czas += gameTime.ElapsedGameTime.Milliseconds;

            if (Woda_Czas > Woda_Predkosc)
            {
                Woda_Czas -= Woda_Predkosc;
                if (WODA != 0)
                { WODA -= 1; }
                Woda_Czas = 0;
            }
        }

        public void Spadek_Glod(GameTime gameTime)
        {
            Glod_Czas += gameTime.ElapsedGameTime.Milliseconds;

            if (Glod_Czas > Glod_Predkosc)
            {
                Glod_Czas -= Glod_Predkosc;
                if (GLOD != 0)
                { GLOD -= 1; }
                Glod_Czas = 0;
            }
        }

        public void Spadek_Strach(GameTime gameTime)
        {
            Strach_Czas += gameTime.ElapsedGameTime.Milliseconds;

            if (Strach_Czas > Strach_Predkosc)
            {
                Strach_Czas -= Strach_Predkosc;
                if (STRACH != 0)
                { STRACH -= 1; }
                Strach_Czas = 0;
            }
        }



        /// <summary>
        /// Wyœwietlanie Animacji
        /// </summary>
        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > milliseconsuPerFrame)
            {
                timeSinceLastFrame -= milliseconsuPerFrame;
                currentFrame++;
                timeSinceLastFrame = 0;

                if (currentFrame == totalFrames)
                { currentFrame = 0; }

                if ((currentFrame == 1 || currentFrame == 3) && IsMoving)
                {
                    Grass.Play();
                }
            }
        }


        /// <summary>
        /// Rysowanie Postaci
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Rectangle location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);

            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, location.Width, location.Height + (int)(location.Height * 0.4));


            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);

        }

        /// <summary>
        /// Sterowanie
        /// </summary>
        public void Move(Direction direction, List<Texture2D> tx)
        {
            switch (direction)
            {
                case Direction.Idle_Down:
                { IsMoving = false; Texture = tx [4]; break; }
                case Direction.Up:
                { IsMoving = true; Texture = tx [2]; break; }
                case Direction.Down:
                { IsMoving = true; Texture = tx [3]; break; }
                case Direction.Left:
                { IsMoving = true; Texture = tx [1]; break; }
                case Direction.Right:
                { IsMoving = true; Texture = tx [0]; break; }
                case Direction.Idle_Left:
                { IsMoving = false; Texture = tx [5]; break; }
                case Direction.Idle_Right:
                { IsMoving = false; Texture = tx [6]; break; }
                case Direction.Idle_Back:
                { IsMoving = false; Texture = tx [7]; break; }
                case Direction.Pac_Right:
                { IsMoving = false; Texture = tx[8]; break; }
                case Direction.Pac_Left:
                { IsMoving = false; Texture = tx[9]; break; }
                default:
                break;
            }
        }

        // TESTOWE
        public void gin(GameTime gameTime)
        {
            //Spadek_HP(gameTime);
            Spadek_Wody(gameTime);
            Spadek_Glod(gameTime);
            //Spadek_Strach(gameTime);
        }

    }
}