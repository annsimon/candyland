using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Candyland
{
    class SalesmanDialogueScreen : GameScreen
    {
        Texture2D OtherTalkBubble;
        Texture2D OwnTalkBubble;
        Texture2D arrowDown;
        SpriteFont font;

        int screenWidth;
        int screenHeight;

        Rectangle ownDiagBox;
        Rectangle otherDiagBox;

        string otherText;
        string option1, option2, option3, option4;

        int activeIndex = 0;
        int numberOfOptions = 4;

        int[] numberOfLines;

        bool isTimeToAnswer = false;
        bool canScroll = false;
        int lineCapacity;
        bool arrowBlink = false;
        int timePastSinceLastArrowBling;
        int lineDist;
        int offset = 15;

        public override void Open(Game game)
        {
            OtherTalkBubble = ScreenManager.Content.Load<Texture2D>("ScreenTextures/otherTalkBubble");
            OwnTalkBubble = ScreenManager.Content.Load<Texture2D>("ScreenTextures/talkBubbleOwn");
            arrowDown = ScreenManager.Content.Load<Texture2D>("ScreenTextures/arrowDown");
            font = ScreenManager.Font;

            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            numberOfLines = new int[numberOfOptions + 1];

            otherDiagBox = new Rectangle(0, (screenHeight * 2) / 3, screenWidth, screenHeight / 3);
            ownDiagBox = new Rectangle(screenWidth/3, 0, screenWidth * 2/3, screenHeight * 2/3);

            otherText = wrapText(GameConstants.tradesmanGreeting, font, otherDiagBox, 0);
            option1 = wrapText("Reden", font, ownDiagBox, 1);
            option2 = wrapText("Einkaufen", font, ownDiagBox, 2);
            option3 = wrapText("Reisen", font, ownDiagBox, 3);
            option4 = wrapText("Auf Wiedersehen!", font, ownDiagBox, 4);
            // Check if text needs scrolling
            lineDist = font.LineSpacing;
            lineCapacity = (otherDiagBox.Height - offset) / font.LineSpacing;
        }

        public override void Update(GameTime gameTime)
        {
            bool enterPressed = false;

            // look at input and update button selection
            switch (ScreenManager.Input)
            {
                case InputState.Enter: enterPressed = true; break;
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
                    case 0:  break;
                    case 1: this.ScreenState = ScreenState.Hidden;
                        ScreenManager.ActivateNewScreen(new ShopScreen()); break;
                    case 2: ScreenManager.ActivateNewScreen(new TravelScreen()); break;
                    case 3: ScreenManager.ResumeLast(this); break;
                }
                return;
            }

            // Check if text needs scrolling
            if (numberOfLines[0] > lineCapacity)
            {
                canScroll = true;

                // If other person is still talking continue through text by pressing Enter
                if (enterPressed)
                {
                    otherText = removeReadLines(otherText, lineCapacity);
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

            m_sprite.Draw(OtherTalkBubble, otherDiagBox, Color.White);

            m_sprite.DrawString(font, otherText, new Vector2(otherDiagBox.X + offset, otherDiagBox.Y + offset), Color.Black);

            if (isTimeToAnswer)
            {
                m_sprite.Draw(OwnTalkBubble, ownDiagBox, Color.White);

                m_sprite.DrawString(font, option1, new Vector2(leftAlign, topAlign), Color.Black);
                m_sprite.DrawString(font, option2, new Vector2(leftAlign, topAlign + lineDist * numberOfLines[1]), Color.Black);
                m_sprite.DrawString(font, option3, new Vector2(leftAlign, topAlign + lineDist * (numberOfLines[2] + numberOfLines[1])), Color.Black);
                m_sprite.DrawString(font, option4, new Vector2(leftAlign, topAlign + lineDist * (numberOfLines[3] + numberOfLines[2] + numberOfLines[1])), Color.Black);

                // Draw active option in different color
                switch (activeIndex)
                {
                    case 0: m_sprite.DrawString(font, option1, new Vector2(leftAlign, topAlign), Color.Green); break;
                    case 1: m_sprite.DrawString(font, option2, new Vector2(leftAlign, topAlign + lineDist * numberOfLines[1]), Color.Green); break;
                    case 2: m_sprite.DrawString(font, option3, new Vector2(leftAlign, topAlign + lineDist * (numberOfLines[2] + numberOfLines[1])), Color.Green); break;
                    case 3: m_sprite.DrawString(font, option4, new Vector2(leftAlign, topAlign + lineDist * (numberOfLines[3] + numberOfLines[2] + numberOfLines[1])), Color.Green); break;
                }
            }
            // other person is still talking
            else
            {
                if (canScroll)
                {
                    if (arrowBlink)
                    {
                        m_sprite.Draw(arrowDown, new Rectangle(otherDiagBox.Right - 50, otherDiagBox.Bottom - 40, 40, 20), Color.White);
                    }
                    else m_sprite.Draw(arrowDown, new Rectangle(otherDiagBox.Right - 50, otherDiagBox.Bottom - 30, 40, 20), Color.White);
                }
            }
            m_sprite.End();

            // we need the following as spriteBatch.Begin() sets them to None and AlphaBlend
            // which breaks our model rendering
            GraphicsDevice m_graphics = ScreenManager.Game.GraphicsDevice;
            m_graphics.DepthStencilState = DepthStencilState.Default;
            m_graphics.BlendState = BlendState.Opaque;
        }

        /// <summary>
        /// Puts line breaks into a string to make it fit into a given text box
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="textBox"></param>
        /// <returns></returns>
        private String wrapText(String text, SpriteFont font, Rectangle textBox, int index)
        {
            String lineString = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');
            int numOfLines = 1;

            foreach (String word in wordArray)
            {
                float lineWidth = font.MeasureString(lineString + word).Length();
                if (lineWidth + offset > textBox.Width)
                {
                    returnString = returnString + lineString + '\n';
                    numOfLines++;
                    lineString = String.Empty;
                }

                lineString = lineString + word + ' ';
            }
            numberOfLines[index] = numOfLines;
            return returnString + lineString;
        }

        /// <summary>
        /// scrolling
        /// </summary>
        /// <param name="text"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        private String removeReadLines(String text, int capacity)
        {
            String[] lineArray = text.Split('\n');
            String returnString = String.Empty;
            int numOfLines = 0;

            for (int i = capacity; i < lineArray.Length; i++)
            {
                returnString += lineArray[i] + '\n';
                numOfLines++;
            }
            numberOfLines[0] = numOfLines;
            return returnString;
        }
    }
}
