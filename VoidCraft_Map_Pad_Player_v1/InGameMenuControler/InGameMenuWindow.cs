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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace InGameMenuControler
{
    public enum InGameMenuState { _Game, _Settings, _Inventory, _Crafting, _Quests }
    public enum ItemToCraftChosen { _Hammer, _Pickaxe, _Axe, _Saw, _None }

    public class InGameMenuWindow
    {
        ContentManager Content;
        public InGameMenuState MenuState; // Identyfikator okna
        public Texture2D InGameMenuWindowTexture; // Tekstura g³ówna okna
        public Rectangle InGameMenuWindowPos; // Pozycja okna do rysowania
        public List<Texture2D> Buttons; // Tekstury klawiszy
        public List<Rectangle> ButtonsPos; // Pozycje klawiszy
        public List<Texture2D> Icons; // Tekstury ikon wyœwietlanych w menu
        public List<Rectangle> IconsPos; // Tekstury ikon wyœwietlanych w menu
        public InGameMenuWindow(InGameMenuState _InGameMenuState, String _InGameMenuWindowTextureName, List<Texture2D> _Buttons, List<Rectangle> _ButtonPos, List<Texture2D> _Icons, List<Rectangle> _IconsPos, ContentManager _Content)
        {
            Content = _Content;
            // MenuState = InGameMenuState._Game;
            MenuState = _InGameMenuState;
            InGameMenuWindowTexture = Content.Load<Texture2D>(_InGameMenuWindowTextureName);
            InGameMenuWindowPos = new Rectangle(50, 50, (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100));
            Buttons = _Buttons;
            ButtonsPos = _ButtonPos;
            Icons = _Icons;
            IconsPos = _IconsPos;
        }
        public void DrawInGameMenuWindow(SpriteBatch _SpriteBatch)
        {

            _SpriteBatch.Draw(InGameMenuWindowTexture, InGameMenuWindowPos, Color.White); // Rysowanie g³ównego okna

            // --------------------------------------- Przyciski --------------------------------------------------------------

            for (int i = 0; i < Buttons.Count; i++) // Pêtla rysowania przycisków
            {
                _SpriteBatch.Draw(Buttons.ElementAt(i), ButtonsPos.ElementAt(i), Color.White); // Rysowanie przycisków ( Tak jak by³y dodane do listy !!! )
            }
            // ---------------------------------------------------------------------------------------------------------------


            // --------------------------------------- Ikony -----------------------------------------------------------------
            if (Icons != null)
            {
                for (int i = 0; i < Icons.Count; i++) // Pêtla rysowania ikon
                {
                    _SpriteBatch.Draw(Icons.ElementAt(i), IconsPos.ElementAt(i), Color.White); // Rysowanie dodatkowych ikonek
                }
            }

            // ---------------------------------------------------------------------------------------------------------------

        }
    }
}