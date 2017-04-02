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

namespace PadControler {

    public enum GamePadStatus {
        None,
        Up, Down, Right, Left,
        A,B,
        ZoomOut,ZoomIn
    }

    class GameControler {

        private GraphicsDevice graphicsDevice;
        List<PadButton> Buttons;

        PadButton Up, Down, Left, Right ,A,B;

        public GameControler(GraphicsDevice graphicsDevice,int ScreenWidth,int ScreenHeight) {
            this.graphicsDevice = graphicsDevice;
            Buttons = new List<PadButton>();

            int BtnSize = ScreenWidth/17;

            Up = new PadButton(graphicsDevice, "Content/UI/Pad/UP.png", new Rectangle((2 * BtnSize), ScreenHeight - (4*BtnSize), BtnSize, BtnSize));

            Down = new PadButton(graphicsDevice, "Content/UI/Pad/DOWN.png", new Rectangle((2 * BtnSize), ScreenHeight- (2 * BtnSize), BtnSize, BtnSize));

            Right = new PadButton(graphicsDevice, "Content/UI/Pad/RIGHT.png", new Rectangle((3 * BtnSize), ScreenHeight - (3 * BtnSize), BtnSize, BtnSize));

            Left = new PadButton(graphicsDevice, "Content/UI/Pad/LEFT.png", new Rectangle(BtnSize, ScreenHeight - (3 * BtnSize), BtnSize, BtnSize));


            A = new PadButton(graphicsDevice, "Content/UI/Pad/A.png", new Rectangle(ScreenWidth- (2 * BtnSize), ScreenHeight - (3 * BtnSize), BtnSize, BtnSize));

            B = new PadButton(graphicsDevice, "Content/UI/Pad/B.png", new Rectangle(ScreenWidth - (4 * BtnSize), ScreenHeight - (2 * BtnSize), BtnSize, BtnSize));
        }

        public List<GamePadStatus> GamePadState() {
            List<GamePadStatus> ToReturn = new List<GamePadStatus>();

            TouchCollection tl = TouchPanel.GetState();

            if(tl.Count == 0) ToReturn.Add(GamePadStatus.None);

            foreach (TouchLocation T in tl) {

                if (Up.Position.Contains(T.Position)) {
                    ToReturn.Add(GamePadStatus.Up);
                } else if (Down.Position.Contains(T.Position)) {
                    ToReturn.Add(GamePadStatus.Down);
                } else if (Left.Position.Contains(T.Position)) {
                    ToReturn.Add(GamePadStatus.Left);
                } else if (Right.Position.Contains(T.Position)) {
                    ToReturn.Add(GamePadStatus.Right);
                } else {
                    ToReturn.Add(GamePadStatus.None);
                }

                if (A.Position.Contains(T.Position)) {
                    ToReturn.Add(GamePadStatus.A);
                } else if (B.Position.Contains(T.Position)) {
                    ToReturn.Add(GamePadStatus.B);
                }
            }
            return ToReturn;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Up.Bitmap, Up.Position, Color.White);
            spriteBatch.Draw(Down.Bitmap, Down.Position, Color.White);
            spriteBatch.Draw(Right.Bitmap, Right.Position, Color.White);
            spriteBatch.Draw(Left.Bitmap, Left.Position, Color.White);
            spriteBatch.Draw(A.Bitmap, A.Position, Color.White);
            spriteBatch.Draw(B.Bitmap, B.Position, Color.White);
        }

    }
}