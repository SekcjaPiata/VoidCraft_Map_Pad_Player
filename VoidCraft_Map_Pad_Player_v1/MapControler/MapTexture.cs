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