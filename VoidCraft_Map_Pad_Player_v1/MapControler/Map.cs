using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Remoting.Contexts;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Input.Touch;

namespace MapControler {
    enum Direction {
        None, On, Left, Right, Up, Down
    }

    class Map {
        /// <summary>
        /// Variables
        /// </summary>
        GraphicsDevice GraphicDevice;
        List<List<MapTexture>> Textur; //id ,layer ,name ,path ,bitmap
        List<Tile[,]> Tiles;

        Rectangle MessagesPosition = new Rectangle(0, 0, 800, 400);
        Texture2D MessageTexture, MessageTexturBg;
        string MessageText;
        SpriteFont font;
        int MessageActive = 0;

        string MapName;
        int MapSizeX = 18, MapSizeY = 11;
        int NumberOfLayers, Height, Width;
        double MapOfsetX, MapOfsetY;
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

            this.MapZoom = (screenX / 17);

            this.InitMapZoom = this.MapZoom;

            this.MapName = MapName;
            this.MapOfsetX = 9;
            this.MapOfsetY = 6;

            GetMapSize();

            Textur = new List<List<MapTexture>>();
            Tiles = new List<Tile[,]>();

            for (int i = 0; i < NumberOfLayers; i++) {
                Textur.Add(new List<MapTexture>());

                Textur[i] = new List<MapTexture>();
                Tiles.Add(new Tile[Width, Height]);
            }

            LoadTextures();
            LoadMap();

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
                    string[] line = reader.ReadLine().Split(' ');
                    int l = int.Parse(line[0]);
                    int i = int.Parse(line[1]);
                    string name = line[2];
                    string path = line[3];

                    Textur[l].Add(new MapTexture(i, l, name, path, GetTextureFromFile("Content/Maps/" + MapName + "/" + path)));
                }
            }

            MessageTexturBg = GetTextureFromFile("Content/UI/MessageWnd.png");
        }

        private void LoadMap() {// name = Level_1
            string line;
            string[] lineNumbers;

            for (int i = 0; i < NumberOfLayers; i++) {
                int y = 0;
                using (var stream = TitleContainer.OpenStream("Content/Maps/" + MapName + "/Map/L" + (i) + ".vcmf"))
                using (var reader = new StreamReader(stream)) {
                    while (!reader.EndOfStream) {
                        line = reader.ReadLine();
                        lineNumbers = line.Split(' ');

                        int x = 0;
                        foreach (string L in lineNumbers) {
                            if (x >= Width) break;

                            if (Tiles[i][x, y] == null)
                                Tiles[i][x, y] = new Tile();
                            Tiles[i][x, y].Set(int.Parse(L), i);

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

        private void DrawMessage(SpriteBatch spriteBatch) {
            if (MessageActive == 1)
                spriteBatch.Draw(MessageTexture, MessagesPosition, Color.White);
            else if (MessageActive == 2) {
                spriteBatch.Draw(MessageTexturBg, MessagesPosition, Color.White);
                spriteBatch.DrawString(font, MessageText, new Vector2(MessagesPosition.X + 20, MessagesPosition.Y + 20), Color.White);
            }
        }

        /// <summary>
        /// Public methods ...
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, int LayerToDraw, bool LayerForMessages) {

            for (int y = 0; y < Height; y++) {//18
                for (int x = 0; x < Width; x++) {//11

                    int XX = (int)(((x - (MapOfsetX - 10)) * MapZoom));
                    int YY = (int)(((y - (MapOfsetY - 7)) * MapZoom));

                    if (YY >= 0 && (YY / MapZoom) < MapSizeY+ 1) {
                        if (XX >= 0 && (XX / MapZoom) < MapSizeX+ 1) {

                            spriteBatch.Draw(Textur[LayerToDraw][Tiles[LayerToDraw][x, y].Id].Bitmap,
                                    new Rectangle(XX- MapZoom, YY - MapZoom, MapZoom , MapZoom),
                                    Color.White);
                        }
                    } else break;
                }
            }

            if (MessageActive != 0 && LayerForMessages)
                DrawMessage(spriteBatch);

        }

        public void Update() {
            TouchCollection tl = TouchPanel.GetState();

            foreach (TouchLocation T in tl) {
                if (MessagesPosition.Contains(T.Position)) {
                    MessageActive = 0;
                    break;
                }
            }
        }

        public void Message(Texture2D MessageTexture, Rectangle MessagesPosition) {
            this.MessagesPosition = MessagesPosition;
            this.MessageTexture = MessageTexture;
            MessageActive = 1;
        }

        public void Message(string MessageTexture, SpriteFont font, Rectangle MessagesPosition) {
            this.MessagesPosition = MessagesPosition;
            MessageText = MessageTexture;
            this.font = font;
            MessageActive = 2;
        }

        /// <summary>
        /// Geters
        /// </summary>
        public Vector2 GetPosition() {
            return new Vector2(Convert.ToInt32((float)MapOfsetX), Convert.ToInt32((float)MapOfsetY));
        }

        public int GetObjectType(int Layer ,Direction Dir) {
            int i = -1;
            if (Dir != Direction.None) {
                int x = (Dir == Direction.Left) ? -1 : (Dir == Direction.Right) ? 1 : 0;
                int y = (Dir == Direction.Up) ? -1 : (Dir == Direction.Down) ? 1 : 0;


                if (GetPosition().X + x >= 0 && GetPosition().Y + y >= 0)
                    if (GetPosition().X + x < Width && GetPosition().Y + y < Height)
                        i = Textur[Layer][Tiles[Layer][((int)GetPosition().X + x)-1, ((int)GetPosition().Y + y)-1].Id].ID;

            }
            return i;
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

        public void ChangeZoom(int ToAddValue) {

            if (MapZoom + ToAddValue >= InitMapZoom && MapZoom + ToAddValue <= InitMapZoom + 50) {
                //MapZoom += ToAddValue;
                throw new NotImplementedException();
            }
        }

        public void SetPosition(double PosX, double PosY) {
            MapOfsetX = PosX;
            MapOfsetY = PosY;
        }

    }

}