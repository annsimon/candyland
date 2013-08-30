using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Candyland
{
    class SalesmanDialogueScreen : DialogListeningScreen
    {
        Texture2D OwnTalkBubble;

        Rectangle ownDiagBox;

        private string option1, option2, option3, option4;

        private string Greeting = "Hallo mein Freund";
        private string Text = "";
        private string Picture = "Images/DialogImages/DefaultImage";
        private string[] TextArray;
        private string[] GreetingArray;

        private string salesmanID;
        private UpdateInfo m_updateInfo;
        private int chocosCollected;

        int activeIndex = 0;
        int numberOfOptions = 4;

        bool isGreeting = true;
        bool isTimeToAnswer = false;

        public SalesmanDialogueScreen(string text, string saleID, UpdateInfo info, int chocoCount, string picture = "Images/DialogImages/DefaultImage")
        {
            this.Text = text;
            this.Picture = picture;
            salesmanID = saleID;
            m_updateInfo = info;
            chocosCollected = chocoCount;
        }

        public override void Open(Game game)
        {
            base.Open(game);

            OwnTalkBubble = ScreenManager.Content.Load<Texture2D>("ScreenTextures/talkBubbleOwn");

            ownDiagBox = new Rectangle(screenWidth / 3, 0, screenWidth * 2 / 3, screenHeight * 2 / 3);

            option1 = "Reden";
            option2 = "Einkaufen";
            option3 = "Reisen";
            option4 = "Auf Wiedersehen!";

            //lineCapacity = (TextBox.Height - offset) / font.LineSpacing;

            GreetingArray = wrapText(Greeting, font, TextBox, lineCapacity);
            TextArray = wrapText(Text, font, TextBox, lineCapacity);
        }

        public override void Update(GameTime gameTime)
        {
            bool enterPressed = false;

            // look at input and update button selection
            switch (ScreenManager.Input)
            {
                case InputState.Continue: enterPressed = true; break;
                case InputState.Up: activeIndex--; break;
                case InputState.Down: activeIndex++; break;
            }
            if (activeIndex >= numberOfOptions) activeIndex = 0;
            if (activeIndex < 0) activeIndex = numberOfOptions - 1;

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
                m_sprite.Draw(OwnTalkBubble, ownDiagBox, Color.White);

                m_sprite.DrawString(font, option1, new Vector2(leftAlign, topAlign), Color.Black);
                m_sprite.DrawString(font, option2, new Vector2(leftAlign, topAlign + lineDist ), Color.Black);
                m_sprite.DrawString(font, option3, new Vector2(leftAlign, topAlign + lineDist * 2), Color.Black);
                m_sprite.DrawString(font, option4, new Vector2(leftAlign, topAlign + lineDist * 3), Color.Black);

                // Draw active option in different color
                switch (activeIndex)
                {
                    case 0: m_sprite.DrawString(font, option1, new Vector2(leftAlign, topAlign), Color.Green); break;
                    case 1: m_sprite.DrawString(font, option2, new Vector2(leftAlign, topAlign + lineDist), Color.Green); break;
                    case 2: m_sprite.DrawString(font, option3, new Vector2(leftAlign, topAlign + lineDist * 2), Color.Green); break;
                    case 3: m_sprite.DrawString(font, option4, new Vector2(leftAlign, topAlign + lineDist * 3), Color.Green); break;
                }
            }
            //other person is still talking
            else
            {
                if (canScroll)
                {
                    if (arrowBlink)
                    {
                        m_sprite.Draw(arrowDown, new Rectangle(DiagBoxBR.Right - 35, DiagBoxBR.Bottom - 30, 30, 15), Color.White);
                    }
                    else m_sprite.Draw(arrowDown, new Rectangle(DiagBoxBR.Right - 35, DiagBoxBR.Bottom - 25, 30, 15), Color.White);
                }
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
