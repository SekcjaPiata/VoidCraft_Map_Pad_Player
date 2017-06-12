using Microsoft.Xna.Framework.Graphics;

namespace MapControler {
    // Klasa przechowuj¹ca teksture mapy i informacje do jakiej warstwy przynale¿y i jakie ma ID
    class MapTexture {
        public Texture2D Bitmap { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Layer { get; set; }
        public int ID { get; set; }

        public MapTexture(int ID, int Layer, string Name, string Path, Texture2D Bitmap) {
            this.ID = ID;
            this.Layer = Layer;
            this.Name = Name;
            this.Path = Path;
            this.Bitmap = Bitmap;
        }
    }
}