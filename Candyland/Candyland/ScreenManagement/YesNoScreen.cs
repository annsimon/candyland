using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class YesNoScreen : GameScreen
    {
        protected Texture2D yes;
        protected Texture2D no;
        protected Texture2D yesSelected;
        protected Texture2D noSelected;

        protected string play = "?";

        protected Rectangle MenuBoxTL;
        protected Rectangle MenuBoxTR;
        protected Rectangle MenuBoxBL;
        protected Rectangle MenuBoxBR;
        protected Rectangle MenuBoxL;
        protected Rectangle MenuBoxR;
        protected Rectangle MenuBoxT;
        protected Rectangle MenuBoxB;
        protected Rectangle MenuBoxM;

        protected Texture2D BorderTopLeft;
        protected Texture2D BorderTopRight;
        protected Texture2D BorderBottomLeft;
        protected Texture2D BorderBottomRight;
        protected Texture2D BorderLeft;
        protected Texture2D BorderRight;
        protected Texture2D BorderTop;
        protected Texture2D BorderBottom;
        protected Texture2D BorderMiddle;

        protected SpriteFont font;

        protected bool answer = false;
        protected bool enterPressed = false;

        protected int screenWidth;
        protected int screenHeight;

        public override void Open(Game game)
        {
            yes = ScreenManager.Content.Load<Texture2D>("Images/Captions/MainMenu");
            no = ScreenManager.Content.Load<Texture2D>("ScreenTextures/transparent");
            yesSelected = ScreenManager.Content.Load<Texture2D>("Images/Captions/MainMenu");
            noSelected = ScreenManager.Content.Load<Texture2D>("ScreenTextures/transparent");

            font = ScreenManager.Font;

            // Initialize Buttons
            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            BorderTopLeft = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogTopLeft");
            BorderTopRight = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogTopRight");
            BorderBottomLeft = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogBottomLeft");
            BorderBottomRight = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogBottomRight");
            BorderLeft = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogLeft");
            BorderRight = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogRight");
            BorderTop = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogTop");
            BorderBottom = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogBottom");
            BorderMiddle = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogMiddle");

            Rectangle boxRec = new Rectangle(100, 100, 200, 200);
            MakeBorderBox(boxRec, out MenuBoxTL, out MenuBoxT, out MenuBoxTR, out MenuBoxR,
                out MenuBoxBR, out MenuBoxB, out MenuBoxBL, out MenuBoxL, out MenuBoxM);
        }

        public override void Update(GameTime gameTime)
        {
            enterPressed = false;

            // look at input and update button selection
            switch (ScreenManager.Input)
            {
                case InputState.Continue: enterPressed = true; break;
                case InputState.Left: if(answer) answer = false; break;
                case InputState.Right: if (!answer) answer = true; break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch m_sprite = ScreenManager.SpriteBatch;

            m_sprite.Begin();

            // Draw Border
            m_sprite.Draw(BorderTopLeft, MenuBoxTL, Color.White);
            m_sprite.Draw(BorderTopRight, MenuBoxTR, Color.White);
            m_sprite.Draw(BorderBottomLeft, MenuBoxBL, Color.White);
            m_sprite.Draw(BorderBottomRight, MenuBoxBR, Color.White);
            m_sprite.Draw(BorderLeft, MenuBoxL, Color.White);
            m_sprite.Draw(BorderRight, MenuBoxR, Color.White);
            m_sprite.Draw(BorderTop, MenuBoxT, Color.White);
            m_sprite.Draw(BorderBottom, MenuBoxB, Color.White);
            m_sprite.Draw(BorderMiddle, MenuBoxM, Color.White);

            m_sprite.End();
        }
    }
}
