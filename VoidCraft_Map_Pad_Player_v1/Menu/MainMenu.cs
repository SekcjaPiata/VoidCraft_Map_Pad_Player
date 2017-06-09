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
        private Song mor;

        enum GameState { MainMenu, authors, inGame, Options }
        GameState gamestate;

        List<GUIElement> options = new List<GUIElement>();
        List<GUIElement> main = new List<GUIElement>();
        public List<GUIElement> enterName = new List<GUIElement>();

        //  private Keys
        public MainMenu()
        {

            main.Add(new GUIElement("Menu\\M_BACK"));
            main.Add(new GUIElement("Menu\\B_graj"));
            main.Add(new GUIElement("Menu\\B_ustawianie"));
            main.Add(new GUIElement("Menu\\B_autorzy"));
            main.Add(new GUIElement("Menu\\voidscraft"));
            //  options.Add(new GUIElement("2"));
            options.Add(new GUIElement("Menu\\Bot"));

            enterName.Add(new GUIElement("Menu\\name"));
            enterName.Add(new GUIElement("Menu\\done"));
        }

        public void LoadContent(ContentManager content)
        {

            foreach (GUIElement element in main)
            {
                element.LoadContent(content);

                element.CenterElement((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 500) + 250 * c, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
                c++;
                element.clickEvent += OnClick;
            }
            c = 1;
            main.Find(x => x.AssetName == "Menu\\M_BACK").Background();
            main.Find(x => x.AssetName == "Menu\\voidscraft").MoveElement(0, -600);

            foreach (GUIElement element in enterName)
            {
                element.LoadContent(content);

                element.CenterElement((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 500) + 250 * c, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
                element.clickEvent += OnClick;
                c++;
            }


            foreach (GUIElement element in options)
            {

                element.LoadContent(content);
                element.CenterElement(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
                element.clickEvent += OnClick;
            }

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
                foreach (GUIElement element in enterName)
                {
                    element.Update();
                }
                break;
                case GameState.inGame:
                break;
                case GameState.Options:
                foreach (GUIElement element in options)
                {
                    element.Update();
                }

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
                foreach (GUIElement element in enterName)
                {

                    element.Draw(spriteBatch);

                }
                break;
                case GameState.inGame:
                break;
                case GameState.Options:
                foreach (GUIElement element in options)
                {
                    element.Draw(spriteBatch);
                }

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
            if (element == "Menu\\B_autorzy")
            {
                gamestate = GameState.authors;


            }
            if (element == "Menu\\done")//Done button
            {
                gamestate = GameState.MainMenu;
            }
            if (element == "Menu\\B_ustawianie")
            {
                gamestate = GameState.Options;
            }
            if (element == "Menu\\Bot")
            {
                // To do: Tuaj będą "ustawienia" jakieś pomysły ?
            }
        }
        //private void GetKeys()
        //{
        //    KeyboardState state = Keyboard.GetState();
        //   // TouchCollection state = TouchPanel.GetState();
        //    System.Text.StringBuilder sb = new StringBuilder();
        //    foreach (var key in state.GetPressedKeys())
        //        sb.Append(key);

        //    if (sb.Length > 0) System.Diagnostics.Debug.WriteLine(sb.ToString());
        //    else
        //        System.Diagnostics.Debug.WriteLine("NIc sie nie wcisnęło");

        //}
    }
}