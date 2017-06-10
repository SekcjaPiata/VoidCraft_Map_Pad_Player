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

namespace EpicQuests
{
    [Serializable]
    public class Dairy
    {
        public List<string> dairy_notes { get; set; }

        public Dairy()
        {
            dairy_notes = new List<string>();
        }
        //public Dairy() { }
    }
}