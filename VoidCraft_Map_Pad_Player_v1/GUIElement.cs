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
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
namespace VoidCraft_Map_Pad_Player_v1
{
  public  class GUIElement
    {
        private Texture2D GUITexture;
        private Rectangle GUIRect;

       // private string assetName;
        public string AssetName { get; set; }

        public delegate void ElementClicked(string element);
        public event ElementClicked clickEvent;

        public GUIElement(string assetName)
        {
            this.AssetName = assetName;
        }

        public void LoadContent(ContentManager content)
        {
            GUITexture = content.Load<Texture2D>(AssetName); // GUITEXTURE instancja Texture2D
           
    
            
            GUIRect = new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width/17, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height/17);


        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(GUITexture, GUIRect, Color.White);
        }
        public void Update()
        {
            TouchCollection touchcollection =  TouchPanel.GetState();
          
            
            foreach (TouchLocation tl in touchcollection) // włazi w Kwadrat
            if (GUIRect.Contains(tl.Position))
            {
                    clickEvent(AssetName);
            }
      
        }

        public void CenterElement(int height,int width)
        {
                GUIRect = new Rectangle((width / 2) - (this.GUITexture.Width / 2), (height / 2) - (this.GUITexture.Height / 2), this.GUITexture.Width / 2, this.GUITexture.Height/2 );

        }

        public void MoveElement(int x,int y)
        {
            GUIRect = new Rectangle(GUIRect.X += x, GUIRect.Y += y, GUIRect.Width, GUIRect.Height);
        }

        public void Background()
        {
            GUIRect = new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        }

    }


}