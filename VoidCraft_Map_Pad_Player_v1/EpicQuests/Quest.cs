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
using Microsoft.Xna.Framework;

namespace EpicQuests
{
    class Quest
    {
        //  TODO:
        //wymyœliæ jak zapisywaæ wartoœci dla spe³nionych warunków questa



        private String name;//nazwa questa

        public String Name
        {
            get { return name; }
            set { String name = value; }
        }

        private Vector2 start_position;

        public Vector2 Start_position
        {
            get { return start_position; }
            set { start_position = value; }
        }
        //Wymagania dla questa, zawieraj¹ pozycjê i to, czy dana pozycja zosta³a zaliczona(bool) czy nie
        private Dictionary<Vector2, bool> quest_requirements;

        public Dictionary<Vector2, bool> Quest_requirements
        {
            get { return quest_requirements; }
            set { quest_requirements = value; }
        }




        public Quest(String name, Vector2 start_position, params Vector2 [] quest_places)
        {
            this.name = name;
            this.Start_position = new Vector2(start_position.X, start_position.Y);

            for (int i = 0; i < quest_places.Length; i++)
            {
                Quest_requirements.Add(quest_places [i], false);//dodaje miejsca, ktore trzeba zaliczyæ i domyœlnie ustawia je na false
            }

        }


    }
}