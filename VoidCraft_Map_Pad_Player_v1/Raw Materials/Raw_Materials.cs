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

namespace VoidCraft_Map_Pad_Player_v1.Raw_Materials
{
    class Raw_Materials
    {
        //Obiekt tej klasy bêdzie posiada³ w sobie player.
        //Klasa ta reprezentuje aktualny stan zasobów podstawowowych opisanych w pliku Raw Materials.txt
        private uint _wood;

        public uint Wood
        {
            get { return _wood; }
            set { _wood = value; }
        }
        //Dokoñczê potem ^^



    }
}