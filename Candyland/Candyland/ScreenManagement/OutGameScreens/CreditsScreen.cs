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
        Texture2D logo;

        protected SpriteFont font;
        protected SpriteFont fontRegular;
        protected SpriteFont fontSmall;

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

        public CreditsScreen()
        {
            this.isFullscreen = true;
        }

        public override void Open(Game game, AssetManager assets)
        {
            caption = assets.captionCredits;
            logo = assets.acagamicsLogo;
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

            font = assets.mainText;
            fontRegular = assets.mainRegular;
            fontSmall = assets.smallText;

            int offset = 5;

            int MenuBoxWidth = ScreenManager.PrefScreenWidth - 2 * offset;
            int MenuBoxHeight = ScreenManager.PrefScreenHeight - 2 * offset;

            MakeBorderBox(new Rectangle((screenWidth - MenuBoxWidth) / 2, (screenHeight - MenuBoxHeight) / 2, MenuBoxWidth, MenuBoxHeight),
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

                m_sprite.Draw(caption, new Rectangle(MenuBoxL.Left + 5, MenuBoxT.Top + 5, (int)(caption.Width * 0.8f), (int)(caption.Height * 0.8f)), Color.White);

            DrawCredits(screenWidth, m_sprite);
                
            // Go Back Option
            m_sprite.DrawString(font, "Zurück mit\n'Escape'", new Vector2(20, screenHeight - font.LineSpacing * 3), Color.Black);

            ScreenManager.SpriteBatch.End();
        }

        private void DrawCredits(int screenWidth, SpriteBatch m_sprite)
        {
            Color textColor = Color.Black;
            int lineSpace = font.LineSpacing;
            int lineSpaceSmall = fontRegular.LineSpacing - 3;

            int topAlignProg = MenuBoxT.Top + 70;
            int topAlign3D = topAlignProg + 2 * lineSpace;
            int topAlign2D = topAlign3D + 2 * lineSpace;
            int topAlignTest = topAlign2D + 2 * lineSpace;
            int topAlignSpecial = topAlignTest + 3 * lineSpace;

            string headingProg = "Programming";
            string heading3D = "3D Art";
            string heading2D = "2D Art";
            string headingTest = "Testing";
            string headingSpecial = "Special Thanks";
            string programmer = "Björn Golla, Svenja Handreck, Sebastian Rohde, Anne-Lena Simon";
            string art3D = "Sebastian Rohde";
            string art2D = "Svenja Handreck, Anne-Lena Simon";
            string tester = "Simone Bexten, Sebastian Heerwald, Graeme Fitzapack,";
            string tester2 = "Jin, Sebastian Laubmeyer and many more";
            string specialHelp = "Johannes Jendersie, Sebastian Laubmeyer";

                m_sprite.DrawString(font, headingProg,
                    new Vector2((int)(screenWidth / 2 - (font.MeasureString(headingProg).X / 2)),
                        topAlignProg), textColor);
                m_sprite.DrawString(font, heading3D,
                    new Vector2((int)(screenWidth / 2 - (font.MeasureString(heading3D).X / 2)),
                        topAlign3D), textColor);
                m_sprite.DrawString(font, heading2D,
                    new Vector2((int)(screenWidth / 2 - (font.MeasureString(heading2D).X / 2)),
                        topAlign2D), textColor);
                m_sprite.DrawString(font, headingTest,
                    new Vector2((int)(screenWidth / 2 - (font.MeasureString(headingTest).X / 2)),
                        topAlignTest), textColor);
                m_sprite.DrawString(font, headingSpecial,
                    new Vector2((int)(screenWidth / 2 - (font.MeasureString(headingSpecial).X /2)),
                        topAlignSpecial), textColor);

                m_sprite.DrawString(fontRegular, programmer,
                    new Vector2((int)(screenWidth / 2 - (fontRegular.MeasureString(programmer).X / 2)),
                        topAlignProg + lineSpaceSmall), textColor);
                m_sprite.DrawString(fontRegular, art3D,
                    new Vector2((int)(screenWidth / 2 - (fontRegular.MeasureString(art3D).X / 2)),
                        topAlign3D + lineSpaceSmall), textColor);
                m_sprite.DrawString(fontRegular, art2D,
                    new Vector2((int)(screenWidth / 2 - (fontRegular.MeasureString(art2D).X / 2)),
                        topAlign2D + lineSpaceSmall), textColor);
                m_sprite.DrawString(fontRegular, tester,
                    new Vector2((int)(screenWidth / 2 - (fontRegular.MeasureString(tester).X / 2)),
                        topAlignTest + lineSpaceSmall), textColor);
                m_sprite.DrawString(fontRegular, tester2,
                    new Vector2((int)(screenWidth / 2 - (fontRegular.MeasureString(tester2).X / 2)),
                        topAlignTest + 2 * lineSpaceSmall), textColor);
                m_sprite.DrawString(fontRegular, specialHelp,
                    new Vector2((int)(screenWidth / 2 - (fontRegular.MeasureString(specialHelp).X / 2)),
                        topAlignSpecial + lineSpaceSmall), textColor);

            // Draw Acagamics Logo
            int LogoSizeX = logo.Width;
            int LogoSizeY = logo.Height;
            Rectangle RecLogo = new Rectangle(MenuBoxR.Left - LogoSizeX + 5, MenuBoxB.Top - LogoSizeY, LogoSizeX, LogoSizeY);
            m_sprite.Draw(logo, RecLogo, Color.White);
            m_sprite.DrawString(fontSmall, "supported by\nAcagamics e.V.",
                new Vector2(MenuBoxR.Left - 70, MenuBoxB.Top), textColor);
        }
    }
}
