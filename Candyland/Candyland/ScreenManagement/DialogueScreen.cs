using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class DialogueScreen : GameScreen
    {
        Texture2D OtherTalkBubble;
        Texture2D OwnTalkBubble;
        SpriteFont font;

        int screenWidth;
        int screenHeight;

        Rectangle ownDiagBox;
        Rectangle otherDiagBox;

        string otherText;
        string option1, option2, option3, option4;

        int activeIndex = 0;
        int numberOfOptions = 4;

        public override void Open(Game game)
        {
            OtherTalkBubble = ScreenManager.Content.Load<Texture2D>("ScreenTextures/otherTalkBubble");
            OwnTalkBubble = ScreenManager.Content.Load<Texture2D>("ScreenTextures/talkBubbleOwn");
            font = ScreenManager.Font;

            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            otherDiagBox = new Rectangle(0, (screenHeight * 2) / 3, screenWidth, screenHeight / 3);
            ownDiagBox = new Rectangle(screenWidth/3, 0, screenWidth * 2/3, screenHeight * 2/3); 
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
            if (enterPressed)
            {
                switch (activeIndex)
                {
                    case 0:  break;
                    case 1: this.ScreenState = ScreenState.Hidden;
                        ScreenManager.ActivateNewScreen(new ShopScreen()); break;
                    case 2: ScreenManager.ActivateNewScreen(new TravelScreen()); break;
                    case 3: ScreenManager.ResumeLast(this); break;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            int lineDist = font.LineSpacing;
            int offset = 10;
            int leftAlign = ownDiagBox.X + ownDiagBox.Width / 5;
            int topAlign = ownDiagBox.Y + ownDiagBox.Height / 5; 

            SpriteBatch m_sprite = ScreenManager.SpriteBatch;

            m_sprite.Begin();

            m_sprite.Draw(OwnTalkBubble, ownDiagBox, Color.White);
            m_sprite.Draw(OtherTalkBubble, otherDiagBox, Color.White);

            m_sprite.DrawString(font, wrapText(GameConstants.tradesmanGreeting, font, otherDiagBox), new Vector2(otherDiagBox.X + offset, otherDiagBox.Y + offset), Color.Black);

            m_sprite.DrawString(font, "Reden", new Vector2(leftAlign, topAlign), Color.Black);
            m_sprite.DrawString(font, "Einkaufen", new Vector2(leftAlign, topAlign + lineDist), Color.Black);
            m_sprite.DrawString(font, "Reisen", new Vector2(leftAlign, topAlign + 2 * lineDist), Color.Black);
            m_sprite.DrawString(font, "Auf Wiedersehen!", new Vector2(leftAlign, topAlign + 3 * lineDist), Color.Black);

            // Draw active option in different color
            switch (activeIndex)
            {
                case 0: m_sprite.DrawString(font, "Reden", new Vector2(leftAlign, topAlign), Color.Green); break;
                case 1: m_sprite.DrawString(font, "Einkaufen", new Vector2(leftAlign, topAlign + lineDist), Color.Green); break;
                case 2: m_sprite.DrawString(font, "Reisen", new Vector2(leftAlign, topAlign + 2 * lineDist), Color.Green); break;
                case 3: m_sprite.DrawString(font, "Auf Wiedersehen!", new Vector2(leftAlign, topAlign + 3 * lineDist), Color.Green); break;
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

            foreach (String word in wordArray)
            {
                float lineWidth = font.MeasureString(lineString + word).Length();
                if (lineWidth > textBox.Width)
                {
                    returnString = returnString + lineString + '\n';
                    lineString = String.Empty;
                }

                lineString = lineString + word + ' ';
            }
            return returnString + lineString;
        }
    }
}
