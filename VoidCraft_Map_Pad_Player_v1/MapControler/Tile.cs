using System;

namespace MapControler {
    [Serializable]
    // Klasa opisuj¹ca 1 pole mapy
    public class Tile {
        public int Id { get; set; }
        public int Layer { get; set; }

        public Tile() { }

        public Tile(int Id, int Layer) {
            Set(Id, Layer);
        }

        public void Set(int Id, int Layer) {
            this.Id = Id;
            this.Layer = Layer;
        }
    }
}