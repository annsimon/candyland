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
        protected Texture2D TalkBubble;
        protected Texture2D arrowDown;
        protected Texture2D talkingNPC;
        protected SpriteFont font;

        protected int screenWidth;
        protected int screenHeight;

        protected Rectangle DiagBox;
        protected Rectangle pictureNPC;
        protected Rectangle TextBox;

        private string Text = "";
        private string Picture = "testBonus";

        protected int numberOfLines;

        protected bool canScroll = false;
        protected int lineCapacity;
        protected bool arrowBlink = false;
        protected int timePastSinceLastArrowBling;
        protected int lineDist;
        protected int offset = 15;

        public DialogListeningScreen() {}

        public DialogListeningScreen(string text, string picture)
        {
            this.Text = text;
            this.Picture = picture;
        }

        public override void Open(Game game)
        {
            TalkBubble = ScreenManager.Content.Load<Texture2D>("ScreenTextures/otherTalkBubble");
            arrowDown = ScreenManager.Content.Load<Texture2D>("ScreenTextures/arrowDown");
            talkingNPC = ScreenManager.Content.Load<Texture2D>("ScreenTextures/" + Picture);
            font = ScreenManager.Font;
            lineDist = font.LineSpacing;

            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            numberOfLines = 1;

            DiagBox = new Rectangle(0, (screenHeight * 2) / 3, screenWidth, screenHeight / 3);
            pictureNPC = new Rectangle(DiagBox.X + offset, DiagBox.Y + offset, 3 * lineDist, 3 * lineDist);
            TextBox = new Rectangle(pictureNPC.Right + offset, pictureNPC.Top + lineDist / 4, screenWidth - pictureNPC.Right - 2 * offset, pictureNPC.Height + offset);


            Text = wrapText(Text, font, DiagBox);
            // Check if text needs scrolling
            lineCapacity = (TextBox.Height - offset) / font.LineSpacing;
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
            m_sprite.Draw(talkingNPC, pictureNPC, Color.White);

            m_sprite.DrawString(font, Text, new Vector2(TextBox.X, TextBox.Y), Color.Black);

            if (canScroll)
            {
                if (arrowBlink)
                {
                    m_sprite.Draw(arrowDown, new Rectangle(DiagBox.Right - 35, DiagBox.Bottom - 30, 30, 15), Color.White);
                }
                else m_sprite.Draw(arrowDown, new Rectangle(DiagBox.Right - 35, DiagBox.Bottom - 25, 30, 15), Color.White);
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
        protected String wrapText(String text, SpriteFont font, Rectangle textBox)
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
        protected String removeReadLines(String text, int capacity)
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
