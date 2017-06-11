using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using VoidCraft_Map_Pad_Player_v1;

namespace Menu
{



    public class MainMenu
    {
        private int c = 1;
     

        enum GameState { MainMenu, authors, inGame }
        GameState gamestate;

        List<GUIElement> main = new List<GUIElement>();
        List<GUIElement> authors = new List<GUIElement>();
        

        //  private Keys
        public MainMenu( )
        {
            
            main.Add(new GUIElement("Menu\\M_BACK"));
            main.Add(new GUIElement("Menu\\B_graj"));
            main.Add(new GUIElement("Menu\\Wczytaj"));
            main.Add(new GUIElement("Menu\\B_autorzy"));
            main.Add(new GUIElement("Menu\\voidscraft"));
            //  options.Add(new GUIElement("2"));

            authors.Add(new GUIElement("Menu\\M_BACK"));
            authors.Add(new GUIElement("Menu\\B_powrot"));
        }

        public void LoadContent(ContentManager content)
        {

            foreach (GUIElement element in main)
            {
                element.LoadContent(content);

                element.CenterElement((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 700) + 250 * c, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
                c++;
                element.clickEvent += OnClick;
            }
            c = 1;
            main.Find(x => x.AssetName == "Menu\\M_BACK").Background();
            main.Find(x => x.AssetName == "Menu\\voidscraft").Resize(50, 150);
            main.Find(x => x.AssetName == "Menu\\voidscraft").MoveElement(-50, -570);

            foreach (GUIElement element in authors)
            {
                element.LoadContent(content);

                element.CenterElement((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 500) + 250 * c, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
                element.clickEvent += OnClick;
                c++;
            }
            authors.Find(x => x.AssetName == "Menu\\B_powrot").MoveElement(-500, 300);
            authors.Find(x => x.AssetName == "Menu\\M_BACK").Background();
            c = 1;


        }

        public void Update()
        {
            switch (gamestate)
            {
                case GameState.MainMenu:
                    foreach (GUIElement element in main)
                    {
                        element.Update();
                    }
                    break;
                case GameState.authors:
                    foreach (GUIElement element in authors)
                    {
                        element.Update();
                    }
                    break;
                case GameState.inGame:
                    break;
   
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (gamestate)
            {
                case GameState.MainMenu:
                    foreach (GUIElement element in main)
                    {
                        element.Draw(spriteBatch);

                    }
                    break;
                case GameState.authors:
                    foreach (GUIElement element in authors)
                    {

                        element.Draw(spriteBatch);

                    }
                    break;
                case GameState.inGame:
                    break;
        

                    break;

            }


        }



        public void OnClick(string element)
        {

            if (element == "Menu\\B_graj")
            {
                Game1.GameRunning = true;
                Game1.SongPlayed = 1;
                // plays the game
                gamestate = GameState.inGame;
            }
            if(element=="Menu\\Wczytaj")
            {
                // to do wczytywanie 
                Game1.GameRunning = true;
                Game1.LoadedGame = true;
                Game1.SongPlayed = 1;
                 gamestate = GameState.inGame;
                
              


            }
            if (element == "Menu\\B_autorzy")
            {
                gamestate = GameState.authors;
            

            }
            if (element == "Menu\\done")//Done button
            {
                gamestate = GameState.MainMenu;
            }
    

            if (element == "Menu\\B_powrot")
            {
                gamestate = GameState.MainMenu;

            }



        }

    }
}