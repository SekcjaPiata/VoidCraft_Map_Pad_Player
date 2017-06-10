using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Input.Touch;


using MapControler;
using Microsoft.Xna.Framework.Audio;
using MonoGame;
using System.Web;
using Tools;
using Raw_Materials_C;
using EpicQuests;
using System.Xml.Serialization;
using Message;
using System.IO.IsolatedStorage;


using System.Diagnostics;


namespace MapControler {
   public enum Direction {
        Idle_Down, Left, Right, Up, Down, Idle_Left, Idle_Right, Idle_Back, On
    }

   public enum DirectionPAC {
        Pac_Left, Pac_Right, Pac_R_Axe, Pac_L_Axe, Pac_Up, Pac_Down
    }
    [Serializable]
  public  class Map {
        /// <summary>
        /// Variables
        /// </summary>
        /// [
        /// 
        [NonSerialized]
        GraphicsDevice GraphicDevice;
        [NonSerialized]
        List<List<MapTexture>> Textur; //id ,layer ,name ,path ,bitmap

        public List<Tile [][]> Tiles;


        //internal int MessageActive = 0;

        string MapName;
        int MapSizeX = 18, MapSizeY = 11;
        int NumberOfLayers, Height, Width;

        public double MapOfsetX { get; set; }
        public double MapOfsetY { get; set; }

        private int screenX;
        private int screenY;
        private int InitMapZoom;

        private int MapZoom;

        /// <summary>
        /// Constructors
        /// </summary>
        public Map(GraphicsDevice GraphicDevice, string MapName, int screenX, int screenY) {
            this.GraphicDevice = GraphicDevice;
            this.screenX = screenX;
            this.screenY = screenY;

            //this.MapZoom = (screenX / 17); // Po szerokosci
            //this.MapZoom = (screenY / 10); // Po wyskokosci

            this.MapZoom = ((screenY / 10) + (screenX / 17)) / 2; // Po wyskokosci i szerokosci

            this.InitMapZoom = this.MapZoom;

            this.MapName = MapName;
            this.MapOfsetX = 9;
            this.MapOfsetY = 2;

            GetMapSize();

            Textur = new List<List<MapTexture>>();
            Tiles = new List<Tile[][]>();

            for (int i = 0; i < NumberOfLayers; i++) {
                Textur.Add(new List<MapTexture>());

                Textur [i] = new List<MapTexture>();
                Tile[][] temp = new Tile[Width][];
                for (int j = 0; j < Width; j++)
                {
                    temp[j] = new Tile[Height];
                }
                Tiles.Add(temp);
                
            }

            LoadTextures();
            LoadMap();

        }

        public Map()
        {
            Textur = new List<List<MapTexture>>();
            Tiles = new List<Tile[][]>();
        }

        public void SaveMap()
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            XmlSerializer xmlFormat = null;
            try
            {
                xmlFormat = new XmlSerializer(typeof(Map));
            }
            catch (Exception ex)
            {
                Debug.Write(ex.InnerException.ToString());
            }

            if (store.FileExists("Mapa.xml"))
            {
                store.DeleteFile("Mapa.xml");
            }

            var fs = store.CreateFile("Mapa.xml");
            using (StreamWriter sw = new StreamWriter(fs))
            {
                xmlFormat.Serialize(sw, this);
            }


            if (store.FileExists("Mapa.xml"))
            {
                var fss = store.OpenFile("Mapa.xml", FileMode.Open);
                using (StreamReader sr = new StreamReader(fss))
                {

                    string xmls = sr.ReadToEnd();

                    Debug.Write(xmls);
                    store.Close();
                }
            }
            else
            {
                Debug.Write("Plik nie istnieje");
            }
        }
        public void LoadMapFromXML()
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            XmlSerializer xmlFormat = null;
            try
            {
                xmlFormat = new XmlSerializer(typeof(Map));
            }
            catch (Exception ex)
            {
                Debug.Write(ex.InnerException.ToString());
            }

            if (store.FileExists("Mapa.xml"))
            {
                var fs = store.OpenFile("Mapa.xml", FileMode.Open);

                using (StreamReader sw = new StreamReader(fs))
                {
                    Map temp = (Map)xmlFormat.Deserialize(sw);
                    //return (Map)xmlFormat.Deserialize(sw);
                    this.Tiles = temp.Tiles;


                }
            }
            else throw new Exception("Brak pliku z zapisem mapy");


        }

     
        /// <summary>
        /// Private methods ...
        /// </summary>
        private void GetMapSize() {
            using (var stream = TitleContainer.OpenStream("Content/Maps/" + MapName + "/mapdata.vcmd"))
            using (var reader = new StreamReader(stream)) {
                this.MapName = reader.ReadLine();
                this.Width = int.Parse(reader.ReadLine());
                this.Height = int.Parse(reader.ReadLine());
                this.NumberOfLayers = int.Parse(reader.ReadLine());
            }
        }

        private void LoadTextures() {
            using (var stream = TitleContainer.OpenStream("Content/Maps/" + MapName + "/texturelist.vctl"))
            using (var reader = new StreamReader(stream)) {
                while (!reader.EndOfStream) {
                    string [] line = reader.ReadLine().Split(' ');
                    int l = int.Parse(line [0]);
                    int i = int.Parse(line [1]);
                    string name = line [2];
                    string path = line [3];

                    Textur [l].Add(new MapTexture(i, l, name, path, GetTextureFromFile("Content/Maps/" + MapName + "/" + path)));
                }
            }


        }

        private void LoadMap() {// name = Level_1
            string line;
            string [] lineNumbers;

            for (int i = 0; i < NumberOfLayers; i++) {
                int y = 0;
                using (var stream = TitleContainer.OpenStream("Content/Maps/" + MapName + "/Map/L" + (i) + ".vcmf"))
                using (var reader = new StreamReader(stream)) {
                    while (!reader.EndOfStream) {
                        line = reader.ReadLine();
                        lineNumbers = line.Split(' ');

                        int x = 0;
                        foreach (string L in lineNumbers) {
                            if (x >= Width)
                                break;

                            if (Tiles [i] [x] [y] == null)
                                Tiles [i] [x] [y] = new Tile();
                            Tiles [i] [x] [y].Set(int.Parse(L), i);

                            x++;
                        }
                        y++;
                    }

                }

            }
        }

        private Texture2D GetTextureFromFile(String Path) {
            using (var stream = TitleContainer.OpenStream(Path)) {
                return Texture2D.FromStream(GraphicDevice, stream);
            }
        }


        /// <summary>
        /// Public methods ...
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, int LayerToDraw) {

            for (int y = 0; y < Height; y++) {//18
                for (int x = 0; x < Width; x++) {//11

                    int XX = (int)(((x - (MapOfsetX - 10)) * MapZoom));
                    int YY = (int)(((y - (MapOfsetY - 7)) * MapZoom));

                    if (YY >= 0 && (YY / MapZoom) < MapSizeY + 1) {
                        if (XX >= 0 && (XX / MapZoom) < MapSizeX + 1) {

                            spriteBatch.Draw(Textur [LayerToDraw] [Tiles [LayerToDraw] [x] [y].Id].Bitmap,
                                    new Rectangle(XX - MapZoom, YY - MapZoom, MapZoom, MapZoom),
                                    Color.White);
                        }
                    } else
                        break;
                }
            }
        }




        /// <summary>
        /// Geters
        /// </summary>
        public Vector2 GetPosition() {
            return new Vector2(Convert.ToInt32((float)MapOfsetX), Convert.ToInt32((float)MapOfsetY));
        }

        public int GetNextID(int Layer, Direction Dir) {
            int i = -1;
            if (Dir != Direction.Idle_Down) {
                int x = (Dir == Direction.Left) ? -1 : (Dir == Direction.Right) ? 1 : 0;
                int y = (Dir == Direction.Up) ? -1 : (Dir == Direction.Down) ? 1 : 0;
                // x = 0;
                // y = 0;

                if (GetPosition().X + x >= 0 && GetPosition().Y + y >= 0) {
                    if (GetPosition().X + x < Width && GetPosition().Y + y < Height) {
                        if (((int)GetPosition().X + x) - 1 >= 0 && ((int)GetPosition().Y + y) - 1 >= 0) {
                            i = Textur [Layer] [Tiles [Layer] [((int)GetPosition().X + x) - 1] [((int)GetPosition().Y + y) - 1].Id].ID;
                        }

                        // TOFIX

                    }
                }

            }
            return i;
        }

        public int GetCurrentID(int Layer) {

            return Textur [Layer] [Tiles [Layer] [((int)GetPosition().X) - 1][ ((int)GetPosition().Y) - 1].Id].ID;
        }

        public int GetZoomValue() {
            return MapZoom;
        }

        /// <summary>
        /// Seters
        /// </summary>
        public void MoveMap(double ToAddX, double ToAddY) {

            MapOfsetX += ToAddX;
            MapOfsetY += ToAddY;


            if (MapOfsetX <= 1) { MapOfsetX = 1; }
            if (MapOfsetY <= 1) { MapOfsetY = 1; }

            if (MapOfsetX >= Width) { MapOfsetX = Width; }

            if (MapOfsetY >= Height) { MapOfsetY = Height; }

        }

        public void SetPosition(double PosX, double PosY) {
            MapOfsetX = PosX;
            MapOfsetY = PosY;
        }

        public void ChangeID(int x, int y, int layer, int NewID) {
            Tiles [layer] [x - 1][ y - 1].Id = NewID;
            //Textur [layer] [Tiles [layer] [x-1, y-1].Id].ID = NewID;
        }

        public Vector2 GetNextCords(Direction Dir) {
            int x = (int)GetPosition().X + ((Dir == Direction.Left) ? -1 : (Dir == Direction.Right) ? 1 : 0);
            int y = (int)GetPosition().Y + ((Dir == Direction.Up) ? -1 : (Dir == Direction.Down) ? 1 : 0);

            return new Vector2(x, y);
        }

        public Vector2 ChangeID(Direction Dir, int layer, int NewID) {
            int x = (int)GetPosition().X + ((Dir == Direction.Left) ? -1 : (Dir == Direction.Right) ? 1 : 0);
            int y = (int)GetPosition().Y + ((Dir == Direction.Up) ? -1 : (Dir == Direction.Down) ? 1 : 0);

            Tiles [layer] [x - 1][ y - 1].Id = NewID;
            //Textur [layer] [Tiles [layer] [x-1, y-1].Id].ID = NewID;

            return new Vector2(x, y);
        }
    }

}