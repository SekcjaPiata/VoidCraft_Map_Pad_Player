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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;

namespace InGameMenuControler
{
    public class InGameMenu
    {
        private int _ScreenX, _ScreenY;
        public List<InGameMenuWindow> InGameMenuWindows;
     //   private SpriteBatch spritebatch;
        private ContentManager content;
        public Texture2D InGameMenuButton;
        public Rectangle InGameMenuButtonPos;

        public List<Texture2D> SettingsButtons;
        public List<Rectangle> SettingsButtonsPos;

        public List<Texture2D> InventoryButtons;
        public List<Rectangle> InventoryButtonsPos;

        public List<Texture2D> CraftingButtons;
        public List<Rectangle> CraftingButtonsPos;

        public List<Texture2D> QuestsButtons;
        public List<Rectangle> QuestsButtonsPos;

        public List<Texture2D> InventoryIcons;
        public List<Rectangle> InventoryIconsPos;

        public List<Texture2D> CraftingIcons;
        public List<Rectangle> CraftingIconsPos;


        public InGameMenu(ContentManager _Content)
        {
            content = _Content;
            _ScreenX = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _ScreenY = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            // InGameMenuButton
            InGameMenuButton = content.Load<Texture2D>("Buttons/Button_InGameMenu");
            InGameMenuButtonPos = new Rectangle((int)(_ScreenX / 1.125), 50, 50, 50);

            // ---------- Allokacja List ----------
            SettingsButtons = new List<Texture2D>();
            SettingsButtonsPos = new List<Rectangle>();

            //----
            InventoryButtons = new List<Texture2D>();
            InventoryButtonsPos = new List<Rectangle>();
            InventoryIcons = new List<Texture2D>();
            InventoryIconsPos = new List<Rectangle>();
            //----


            //----
            CraftingButtons = new List<Texture2D>();
            CraftingButtonsPos = new List<Rectangle>();
            CraftingIcons = new List<Texture2D>();
            CraftingIconsPos = new List<Rectangle>();
            //----


            QuestsButtons = new List<Texture2D>();
            QuestsButtonsPos = new List<Rectangle>();

            InGameMenuWindows = new List<InGameMenuWindow>();




            // ---------- SettingsButtons ----------

            // Index 0 - Button do dŸwiêku
            SettingsButtons.Add(content.Load<Texture2D>("Buttons/Button_Checked"));
            SettingsButtonsPos.Add(new Rectangle(250, 200, 50, 50));

            // Index 1 - Button do zapisu
            SettingsButtons.Add(content.Load<Texture2D>("Buttons/Button_Checked"));
            SettingsButtonsPos.Add(new Rectangle(250, 300, 50, 50));

            // Index 2 - Button do przejœcia w prawo
            SettingsButtons.Add(content.Load<Texture2D>("Buttons/Button_Right"));
            SettingsButtonsPos.Add(new Rectangle(_ScreenX / 2 + 100, _ScreenY - 150, 50, 50));


            // ---------- InventoryButtons ----------

            // Index 0 - Button do przejœcia w lewo
            InventoryButtons.Add(content.Load<Texture2D>("Buttons/Button_Left"));
            InventoryButtonsPos.Add(new Rectangle(_ScreenX / 2 - 100, _ScreenY - 150, 50, 50));

            // Index 1 - Button do przejœcia w prawo
            InventoryButtons.Add(content.Load<Texture2D>("Buttons/Button_Right"));
            InventoryButtonsPos.Add(new Rectangle(_ScreenX / 2 + 100, _ScreenY - 150, 50, 50));

            // ---------- InventoryIcons ----------

            // Index 0 - Drewno
            InventoryIcons.Add(content.Load<Texture2D>("Icons/Icon_Wood"));
            InventoryIconsPos.Add(new Rectangle(100, 150, 50, 50));

            // Index 1 - Kamieñ
            InventoryIcons.Add(content.Load<Texture2D>("Icons/Icon_Stone"));
            InventoryIconsPos.Add(new Rectangle(100, 200, 50, 50));

            // Index 2 - Liany
            InventoryIcons.Add(content.Load<Texture2D>("Icons/Icon_Liane"));
            InventoryIconsPos.Add(new Rectangle(100, 250, 50, 50));

            // Index 3 - Jedzenie
            InventoryIcons.Add(content.Load<Texture2D>("Icons/Icon_Food"));
            InventoryIconsPos.Add(new Rectangle(100, 300, 50, 50));

            // Index 4 - Woda
            InventoryIcons.Add(content.Load<Texture2D>("Icons/Icon_Water"));
            InventoryIconsPos.Add(new Rectangle(100, 350, 50, 50));

            // Index 5 - M³otek
            InventoryIcons.Add(content.Load<Texture2D>("Icons/Icon_HammerLocked"));
            InventoryIconsPos.Add(new Rectangle(_ScreenX - 150, 150, 50, 50));

            // Index 6 - Kilof
            InventoryIcons.Add(content.Load<Texture2D>("Icons/Icon_PickaxeLocked"));
            InventoryIconsPos.Add(new Rectangle(_ScreenX - 150, 200, 50, 50));

            // Index 7 - Topór
            InventoryIcons.Add(content.Load<Texture2D>("Icons/Icon_AxeLocked"));
            InventoryIconsPos.Add(new Rectangle(_ScreenX - 150, 250, 50, 50));

            // Index 8 - Pi³a
            InventoryIcons.Add(content.Load<Texture2D>("Icons/Icon_SawLocked"));
            InventoryIconsPos.Add(new Rectangle(_ScreenX - 150, 300, 50, 50));

            // ---------- CraftingButtons ----------

            // Index 0 - Button do przejœcia w lewo
            CraftingButtons.Add(content.Load<Texture2D>("Buttons/Button_Left"));
            CraftingButtonsPos.Add(new Rectangle(_ScreenX / 2 - 100, _ScreenY - 150, 50, 50));

            // Index 1 - Button do przejœcia w prawo
            CraftingButtons.Add(content.Load<Texture2D>("Buttons/Button_Right"));
            CraftingButtonsPos.Add(new Rectangle(_ScreenX / 2 + 100, _ScreenY - 150, 50, 50));

            // ---------- QuestsButtons ----------

            // Index 0 - Button do przejœcia w lewo
            QuestsButtons.Add(content.Load<Texture2D>("Buttons/Button_Left"));
            QuestsButtonsPos.Add(new Rectangle(_ScreenX / 2 - 100, _ScreenY - 150, 50, 50));


            InGameMenuWindows.Add(new InGameMenuWindow(InGameMenuState._Settings, "MenuWindows/MenuWindow_Settings", SettingsButtons, SettingsButtonsPos, null, null, content));
            InGameMenuWindows.Add(new InGameMenuWindow(InGameMenuState._Inventory, "MenuWindows/MenuWindow_Inventory", InventoryButtons, InventoryButtonsPos, InventoryIcons, InventoryIconsPos, content));
            InGameMenuWindows.Add(new InGameMenuWindow(InGameMenuState._Crafting, "MenuWindows/MenuWindow_Crafting", CraftingButtons, CraftingButtonsPos, CraftingIcons, CraftingIconsPos, content));
            InGameMenuWindows.Add(new InGameMenuWindow(InGameMenuState._Quests, "MenuWindows/MenuWindow_Quests", QuestsButtons, QuestsButtonsPos, null, null, content));
        }

        public void DrawInGameMenuButton(SpriteBatch spritebatch)
        {
            spritebatch.Draw(InGameMenuButton, InGameMenuButtonPos, Color.White);
        }
        public void DrawInGameMenu(InGameMenuState _GameState, SpriteBatch spritebatch)
        {
            for (int i = 0; i < InGameMenuWindows.Count; i++)
            {
                if (InGameMenuWindows.ElementAt(i).MenuState == _GameState) InGameMenuWindows.ElementAt(i).DrawInGameMenuWindow(spritebatch);
            }

            


            //foreach(InGameMenuWindow TTT in InGameMenuWindows)
            //{
            //    if (TTT.MenuState == _GameState) TTT.DrawInGameMenuWindow(spritebatch);
            //}

        }
    }
}