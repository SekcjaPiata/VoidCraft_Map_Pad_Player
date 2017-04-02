using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MapControler {
    class Tile {
        public int Id { get; set; }
        public int Layer { get; set; }

        public Tile() { }

        public Tile(int Id,int Layer) {
            Set(Id, Layer);
        }

        public void Set(int Id, int Layer) {
            this.Id = Id;
            this.Layer = Layer;
        }
        
    }
}