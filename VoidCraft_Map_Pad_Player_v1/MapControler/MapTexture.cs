using Microsoft.Xna.Framework.Graphics;

namespace MapControler
{
    class MapTexture
    {
        public int ID { get; set; }
        public int Layer { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Texture2D Bitmap { get; set; }
        
        public MapTexture(int id, int layer, string name, string path, Texture2D bitmap)
        {
            ID = id;
            Layer = layer;
            Name = name;
            Path = path;
            Bitmap = bitmap;
        }

    }
}