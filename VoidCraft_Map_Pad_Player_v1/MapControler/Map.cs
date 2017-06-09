using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace MapControler {
    enum Direction {
        Idle_Down, Left, Right, Up, Down, Idle_Left, Idle_Right, Idle_Back, On
    }

    enum DirectionPAC {
        Pac_Left, Pac_Right, Pac_R_Axe
    }

    class Map {
        GraphicsDevice GraphicDevice;
        List<List<MapTexture>> Textur; //id ,layer ,name ,path ,bitmap
        List<Tile [,]> Tiles;
        
        internal int MessageActive = 0;

        string MapName;
        private const int MapSizeX = 18, MapSizeY = 11;
        int NumberOfLayers, Height, Width;

        public double MapOfsetX { get; set; }
        public double MapOfsetY { get; set; }

        private int screenX;
        private int screenY;
        private int InitMapZoom;

        private int MapZoom;
        
        public Map(GraphicsDevice GraphicDevice, string MapName, int screenX, int screenY) {
            this.GraphicDevice = GraphicDevice;
            this.screenX = screenX;
            this.screenY = screenY;

            this.MapZoom = ((screenY / 10) + (screenX / 17)) / 2; 

            this.InitMapZoom = this.MapZoom;

            this.MapName = MapName;
            this.MapOfsetX = 9;
            this.MapOfsetY = 2;

            GetMapSize();

            Textur = new List<List<MapTexture>>();
            Tiles = new List<Tile [,]>();

            for (int i = 0; i < NumberOfLayers; i++) {
                Textur.Add(new List<MapTexture>());

                Textur [i] = new List<MapTexture>();
                Tiles.Add(new Tile [Width, Height]);
            }

            LoadTextures();
            LoadMap();

        }
        
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

        private void LoadMap() {
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

                            if (Tiles [i] [x, y] == null)
                                Tiles [i] [x, y] = new Tile();
                            Tiles [i] [x, y].Set(int.Parse(L), i);

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

        public void Draw(SpriteBatch spriteBatch, int LayerToDraw) {

            for (int y = 0; y < Height; y++) {//18
                for (int x = 0; x < Width; x++) {//11

                    int XX = (int)(((x - (MapOfsetX - 10)) * MapZoom));
                    int YY = (int)(((y - (MapOfsetY - 7)) * MapZoom));

                    if (YY >= 0 && (YY / MapZoom) < MapSizeY + 1) {
                        if (XX >= 0 && (XX / MapZoom) < MapSizeX + 1) {

                            spriteBatch.Draw(Textur [LayerToDraw] [Tiles [LayerToDraw] [x, y].Id].Bitmap,
                                    new Rectangle(XX - MapZoom, YY - MapZoom, MapZoom, MapZoom),
                                    Color.White);
                        }
                    } else
                        break;
                }
            }
        }

        public Vector2 GetPosition() {
            return new Vector2(Convert.ToInt32((float)MapOfsetX), Convert.ToInt32((float)MapOfsetY));
        }

        public int GetNextID(int Layer, Direction Dir) {
            int i = -1;
            if (Dir != Direction.Idle_Down) {
                int x = (Dir == Direction.Left) ? -1 : (Dir == Direction.Right) ? 1 : 0;
                int y = (Dir == Direction.Up) ? -1 : (Dir == Direction.Down) ? 1 : 0;

                if (GetPosition().X + x >= 0 && GetPosition().Y + y >= 0) {
                    if (GetPosition().X + x < Width && GetPosition().Y + y < Height) {
                        if (((int)GetPosition().X + x) - 1 >= 0 && ((int)GetPosition().Y + y) - 1 >= 0) {
                            i = Textur [Layer] [Tiles [Layer] [((int)GetPosition().X + x) - 1, ((int)GetPosition().Y + y) - 1].Id].ID;
                        }
                    }
                }
            }
            return i;
        }

        public int GetCurrentID(int Layer) {
            return Textur [Layer] [Tiles [Layer] [((int)GetPosition().X) - 1, ((int)GetPosition().Y) - 1].Id].ID;
        }

        public int GetZoomValue() {
            return MapZoom;
        }
        
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
            Tiles [layer] [x - 1, y - 1].Id = NewID;
        }

        public Vector2 GetNextCords(Direction Dir) {
            int x = (int)GetPosition().X + ((Dir == Direction.Left) ? -1 : (Dir == Direction.Right) ? 1 : 0);
            int y = (int)GetPosition().Y + ((Dir == Direction.Up) ? -1 : (Dir == Direction.Down) ? 1 : 0);

            return new Vector2(x, y);
        }

        public Vector2 ChangeID(Direction Dir, int layer, int NewID) {
            int x = (int)GetPosition().X + ((Dir == Direction.Left) ? -1 : (Dir == Direction.Right) ? 1 : 0);
            int y = (int)GetPosition().Y + ((Dir == Direction.Up) ? -1 : (Dir == Direction.Down) ? 1 : 0);

            Tiles [layer] [x - 1, y - 1].Id = NewID;

            return new Vector2(x, y);
        }
    }
}