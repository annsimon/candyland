using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class BonusScreen : GameScreen
    {
        Texture2D caption;

        protected SpriteFont font;

        int screenWidth;
        int screenHeight;

        // Border
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

        public override void Open(Game game)
        {
            this.isFullscreen = true;

            caption = ScreenManager.Content.Load<Texture2D>("Images/Captions/Bonus");
            BorderTopLeft = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogTopLeft");
            BorderTopRight = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogTopRight");
            BorderBottomLeft = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogBottomLeft");
            BorderBottomRight = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogBottomRight");
            BorderLeft = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogLeft");
            BorderRight = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogRight");
            BorderTop = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogTop");
            BorderBottom = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogBottom");
            BorderMiddle = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogMiddle");

            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            int offsetX = 5;
            int offsetY = 5;

            MenuBoxTL = new Rectangle(0 + offsetX,
                                      offsetY,
                                      42, 49);
            MenuBoxTR = new Rectangle(screenWidth - offsetX - 42,
                                      offsetY,
                                      42, 49);
            MenuBoxBL = new Rectangle(0 + offsetX,
                                      screenHeight - offsetY - 49,
                                      42, 49);
            MenuBoxBR = new Rectangle(screenWidth - offsetX - 42,
                                      screenHeight - offsetY - 49,
                                      42, 49);
            MenuBoxL = new Rectangle(0 + offsetX,
                                     offsetY + 49,
                                     42, (screenHeight - offsetY - 49) - (offsetY + 49));
            MenuBoxR = new Rectangle(screenWidth - offsetX - 42,
                                     offsetY + 49,
                                     42, (screenHeight - offsetY - 49) - (offsetY + 49));
            MenuBoxT = new Rectangle(0 + offsetX + 42,
                                      offsetY,
                                      screenWidth - 84 - 2 * offsetX, 49);
            MenuBoxB = new Rectangle(0 + offsetX + 42,
                                     screenHeight - offsetY - 49,
                                     screenWidth - 84 - 2 * offsetX, 49);
            MenuBoxM = new Rectangle(0 + offsetX + 42,
                                     offsetY + 49,
                                     screenWidth - 84 - 2 * offsetX, screenHeight - 2 * offsetY - 96);
        }

        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.Input.Equals(InputState.Back))
            {
                ScreenManager.ResumeLast(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            int screenWidth = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            int screenHeight = ScreenManager.Game.GraphicsDevice.Viewport.Height;

            ScreenManager.GraphicsDevice.Clear(GameConstants.BackgroundColorMenu);

            SpriteBatch m_sprite = ScreenManager.SpriteBatch;

            m_sprite.Begin();

            // Draw Boarder
                m_sprite.Draw(BorderTopLeft, MenuBoxTL, Color.White);
                m_sprite.Draw(BorderTopRight, MenuBoxTR, Color.White);
                m_sprite.Draw(BorderBottomLeft, MenuBoxBL, Color.White);
                m_sprite.Draw(BorderBottomRight, MenuBoxBR, Color.White);
                m_sprite.Draw(BorderLeft, MenuBoxL, Color.White);
                m_sprite.Draw(BorderRight, MenuBoxR, Color.White);
                m_sprite.Draw(BorderTop, MenuBoxT, Color.White);
                m_sprite.Draw(BorderBottom, MenuBoxB, Color.White);
                m_sprite.Draw(BorderMiddle, MenuBoxM, Color.White);
                if (ScreenManager.isFullscreen) m_sprite.Draw(caption, new Rectangle(MenuBoxL.Left + 5, MenuBoxT.Top + 5, caption.Width, caption.Height), Color.White);
                else m_sprite.Draw(caption, new Rectangle(MenuBoxL.Left + 5, MenuBoxT.Top + 5, (int)(caption.Width * 0.8f), (int)(caption.Height * 0.8f)), Color.White);

            ScreenManager.SpriteBatch.End();
        }
    }
}
