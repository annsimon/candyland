using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Threading;

namespace Candyland
{
    class MainMenu : GameScreen
    {
        Texture2D background;
        Texture2D buttonTexture;

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
        //Rectangle button0;
        //Rectangle button1;
        //Rectangle button2;
        //Rectangle button3;
        //Rectangle button4;
        //Rectangle button5;

        public override void Open(Game game)
        {
            this.isFullscreen = true;

            background = ScreenManager.Content.Load<Texture2D>("ScreenTextures/mainMenu");
            buttonTexture = ScreenManager.Content.Load<Texture2D>("ScreenTextures/transparent");

            // Initialize Buttons
            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            numberOfButtons = 6;
            buttonWidth = screenWidth / 2;
            buttonHeight = (screenHeight * 29) / 50 / numberOfButtons;
            leftAlign = (screenWidth - buttonWidth) / 2;
            topAlign = screenHeight / 3;
            //int buttonWidth = screenWidth / 3;
            //int buttonHeight = (screenHeight * 4) / 5 / numberOfButtons;
            //int leftAlign = (screenWidth - buttonWidth) / 2;
            //int topAlign = (screenHeight - numberOfButtons * buttonHeight) / 2;

            selectedButton = new Rectangle(leftAlign, topAlign, buttonWidth, buttonHeight);
            //button0 = new Rectangle(leftAlign, topAlign, buttonWidth, buttonHeight);
            //button1 = new Rectangle(leftAlign, topAlign + buttonHeight, buttonWidth, buttonHeight);
            //button2 = new Rectangle(leftAlign, topAlign + (buttonHeight * 2), buttonWidth, buttonHeight);
            //button3 = new Rectangle(leftAlign, topAlign + (buttonHeight * 3), buttonWidth, buttonHeight);
            //button4 = new Rectangle(leftAlign, topAlign + (buttonHeight * 4), buttonWidth, buttonHeight);
            //button5 = new Rectangle(leftAlign, topAlign + (buttonHeight * 5), buttonWidth, buttonHeight);
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

            m_sprite.Begin();

            m_sprite.Draw(background, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

            selectedButton.Y = topAlign + (buttonHeight * activeButtonIndex);
            m_sprite.Draw(buttonTexture, selectedButton, Color.White);

            //switch (activeButtonIndex)
            //{
            //    case 0: m_sprite.Draw(buttonTexture, button0, Color.White); break;
            //    case 1: m_sprite.Draw(buttonTexture, button1, Color.White); break;
            //    case 2: m_sprite.Draw(buttonTexture, button2, Color.White); break;
            //    case 3: m_sprite.Draw(buttonTexture, button3, Color.White); break;
            //    case 4: m_sprite.Draw(buttonTexture, button4, Color.White); break;
            //    case 5: m_sprite.Draw(buttonTexture, button5, Color.White); break;
            //}
            m_sprite.End();

            // we need the following as spriteBatch.Begin() sets them to None and AlphaBlend
            // which breaks our model rendering
            //m_graphics.DepthStencilState = DepthStencilState.Default;
            //m_graphics.BlendState = BlendState.Opaque;
        }

        public override void Close()
        {
            ;
        }
    }
}
