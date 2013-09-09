using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Candyland
{
    class SalesmanDialogueScreen : DialogListeningScreen
    {
        Texture2D buttonTexture;

        Rectangle ownDiagBox;

        private string option1, option2, option3, option4;

        private string Greeting = "Hallo mein Freund";
        private string[] GreetingArray;

        private string salesmanID;
        private UpdateInfo m_updateInfo;
        private int chocosCollected;

        int activeIndex = 0;
        int numberOfOptions = 4;

        bool isGreeting = true;
        bool isTimeToAnswer = false;

        protected Rectangle AnswerBoxTL;
        protected Rectangle AnswerBoxTR;
        protected Rectangle AnswerBoxBL;
        protected Rectangle AnswerBoxBR;
        protected Rectangle AnswerBoxL;
        protected Rectangle AnswerBoxR;
        protected Rectangle AnswerBoxT;
        protected Rectangle AnswerBoxB;
        protected Rectangle AnswerBoxM;

        // Buttons
        int buttonWidth;
        int buttonHeight;
        int leftAlign = 525;
        int topAlign = 25;
        Rectangle selectedButton;

        public SalesmanDialogueScreen(string text, string saleID, UpdateInfo info, int chocoCount, string picture)
        {
            this.Text = text;
            this.Picture = picture;
            salesmanID = saleID;
            m_updateInfo = info;
            chocosCollected = chocoCount;
            Greeting = GameConstants.tradesmanGreeting;
        }

        public override void Open(Game game, AssetManager assets)
        {
            base.Open(game, assets);

            buttonTexture = assets.menuSelection;
            talkingNPC = ScreenManager.Content.Load<Texture2D>(Picture);

            ownDiagBox = new Rectangle(480, 20, 300, 200);

            MakeBorderBox(ownDiagBox,
            out AnswerBoxTL, out AnswerBoxT, out AnswerBoxTR, out AnswerBoxR,
            out AnswerBoxBR, out AnswerBoxB, out AnswerBoxBL, out AnswerBoxL, out AnswerBoxM);

            option1 = "Reden";
            option2 = "Einkaufen";
            option3 = "Reisen";
            option4 = "Auf Wiedersehen!";

            //lineCapacity = (TextBox.Height - offset) / font.LineSpacing;

            GreetingArray = wrapText(Greeting, font, TextBox, lineCapacity);
            TextArray = wrapText(Text, font, TextBox, lineCapacity);

            buttonWidth = (int)font.MeasureString("Auf Wiedersehen!").X + 20;
            buttonHeight = font.LineSpacing;

            selectedButton = new Rectangle(leftAlign, topAlign, buttonWidth, buttonHeight);
        }

        public override void Update(GameTime gameTime)
        {
            bool enterPressed = false;

            // look at input and update button selection
            if (isTimeToAnswer)
            {
                switch (ScreenManager.Input)
                {
                    case InputState.Continue: enterPressed = true; break;
                    case InputState.Up: activeIndex--; break;
                    case InputState.Down: activeIndex++; break;
                }
                if (activeIndex >= numberOfOptions) activeIndex = 0;
                if (activeIndex < 0) activeIndex = numberOfOptions - 1;
            }
            else
            {
                if ((ScreenManager.Input.Equals(InputState.Continue) || ScreenManager.Input.Equals(InputState.Down)) && Keyboard.GetState().IsKeyUp(Keys.Space))
                    enterPressed = true;
            }

            // Selected Button confirmed by pressing Enter
            if (enterPressed && isTimeToAnswer)
            {
                switch (activeIndex)
                {
                    case 0: scrollIndex = 0; isGreeting = false; isTimeToAnswer = false; break;
                    case 1: this.ScreenState = ScreenState.Hidden;
                        ScreenManager.ActivateNewScreen(new ShopScreen(salesmanID,m_updateInfo, chocosCollected)); break;
                    case 2: ScreenManager.ActivateNewScreen(new TravelScreen(salesmanID, m_updateInfo, this)); break;
                    case 3: ScreenManager.ResumeLast(this); break;
                }
                return;
            }

            if (isGreeting)
            {
                if(enterPressed)
                    isTimeToAnswer = true;
                return;
            }

            // Check if text needs scrolling
            if (!isTimeToAnswer && scrollIndex < (TextArray.Length-1) && TextArray[scrollIndex+1]!= null)
            {
                canScroll = true;
                // If other person is still talking continue through text by pressing Enter
                if (enterPressed)
                {
                    scrollIndex++;
                }

                timePastSinceLastArrowBling += gameTime.ElapsedGameTime.Milliseconds;

                if (timePastSinceLastArrowBling > 400)
                {
                    if (timePastSinceLastArrowBling > 800)
                    {
                        arrowBlink = false;
                        timePastSinceLastArrowBling = 0;
                    }
                    else
                        arrowBlink = true;
                }
            }
            else
            {
                canScroll = false;
                isTimeToAnswer = true;
            }

        }

        public override void Draw(GameTime gameTime)
        {
            int leftAlign = ownDiagBox.X + ownDiagBox.Width / 5;
            int topAlign = ownDiagBox.Y + ownDiagBox.Height / 5; 

            SpriteBatch m_sprite = ScreenManager.SpriteBatch;

            m_sprite.Begin();

            m_sprite.Draw(talkBubbleTopLeft, DiagBoxTL, Color.White);
            m_sprite.Draw(talkBubbleTopRight, DiagBoxTR, Color.White);
            m_sprite.Draw(talkBubbleBottomLeft, DiagBoxBL, Color.White);
            m_sprite.Draw(talkBubbleBottomRight, DiagBoxBR, Color.White);
            m_sprite.Draw(talkBubbleLeft, DiagBoxL, Color.White);
            m_sprite.Draw(talkBubbleRight, DiagBoxR, Color.White);
            m_sprite.Draw(talkBubbleTop, DiagBoxT, Color.White);
            m_sprite.Draw(talkBubbleBottom, DiagBoxB, Color.White);
            m_sprite.Draw(talkBubbleMiddle, DiagBoxM, Color.White);

            m_sprite.Draw(talkingNPC, pictureNPC, Color.White);

            if(isGreeting)
                m_sprite.DrawString(font, GreetingArray[scrollIndex], new Vector2(TextBox.X, TextBox.Y), Color.Black);
            else
                m_sprite.DrawString(font, TextArray[scrollIndex], new Vector2(TextBox.X, TextBox.Y), Color.Black);

            if (isTimeToAnswer)
            {
                m_sprite.Draw(talkBubbleTopLeft, AnswerBoxTL, Color.White);
                m_sprite.Draw(talkBubbleTopRight, AnswerBoxTR, Color.White);
                m_sprite.Draw(talkBubbleBottomLeft, AnswerBoxBL, Color.White);
                m_sprite.Draw(talkBubbleBottomRight, AnswerBoxBR, Color.White);
                m_sprite.Draw(talkBubbleLeft, AnswerBoxL, Color.White);
                m_sprite.Draw(talkBubbleRight, AnswerBoxR, Color.White);
                m_sprite.Draw(talkBubbleTop, AnswerBoxT, Color.White);
                m_sprite.Draw(talkBubbleBottom, AnswerBoxB, Color.White);
                m_sprite.Draw(talkBubbleMiddle, AnswerBoxM, Color.White);

                m_sprite.DrawString(font, option1, new Vector2(leftAlign, topAlign), Color.Black);
                m_sprite.DrawString(font, option2, new Vector2(leftAlign, topAlign + lineDist ), Color.Black);
                m_sprite.DrawString(font, option3, new Vector2(leftAlign, topAlign + lineDist * 2), Color.Black);
                m_sprite.DrawString(font, option4, new Vector2(leftAlign, topAlign + lineDist * 3), Color.Black);

                selectedButton.Y = topAlign + (buttonHeight * activeIndex);
                m_sprite.Draw(buttonTexture, selectedButton, Color.White);
            }
            //other person is still talking
            else
            {
                if (arrowBlink)
                {
                    m_sprite.Draw(arrowDown, new Rectangle(DiagBoxBR.Right - 35, DiagBoxBR.Bottom - 30, 30, 15), Color.White);
                }
                else m_sprite.Draw(arrowDown, new Rectangle(DiagBoxBR.Right - 35, DiagBoxBR.Bottom - 25, 30, 15), Color.White);
            }
            m_sprite.End();

            //// we need the following as spriteBatch.Begin() sets them to None and AlphaBlend
            //// which breaks our model rendering
            GraphicsDevice m_graphics = ScreenManager.Game.GraphicsDevice;
            m_graphics.DepthStencilState = DepthStencilState.Default;
            m_graphics.BlendState = BlendState.Opaque;
        }
    }
}
