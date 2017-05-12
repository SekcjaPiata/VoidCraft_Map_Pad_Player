using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using VoidCraft_Map_Pad_Player_v1.Raw_Materials_C;


namespace VoidCraft_Map_Pad_Player_v1.Tools
{
    class Tool
    {
        internal class CantCraftException : Exception
        {
            public CantCraftException(string message) : base(message) { }
        }

        //Klasa opisuje narzêdzia. Zak³adamy, ¿e takowe siê nie niszcz¹

        private Texture2D _toolTexture;//Wczytana zostanie tekstura narzêdzia

        public Texture2D ToolTexture
        {
            get { return _toolTexture; }
            set { _toolTexture = value; }
        }
        private string _toolName;

        public string ToolName
        {
            get { return _toolName; }
            set { _toolName = value; }
        }
        private bool _isOwned;

        //Zmienna mówi o tym, czy gracz posiada to narzêdzie czy nie
        public bool IsOwned
        {
            get { return _isOwned; }
            set { _isOwned = value; }
        }

        private RawMaterials _requirements;

        public RawMaterials Requirements
        {
            get { return _requirements; }
            set { _requirements = value; }
        }

        public bool CanCraft(RawMaterials PlayerMaterials)//bêdzie sprawdza³, czy wymagania pokrywaj¹ siê z posiadanym eq playera
        {
            if (PlayerMaterials.Wood < Requirements.Wood) return false;
            if (PlayerMaterials.Stone < Requirements.Stone) return false;
            if (PlayerMaterials.Lianas < Requirements.Lianas) return false;
            if (PlayerMaterials.Metal < Requirements.Metal) return false;
            if (PlayerMaterials.Water < Requirements.Water) return false;
            if (PlayerMaterials.Food < Requirements.Food) return false;
            return true;
        }

        public void Craft(ref RawMaterials PlayerMaterials)
        {
            if (CanCraft(PlayerMaterials))
            {

                PlayerMaterials.Wood -= Requirements.Wood;
                PlayerMaterials.Stone -= Requirements.Stone;
                PlayerMaterials.Lianas -= Requirements.Lianas;
                PlayerMaterials.Metal -= Requirements.Metal;
                PlayerMaterials.Water -= Requirements.Water;
                PlayerMaterials.Food -= Requirements.Food;
                IsOwned = true;
            }
            else throw new CantCraftException("Zbyt ma³o materia³ów do stworzenia przedmiotu: " + this.ToolName);
            

        }

        public Tool(string TexturePath, string ToolName, int WoodNeeded, int StoneNeeded,
            int LianasNeeded, int MetalNeeded, int WaterNeeded, int FoodNeeded)
        //kontruktor, który tworzy narzêdzie i ustawia wymagania do jego posiadania(scrafcenia) przez playera
        {
            //potrzeba pomocy w za³adowaniu tekstury! :(
            this.ToolName = ToolName;
            _requirements = new RawMaterials(WoodNeeded, StoneNeeded, LianasNeeded, MetalNeeded, WaterNeeded, FoodNeeded);
            this.IsOwned = false;
        }




    }
}