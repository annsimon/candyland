using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class CreditsScreen : GameScreen
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

        public override void Open(Game game, AssetManager assets)
        {
            this.isFullscreen = true;

            caption = assets.captionCredits;
            BorderTopLeft = assets.dialogTL;
            BorderTopRight = assets.dialogTR;
            BorderBottomLeft = assets.dialogBL;
            BorderBottomRight = assets.dialogBR;
            BorderLeft = assets.dialogL;
            BorderRight = assets.dialogR;
            BorderTop = assets.dialogT;
            BorderBottom = assets.dialogB;
            BorderMiddle = assets.dialogC;

            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            font = ScreenManager.Font;

            int offsetX = 5;
            int offsetY = 5;

            MakeBorderBox(new Rectangle(offsetX, offsetY, screenWidth - 2 * offsetX, screenHeight - 2 * offsetY),
                out MenuBoxTL, out MenuBoxT, out MenuBoxTR, out MenuBoxR,
                out MenuBoxBR, out MenuBoxB, out MenuBoxBL, out MenuBoxL, out MenuBoxM);
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
            if (ScreenManager.isFullscreen) m_sprite.Draw(caption, new Rectangle(MenuBoxL.Left + 5, MenuBoxT.Top + 5, caption.Width, caption.Height), Color.White);
            else m_sprite.Draw(caption, new Rectangle(MenuBoxL.Left + 5, MenuBoxT.Top + 5, (int)(caption.Width * 0.8f), (int)(caption.Height * 0.8f)), Color.White);

            m_sprite.DrawString(font, "Zurück mit\n'Escape'", new Vector2(20, screenHeight - font.LineSpacing * 3), Color.Black);

            ScreenManager.SpriteBatch.End();
        }
    }
}
