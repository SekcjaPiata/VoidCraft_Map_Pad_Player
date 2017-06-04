using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Raw_Materials_C;
using Tools;
namespace EpicQuests
{
    class Quest
    {
        //  TODO:
        //wymyœliæ jak zapisywaæ wartoœci dla spe³nionych warunków questa

       

        public string Name { get; set; }

        public string Quest_message { get; set; }

        public Vector2 Start_position { get; set; }
                                           //Wymagania dla questa, zawieraj¹ pozycjê i to, czy dana pozycja zosta³a zaliczona(bool) czy nie
       
        public RawMaterials Materials_needed { get; set; }

        public bool Materials_for_quest_owned { get; set; }

        public List<Tool>Tools_needed { get; set; }


        public bool Activated { get; set; }


        public bool Finished { get; set; }

        public bool IsFinished(RawMaterials player_materials,List<Tool> player_tools)
        {
            if (Finished) return Finished;//jeœli ju¿ jest ukoñczony nie przeszukuj tylko zwróæ, ¿e ukoñczono

            if (!player_materials.Contains(Materials_needed)) return false;
            foreach (Tool t in Tools_needed)
            {
                if (!t.CanCraft(player_materials)) return false;
                // if (!player_tools.Contains(t)) return false; -> wersja na póŸniejsze questy
                // Gracz.Tools.Find(x => x.ToolName == "Axe").IsOwned == true)
            }
            Materials_for_quest_owned = true;
            Finished = true;
            return true;
        }


        public Quest(String name, Vector2 startposition,RawMaterials requested_materials,string quest_message, params Tool[] tools_needed)
        {
            this.Name = name;
            this.Quest_message = quest_message;
            Start_position = new Vector2(startposition.X,startposition.Y);
            //Start_position.X = startposition.X;//, startposition.Y);
            // Start_position.Y = startposition.Y;
            //quest_requirements = new Dictionary<Vector2, bool>();
            //for (int i = 0; i < quest_places.Length; i++)
            //{
            //    Quest_requirements.Add(quest_places [i], false);//dodaje miejsca, ktore trzeba zaliczyæ i domyœlnie ustawia je na false
            //}
            Materials_needed = requested_materials;
            //this.Tools_needed = new List<Tool>();
            this.Tools_needed = new List<Tool>();

            for (int i = 0; i < tools_needed.Length; i++)
            {
                this.Tools_needed.Add(tools_needed[i]);
            }

            this.Activated = false;
        }


    }
}