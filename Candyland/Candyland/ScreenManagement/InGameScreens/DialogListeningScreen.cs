using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Candyland
{
    class DialogListeningScreen : GameScreen
    {
        #region fields

        protected Texture2D talkBubbleTopLeft;
        protected Texture2D talkBubbleTopRight;
        protected Texture2D talkBubbleBottomLeft;
        protected Texture2D talkBubbleBottomRight;
        protected Texture2D talkBubbleLeft;
        protected Texture2D talkBubbleRight;
        protected Texture2D talkBubbleTop;
        protected Texture2D talkBubbleBottom;
        protected Texture2D talkBubbleMiddle;
        protected Texture2D arrowDown;
        protected Texture2D talkingNPC;
        protected SpriteFont font;

        protected int screenWidth;
        protected int screenHeight;

        protected Rectangle DiagBoxTL;
        protected Rectangle DiagBoxTR;
        protected Rectangle DiagBoxBL;
        protected Rectangle DiagBoxBR;
        protected Rectangle DiagBoxL;
        protected Rectangle DiagBoxR;
        protected Rectangle DiagBoxT;
        protected Rectangle DiagBoxB;
        protected Rectangle DiagBoxM;
        protected Rectangle pictureNPC;
        protected Rectangle TextBox;

        protected string Text = " ";
        protected string Picture = "AcaHelper";
        protected string[] TextArray;

        protected int numberOfLines;

        protected bool canScroll = false;
        protected int lineCapacity;
        protected int scrollIndex = 0;
        protected bool arrowBlink = false;
        protected int timePastSinceLastArrowBling;
        protected int lineDist;
        protected int offset = 15;

        int timeSinceDialogstart = 0;

        #endregion

        public DialogListeningScreen() {}

        public DialogListeningScreen(string text, string picture = "AcaHelper")
        {
            this.Text = text;
            this.Picture = picture;
        }

        public override void Open(Game game, AssetManager assets)
        {
            talkBubbleTopLeft = assets.dialogTL;
            talkBubbleTopRight = assets.dialogTR;
            talkBubbleBottomLeft = assets.dialogBL;
            talkBubbleBottomRight = assets.dialogBR;
            talkBubbleLeft = assets.dialogL;
            talkBubbleRight = assets.dialogR;
            talkBubbleTop = assets.dialogT;
            talkBubbleBottom = assets.dialogB;
            talkBubbleMiddle = assets.dialogC;

            arrowDown = assets.dialogArrow;
            talkingNPC = assets.dialogImages[Picture];
            font = ScreenManager.Font;
            lineDist = font.LineSpacing;

            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            numberOfLines = 1;

            int offset = 5;
            int offsetImage = 10;
            int yPos = (screenHeight * 2) / 3;

            MakeBorderBox(new Rectangle(offset, yPos, screenWidth - 2 * offset, screenHeight - yPos - offset),
                out DiagBoxTL, out DiagBoxT, out DiagBoxTR, out DiagBoxR,
                out DiagBoxBR, out DiagBoxB, out DiagBoxBL, out DiagBoxL, out DiagBoxM);

            pictureNPC = new Rectangle(DiagBoxTL.X + offsetImage, DiagBoxTL.Y + offsetImage, (int)(3.3f * lineDist), (int)(3.75f * lineDist));
            TextBox = new Rectangle(pictureNPC.Right + offset, pictureNPC.Top + lineDist / 4, screenWidth - pictureNPC.Right - 2 * offset, pictureNPC.Height + offset);

            lineCapacity = (TextBox.Height - offset) / font.LineSpacing;

            TextArray = wrapText(Text, font, TextBox, lineCapacity);
        }

        public override void Update(GameTime gameTime)
        {
            bool enterPressed = false;

            // Check if enough time has past to continue through dialog with pressing space
            timeSinceDialogstart += gameTime.ElapsedGameTime.Milliseconds;

            if(((ScreenManager.Input.Equals(InputState.Continue)||ScreenManager.Input.Equals(InputState.Down) ) && Keyboard.GetState().IsKeyUp(Keys.Space))
                || ( timeSinceDialogstart > GameConstants.spaceLockTime) && ScreenManager.Input.Equals(InputState.Continue))
                enterPressed = true;

            // Check if text needs scrolling
            if (scrollIndex < (TextArray.Length-1) && TextArray[scrollIndex+1]!= null)
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
                if (enterPressed)
                {
                    ScreenManager.ResumeLast(this);
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
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

            m_sprite.DrawString(font, TextArray[scrollIndex], new Vector2(TextBox.X, TextBox.Y), Color.Black);

            if (canScroll)
            {
                if (arrowBlink)
                {
                    m_sprite.Draw(arrowDown, new Rectangle(DiagBoxBR.Right - 40, DiagBoxBR.Bottom - 35, 24, 15), Color.White);
                }
                else m_sprite.Draw(arrowDown, new Rectangle(DiagBoxBR.Right - 40, DiagBoxBR.Bottom - 32, 24, 15), Color.White);
            }

            m_sprite.End();

            // we need the following as spriteBatch.Begin() sets them to None and AlphaBlend
            // which breaks our model rendering
            GraphicsDevice m_graphics = ScreenManager.Game.GraphicsDevice;
            m_graphics.DepthStencilState = DepthStencilState.Default;
            m_graphics.BlendState = BlendState.Opaque;
        }

        /// <summary>
        /// Puts line breaks into a string to make it fit into a given text box.
        /// String will be returned in an array, where each entry is made to fit into the textbox
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="textBox"></param>
        /// <param name="lineCapacity"></param>
        /// <returns></returns>
        protected String[] wrapText(String text, SpriteFont font, Rectangle textBox, int lineCapacity)
        {
            String lineString = String.Empty;
            String returnString = String.Empty;
            String[] returnStringArray = new String[10]; // should be generous enough for our dialog
            String[] wordArray = text.Split(' ');

            int textportionIndex = 0;
            int numOfLinesInTextportion = 0;

            foreach (String word in wordArray)
            {
                float lineWidth = font.MeasureString(lineString + word).Length();
                if (word.Contains("!nl!"))
                {
                    int indexOfnl = word.IndexOf("!nl!");
                    returnString = returnString + lineString + '\n';
                    lineString = String.Empty + word.Substring(indexOfnl + 4) + ' ';
                    numOfLinesInTextportion++;
                    if (numOfLinesInTextportion == lineCapacity)
                    {
                        returnStringArray[textportionIndex] = returnString;
                        returnString = string.Empty;
                        numOfLinesInTextportion = 0;
                        textportionIndex++;
                    }
                    continue;
                }
                if(word.Contains("!ns!"))
                {
                    int indexOfns = word.IndexOf("!ns!");
                    returnString = returnString + lineString;
                    lineString = String.Empty + word.Substring(indexOfns + 4) + ' ';

                    returnStringArray[textportionIndex] = returnString;
                    returnString = string.Empty;
                    numOfLinesInTextportion = 0;
                    textportionIndex++;
                    continue;
                }
                if (lineWidth + offset > textBox.Width)
                {
                    returnString = returnString + lineString + '\n';
                    lineString = String.Empty;
                    numOfLinesInTextportion++;
                    if (numOfLinesInTextportion == lineCapacity)
                    {
                        returnStringArray[textportionIndex] = returnString;
                        returnString = string.Empty;
                        numOfLinesInTextportion = 0;
                        textportionIndex++;
                    }
                }

                lineString = lineString + word + ' ';
            }

            returnStringArray[textportionIndex] = returnString + lineString;

            return returnStringArray;
        }
    }
}
