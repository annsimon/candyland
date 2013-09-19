using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Candyland
{
    class OptionsScreen : GameScreen
    {
        Texture2D caption;

        protected SpriteFont font;
        protected SpriteFont Font;

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

        protected Rectangle TextBox;

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
            isFullscreen = true;

            caption = ScreenManager.Content.Load<Texture2D>("Images/Captions/Options");
            BorderTopLeft = assets.dialogTL;
            BorderTopRight = assets.dialogTR;
            BorderBottomLeft = assets.dialogBL;
            BorderBottomRight = assets.dialogBR;
            BorderLeft = assets.dialogL;
            BorderRight = assets.dialogR;
            BorderTop = assets.dialogT;
            BorderBottom = assets.dialogB;
            BorderMiddle = assets.dialogC;

            font = assets.mainText;
            Font = assets.mainTextFullscreen;

            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            int offset = 5;

            int MenuBoxWidth = ScreenManager.PrefScreenWidth - 2 * offset;
            int MenuBoxHeight = ScreenManager.PrefScreenHeight - 2 * offset;

            MakeBorderBox(new Rectangle((screenWidth - MenuBoxWidth) / 2, (screenHeight - MenuBoxHeight) / 2, MenuBoxWidth, MenuBoxHeight),
                out MenuBoxTL, out MenuBoxT, out MenuBoxTR, out MenuBoxR,
                out MenuBoxBR, out MenuBoxB, out MenuBoxBL, out MenuBoxL, out MenuBoxM);

            TextBox = new Rectangle(MenuBoxL.Left + 240, MenuBoxT.Top + 60, 600, 280);
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
            m_sprite.Draw(caption, new Rectangle(MenuBoxL.Left + 5, MenuBoxT.Top + 5, (int)(caption.Width * 0.8f), (int)(caption.Height * 0.8f)), Color.White);

            int topAlignTitles = MenuBoxT.Bottom + 140;
            m_sprite.DrawString(Font, "Steuerung", new Vector2(MenuBoxL.Right - 10, topAlignTitles), Color.Black);
            m_sprite.DrawString(Font, "Grafik", new Vector2(MenuBoxL.Right - 10, topAlignTitles + Font.LineSpacing), Color.Black);
            m_sprite.DrawString(Font, "Audio", new Vector2(MenuBoxL.Right - 10, topAlignTitles + 2 * Font.LineSpacing), Color.Black);

            m_sprite.DrawString(font, GameConstants.controlDescription1 , new Vector2(TextBox.X, TextBox.Y), Color.Black);
            m_sprite.DrawString(font, GameConstants.controlDescription2, new Vector2(TextBox.X + 345, TextBox.Y), Color.Black);

            ScreenManager.SpriteBatch.End();
        }
    }
}
