using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Candyland
{
    class DialogListeningScreen : GameScreen
    {
        Texture2D TalkBubble;
        Texture2D arrowDown;
        SpriteFont font;

        int screenWidth;
        int screenHeight;

        Rectangle DiagBox;

        string Text = "";

        int numberOfLines;

        bool canScroll = false;
        int lineCapacity;
        bool arrowBlink = false;
        int timePastSinceLastArrowBling;
        int lineDist;
        int offset = 15;
        private string p;

        public DialogListeningScreen(string text)
        {
            this.Text = text;
        }

        public override void Open(Game game)
        {
            TalkBubble = ScreenManager.Content.Load<Texture2D>("ScreenTextures/otherTalkBubble");
            arrowDown = ScreenManager.Content.Load<Texture2D>("ScreenTextures/arrowDown");
            font = ScreenManager.Font;

            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            numberOfLines = 1;

            DiagBox = new Rectangle(0, (screenHeight * 2) / 3, screenWidth, screenHeight / 3);

            Text = wrapText(Text, font, DiagBox);
            // Check if text needs scrolling
            lineDist = font.LineSpacing;
            lineCapacity = (DiagBox.Height - offset) / font.LineSpacing;
        }

        public override void Update(GameTime gameTime)
        {
            bool enterPressed = ScreenManager.Input.Equals(InputState.Enter);

            // Check if text needs scrolling
            if (numberOfLines > lineCapacity)
            {
                canScroll = true;

                // If other person is still talking continue through text by pressing Enter
                if (enterPressed)
                {
                    Text = removeReadLines(Text, lineCapacity);
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
                if (enterPressed)
                {
                    ScreenManager.ResumeLast(this);
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            int leftAlign = DiagBox.X + DiagBox.Width / 5;
            int topAlign = DiagBox.Y + DiagBox.Height / 5; 

            SpriteBatch m_sprite = ScreenManager.SpriteBatch;

            m_sprite.Begin();

            m_sprite.Draw(TalkBubble, DiagBox, Color.White);

            m_sprite.DrawString(font, Text, new Vector2(DiagBox.X + offset, DiagBox.Y + offset), Color.Black);

            if (canScroll)
            {
                if (arrowBlink)
                {
                    m_sprite.Draw(arrowDown, new Rectangle(DiagBox.Right - 50, DiagBox.Bottom - 40, 40, 20), Color.White);
                }
                else m_sprite.Draw(arrowDown, new Rectangle(DiagBox.Right - 50, DiagBox.Bottom - 30, 40, 20), Color.White);
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
        private String wrapText(String text, SpriteFont font, Rectangle textBox)
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
            numberOfLines = numOfLines;
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
            numberOfLines = numOfLines;
            return returnString;
        }
    }
}
