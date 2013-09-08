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

        protected string question = "?";

        protected Rectangle yesBox;
        protected Rectangle noBox;

        protected Rectangle boxRec;

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

        public override void Open(Game game, AssetManager assets)
        {
            yes = assets.yes;
            no = assets.no;
            yesSelected = assets.yesActive;
            noSelected = assets.noActive;

            font = ScreenManager.Font;

            // Initialize Buttons
            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            BorderTopLeft = assets.dialogTL;
            BorderTopRight = assets.dialogTR;
            BorderBottomLeft = assets.dialogBL;
            BorderBottomRight = assets.dialogBR;
            BorderLeft = assets.dialogL;
            BorderRight = assets.dialogR;
            BorderTop = assets.dialogT;
            BorderBottom = assets.dialogB;
            BorderMiddle = assets.dialogC;

            int buttonOffset = 10;
            boxRec = new Rectangle((screenWidth - yes.Width * 2) / 2 - buttonOffset,
                (screenHeight - yes.Height - font.LineSpacing * 3) / 2 - buttonOffset / 2,
                yes.Width * 2 + 2 * buttonOffset, yes.Height + font.LineSpacing * 3 + buttonOffset);
            MakeBorderBox(boxRec, out MenuBoxTL, out MenuBoxT, out MenuBoxTR, out MenuBoxR,
                out MenuBoxBR, out MenuBoxB, out MenuBoxBL, out MenuBoxL, out MenuBoxM);

            yesBox = new Rectangle(boxRec.Left + yes.Width + buttonOffset, boxRec.Top + font.LineSpacing * 3, yes.Width, yes.Height);
            noBox = new Rectangle(boxRec.Left + buttonOffset, boxRec.Top + font.LineSpacing * 3, no.Width, no.Height);
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

            if (enterPressed && !answer) ScreenManager.ResumeLast(this);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch m_sprite = ScreenManager.SpriteBatch;

            m_sprite.Begin();

            Color white = Color.White;

            // Draw Border
            m_sprite.Draw(BorderTopLeft, MenuBoxTL, white);
            m_sprite.Draw(BorderTopRight, MenuBoxTR, white);
            m_sprite.Draw(BorderBottomLeft, MenuBoxBL, white);
            m_sprite.Draw(BorderBottomRight, MenuBoxBR, white);
            m_sprite.Draw(BorderLeft, MenuBoxL, white);
            m_sprite.Draw(BorderRight, MenuBoxR, white);
            m_sprite.Draw(BorderTop, MenuBoxT, white);
            m_sprite.Draw(BorderBottom, MenuBoxB, white);
            m_sprite.Draw(BorderMiddle, MenuBoxM, white);

            m_sprite.DrawString(font, question,
                new Vector2(boxRec.Left + (boxRec.Width - (int)font.MeasureString(question).X) / 2,
                boxRec.Top + font.LineSpacing), Color.Black);

            if (answer)
            {
                m_sprite.Draw(yesSelected, yesBox, white);
                m_sprite.Draw(no, noBox, white);
            }
            else
            {
                m_sprite.Draw(yes, yesBox, white);
                m_sprite.Draw(noSelected, noBox, white);
            }

            m_sprite.End();

            // we need the following as spriteBatch.Begin() sets them to None and AlphaBlend
            // which breaks our model rendering
            GraphicsDevice m_graphics = ScreenManager.Game.GraphicsDevice;
            m_graphics.DepthStencilState = DepthStencilState.Default;
            m_graphics.BlendState = BlendState.Opaque;
        }
    }
}
