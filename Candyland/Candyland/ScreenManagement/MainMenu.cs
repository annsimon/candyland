using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Threading;

namespace Candyland
{
    class MainMenu : GameScreen
    {
        Texture2D caption;
        Texture2D buttonTexture;

        protected SpriteFont font;

        int screenWidth;
        int screenHeight;

        int activeButtonIndex = 0;

        // Buttons
        int numberOfButtons;
        int buttonWidth;
        int buttonHeight;
        int leftAlign;
        int topAlign;
        Rectangle selectedButton;

        // Strings for Buttons
        string play = "Weiterspielen";
        string newGame = "Neues Spiel";
        string options = "Optionen";
        string bonus = "Extras";
        string credits = "Credits";
        string end = "Spiel beenden";

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

        public override void Open(Game game)
        {
            this.isFullscreen = true;

            caption = ScreenManager.Content.Load<Texture2D>("Images/Captions/MainMenu");
            buttonTexture = ScreenManager.Content.Load<Texture2D>("ScreenTextures/transparent");

            font = ScreenManager.Font;

            // Initialize Buttons
            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            numberOfButtons = 6;
            buttonWidth = screenWidth / 4;
            buttonHeight = (screenHeight * 29) / 50 / numberOfButtons;
            leftAlign = (screenWidth - buttonWidth) / 2;
            topAlign = screenWidth / 6 + 10;

            selectedButton = new Rectangle(leftAlign, topAlign, buttonWidth, buttonHeight);

            BorderTopLeft = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogTopLeft");
            BorderTopRight = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogTopRight");
            BorderBottomLeft = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogBottomLeft");
            BorderBottomRight = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogBottomRight");
            BorderLeft = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogLeft");
            BorderRight = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogRight");
            BorderTop = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogTop");
            BorderBottom = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogBottom");
            BorderMiddle = ScreenManager.Content.Load<Texture2D>("Images/Dialog/DialogMiddle");

            int offsetX = (screenWidth - buttonWidth)/2 - 100;
            int offsetY = screenHeight/6 - 50;

            MakeBorderBox(new Rectangle(offsetX, offsetY, screenWidth - 2 * offsetX, screenHeight - 2 * offsetY),
                out MenuBoxTL, out MenuBoxT, out MenuBoxTR, out MenuBoxR,
                out MenuBoxBR, out MenuBoxB, out MenuBoxBL, out MenuBoxL, out MenuBoxM);
        }

        public override void Update(GameTime gameTime)
        {
            bool enterPressed = false;

            // look at input and update button selection
            switch (ScreenManager.Input)
            {
                case InputState.Continue: enterPressed = true; break;
                case InputState.Up: activeButtonIndex--; break;
                case InputState.Down: activeButtonIndex++; break;
            }
            if (activeButtonIndex >= numberOfButtons) activeButtonIndex = 0;
            if (activeButtonIndex < 0) activeButtonIndex = numberOfButtons - 1;

            // Selected Button confirmed by pressing Enter
            if (enterPressed)
            {
                switch (activeButtonIndex)
                {
                    case 0: ScreenManager.StartNewGame(); break;
                    case 1: ScreenManager.ResumeGame(); break;
                    case 2: ScreenManager.ActivateNewScreen(new OptionsScreen()); break;
                    case 3: ScreenManager.ActivateNewScreen(new BonusScreen()); break;
                    case 4: ScreenManager.ActivateNewScreen(new CreditsScreen()); break;
                    case 5: ScreenManager.Game.Exit(); break ;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch m_sprite = ScreenManager.SpriteBatch;


            ScreenManager.GraphicsDevice.Clear(GameConstants.BackgroundColorMenu);

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
            if(ScreenManager.isFullscreen) m_sprite.Draw(caption, new Rectangle(MenuBoxL.Left + 5, MenuBoxT.Top + 5, caption.Width, caption.Height), Color.White);
            else m_sprite.Draw(caption, new Rectangle(MenuBoxL.Left + 5, MenuBoxT.Top + 5, (int)(caption.Width * 0.8f), (int)(caption.Height*0.8f)), Color.White);

            // Draw Option Strings
            Color textColor = Color.Black;

            m_sprite.DrawString(font, play, new Vector2(leftAlign + (buttonWidth - font.MeasureString(play).X) / 2, topAlign - buttonHeight/2), textColor);
            m_sprite.DrawString(font, newGame, new Vector2(leftAlign + (buttonWidth - font.MeasureString(newGame).X) / 2, topAlign + (buttonHeight)), textColor);
            m_sprite.DrawString(font, options, new Vector2(leftAlign + (buttonWidth - font.MeasureString(options).X) / 2, topAlign + (buttonHeight * 2)), textColor);
            m_sprite.DrawString(font, bonus, new Vector2(leftAlign + (buttonWidth - font.MeasureString(bonus).X) / 2, topAlign + (buttonHeight * 3)), textColor);
            m_sprite.DrawString(font, credits, new Vector2(leftAlign + (buttonWidth - font.MeasureString(credits).X) / 2, topAlign + (buttonHeight * 4)), textColor);
            m_sprite.DrawString(font, end, new Vector2(leftAlign + (buttonWidth - font.MeasureString(end).X) / 2, topAlign + (buttonHeight * 5)), textColor);

            selectedButton.Y = topAlign + (buttonHeight * activeButtonIndex);
            if (activeButtonIndex == 0) selectedButton.Y -= buttonHeight / 2;
            m_sprite.Draw(buttonTexture, selectedButton, Color.White);

            m_sprite.End();
        }

        public override void Close()
        {
            ;
        }
    }
}
