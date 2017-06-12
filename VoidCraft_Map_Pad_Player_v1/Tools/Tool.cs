using System;
using Microsoft.Xna.Framework.Graphics;
using Raw_Materials_C;
using System.Collections.Generic;

namespace Tools
{
    [Serializable]
    public class Tool
    {
        internal class CantCraftException : Exception
        {
            public CantCraftException(string message) : base(message) { }
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

        private Tool [] tools_needed;

        public Tool [] Tools_needed
        {
            get { return tools_needed; }
            set { tools_needed = value; }
        }


        private RawMaterials _requirements;

        public RawMaterials Requirements
        {
            get { return _requirements; }
            set { _requirements = value; }
        }

        public bool CanCraft(RawMaterials PlayerMaterials, List<Tool> PlayerTools)//bêdzie sprawdza³, czy wymagania pokrywaj¹ siê z posiadanym eq playera
        {
            if (tools_needed != null)
            {
                foreach (Tool t in tools_needed)
                {

                    if (PlayerTools.Find(x => x.ToolName == t.ToolName).IsOwned == false)
                    {
                        return false;
                    }
                }
            }
            return PlayerMaterials.Contains(this._requirements);
        }

        public void Craft(RawMaterials PlayerMaterials, List<Tool> PlayerTools)
        {
            if (CanCraft(PlayerMaterials, PlayerTools))
            {

                PlayerMaterials.Wood -= Requirements.Wood;
                PlayerMaterials.Stone -= Requirements.Stone;
                PlayerMaterials.Lianas -= Requirements.Lianas;
                PlayerMaterials.Metal -= Requirements.Metal;
                PlayerMaterials.Water -= Requirements.Water;
                PlayerMaterials.Food -= Requirements.Food;
                IsOwned = true;
            } else
                throw new CantCraftException("Zbyt malo materialow do stworzenia przedmiotu: " + this.ToolName);


        }

        public Tool( string ToolName, int WoodNeeded, int StoneNeeded,
            int LianasNeeded, int MetalNeeded, int WaterNeeded, int FoodNeeded)
        //kontruktor, który tworzy narzêdzie i ustawia wymagania do jego posiadania(scrafcenia) przez playera
        {
            //potrzeba pomocy w za³adowaniu tekstury! :(
            this.ToolName = ToolName;
            _requirements = new RawMaterials(WoodNeeded, StoneNeeded, LianasNeeded, MetalNeeded, WaterNeeded, FoodNeeded);
            this.IsOwned = false;
            tools_needed = null;
           // this._toolTexture = TexturePath;
        }
        public Tool( string ToolName, int WoodNeeded, int StoneNeeded,
           int LianasNeeded, int MetalNeeded, int WaterNeeded, int FoodNeeded, params Tool [] ToolsNeeded)
        //kontruktor, który tworzy narzêdzie i ustawia wymagania do jego posiadania(scrafcenia) przez playera i wymagane narzêdzia
        {
            //potrzeba pomocy w za³adowaniu tekstury! :(
            this.ToolName = ToolName;
            _requirements = new RawMaterials(WoodNeeded, StoneNeeded, LianasNeeded, MetalNeeded, WaterNeeded, FoodNeeded);
            this.IsOwned = false;
           // this._toolTexture = TexturePath;
            this.Tools_needed = ToolsNeeded;
        }
        public Tool()
        {

        }



    }
}