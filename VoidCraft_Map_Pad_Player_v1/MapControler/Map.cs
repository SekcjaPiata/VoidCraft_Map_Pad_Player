using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.IO;
using System;

namespace MapControler {
    public enum Direction {
        Idle_Down, Left, Right, Up, Down, Idle_Left, Idle_Right, Idle_Back, On
    }

    public enum DirectionPAC {
        Pac_Left, Pac_Right, Pac_R_Axe, Pac_L_Axe, Pac_Up, Pac_Down
    }


    // G³ówna klasa obs³uguj¹ca mape
    [Serializable]
    public class Map {
        [NonSerialized]
        GraphicsDevice GraphicDevice;
        [NonSerialized]

        List<List<MapTexture>> Textur; // Lista tekstur

        public List<Tile [] []> Tiles; // Tablica wszystki pól mapy

        string MapName; // Nazwa aktualnej mapy
        int MapSizeX = 18, MapSizeY = 11; // Iloœæ pól w osiach X i Y aktualnie rysowanych na ekranie
        int NumberOfLayers, Height, Width; // Iloœæ warstw i wysokoœæ i szerokoœæ mapy

        public double MapOfsetX { get; set; } // Aktualna pozycja mapy (gracza)
        public double MapOfsetY { get; set; }

        private int screenX; // Szerokoœæ ekranu
        private int screenY; // Wysokoœæ ekranu

        private int MapZoom; // Rozmiar 1 pola mapy

        // Konstruktor domyœlny dla celów serializacji
        public Map() {
            Textur = new List<List<MapTexture>>();
            Tiles = new List<Tile [] []>();
        }

        // Konstruktor g³ówny klasy
        public Map(GraphicsDevice GraphicDevice, string MapName, int screenX, int screenY) {
            this.GraphicDevice = GraphicDevice;
            this.screenX = screenX;
            this.screenY = screenY;

            this.MapZoom = ((screenY / 10) + (screenX / 17)) / 2; // Pbliczenie rozmiaru 1 pola wzglêdem rozdzielczoœci ekranu

            this.MapName = MapName;
            this.MapOfsetX = 9;
            this.MapOfsetY = 2;

            // Pobranie informacji o mapie z pliku *.vcmd
            GetMapSize();

            Textur = new List<List<MapTexture>>();
            Tiles = new List<Tile [] []>();

            // Alokacja pamiêci dla tablicy przechowywuj¹cej mape
            for (int i = 0; i < NumberOfLayers; i++) {
                Textur.Add(new List<MapTexture>());

                Textur [i] = new List<MapTexture>();
                Tile [] [] temp = new Tile [Width] [];
                for (int j = 0; j < Width; j++) {
                    temp [j] = new Tile [Height];
                }
                Tiles.Add(temp);

            }
            LoadTextures();
            LoadMap();
        }

        // Pobieranie informacji o mapie z pliku 
        private void GetMapSize() {
            using (var stream = TitleContainer.OpenStream("Content/Maps/" + MapName + "/mapdata.vcmd"))
            using (var reader = new StreamReader(stream)) {
                this.MapName = reader.ReadLine();
                this.Width = int.Parse(reader.ReadLine());
                this.Height = int.Parse(reader.ReadLine());
                this.NumberOfLayers = int.Parse(reader.ReadLine());
            }
        }

        // Wczytywanie listy tekstur z pliku
        private void LoadTextures() {
            using (var stream = TitleContainer.OpenStream("Content/Maps/" + MapName + "/texturelist.vctl"))
            using (var reader = new StreamReader(stream)) {
                while (!reader.EndOfStream) {
                    string [] line = reader.ReadLine().Split(' ');
                    int Layer = int.Parse(line [0]);
                    int Id = int.Parse(line [1]);
                    string Name = line [2];
                    string Path = line [3];

                    Textur [Layer].Add(new MapTexture(Id, Layer, Name, Path, GetTextureFromFile("Content/Maps/" + MapName + "/" + Path)));
                }
            }
        }

        // Wczytywanie mapy i jej warstw z plików Lx/x.vcmf
        private void LoadMap() {
            string line;
            string [] lineNumbers;

            // Wczytanie danych dla ka¿dej warstwy
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

        // Wczytywanie tekstury z podanej œcie¿ki
        private Texture2D GetTextureFromFile(String Path) {
            using (var stream = TitleContainer.OpenStream(Path)) {
                return Texture2D.FromStream(GraphicDevice, stream);
            }
        }

        // Rysowanie podanej warstywy
        public void Draw(SpriteBatch spriteBatch, int LayerToDraw) {
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    int XX = (int)(((x - (MapOfsetX - 10)) * MapZoom));
                    int YY = (int)(((y - (MapOfsetY - 7)) * MapZoom));

                    if (YY >= 0 && (YY / MapZoom) < MapSizeY + 1) {
                        if (XX >= 0 && (XX / MapZoom) < MapSizeX + 1) {
                            spriteBatch.Draw(Textur [LayerToDraw] [Tiles [LayerToDraw] [x] [y].Id].Bitmap,
                                    new Rectangle(XX - MapZoom, YY - MapZoom, MapZoom, MapZoom), Color.White);
                        }
                    } else
                        break;
                }
            }
        }

        // Pobieranie pozycji mapy
        public Vector2 GetPosition() {
            return new Vector2(Convert.ToInt32((float)MapOfsetX), Convert.ToInt32((float)MapOfsetY));
        }

        // Pobranie ID pola z podanej warstwy 
        public int GetNextID(int Layer, Direction Dir) {
            int i = -1;
            if (Dir != Direction.Idle_Down) {
                int x = (Dir == Direction.Left) ? -1 : (Dir == Direction.Right) ? 1 : 0;
                int y = (Dir == Direction.Up) ? -1 : (Dir == Direction.Down) ? 1 : 0;

                if (GetPosition().X + x >= 0 && GetPosition().Y + y >= 0) {
                    if (GetPosition().X + x < Width && GetPosition().Y + y < Height) {
                        if (((int)GetPosition().X + x) - 1 >= 0 && ((int)GetPosition().Y + y) - 1 >= 0) {
                            i = Textur [Layer] [Tiles [Layer] [((int)GetPosition().X + x) - 1] [((int)GetPosition().Y + y) - 1].Id].ID;
                        }
                    }
                }
            }
            return i;
        }

        // Pobranie ID pola na którym stoi gracz
        public int GetCurrentID(int Layer) {
            return Textur [Layer] [Tiles [Layer] [((int)GetPosition().X) - 1] [((int)GetPosition().Y) - 1].Id].ID;
        }

        // Pobranie rozmiaru 1 pola
        public int GetZoomValue() {
            return MapZoom;
        }

        // Przesuwanie mapy
        public void MoveMap(double ToAddX, double ToAddY) {
            MapOfsetX += ToAddX;
            MapOfsetY += ToAddY;
            
            if (MapOfsetX <= 1) { MapOfsetX = 1; }
            if (MapOfsetY <= 1) { MapOfsetY = 1; }

            if (MapOfsetX >= Width) { MapOfsetX = Width; }
            if (MapOfsetY >= Height) { MapOfsetY = Height; }

        }

        // Ustawienie pozycji mapy
        public void SetPosition(double PosX, double PosY) {
            MapOfsetX = PosX;
            MapOfsetY = PosY;
        }

        // Zmiana ID pola na podanej pozycji w wybranej warstwie
        public void ChangeID(int x, int y, int layer, int NewID) {
            Tiles [layer] [x - 1] [y - 1].Id = NewID;
        }

        // Zmiana ID pola w kierunku w którym gracz jest zwrócony gracza w wybranej warstwie
        public Vector2 ChangeID(Direction Dir, int layer, int NewID) {
            int x = (int)GetPosition().X + ((Dir == Direction.Left) ? -1 : (Dir == Direction.Right) ? 1 : 0);
            int y = (int)GetPosition().Y + ((Dir == Direction.Up) ? -1 : (Dir == Direction.Down) ? 1 : 0);

            Tiles [layer] [x - 1] [y - 1].Id = NewID;

            return new Vector2(x, y);
        }

        // Serializacja mapy do pliku XML
        public void SaveMap() {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            XmlSerializer xmlFormat = null;
            try {
                xmlFormat = new XmlSerializer(typeof(Map));
            } catch (Exception ex) {
                Debug.Write(ex.InnerException.ToString());
            }

            if (store.FileExists("Mapa.xml")) {
                store.DeleteFile("Mapa.xml");
            }

            var fs = store.CreateFile("Mapa.xml");
            using (StreamWriter sw = new StreamWriter(fs)) {
                xmlFormat.Serialize(sw, this);
            }
            
            if (store.FileExists("Mapa.xml")) {
                var fss = store.OpenFile("Mapa.xml", FileMode.Open);
                using (StreamReader sr = new StreamReader(fss)) {

                    string xmls = sr.ReadToEnd();

                    Debug.Write(xmls);
                    store.Close();
                }
            } else {
                Debug.Write("Plik nie istnieje");
            }
        }

        // Deserializacja mapy z pliku XML
        public void LoadMapFromXML() {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            XmlSerializer xmlFormat = null;
            try {
                xmlFormat = new XmlSerializer(typeof(Map));
            } catch (Exception ex) {
                Debug.Write(ex.InnerException.ToString());
            }

            if (store.FileExists("Mapa.xml")) {
                var fs = store.OpenFile("Mapa.xml", FileMode.Open);

                using (StreamReader sw = new StreamReader(fs)) {
                    Map temp = (Map)xmlFormat.Deserialize(sw);
                    this.Tiles = temp.Tiles;
                }
            } else
                throw new Exception("Brak pliku z zapisem mapy");
        }
    }
}