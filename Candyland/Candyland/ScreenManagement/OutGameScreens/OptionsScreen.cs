using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Candyland
{
    class OptionsScreen : GameScreen
    {
        #region fields

        SaveSettingsData settings;

        Texture2D caption;

        protected SpriteFont font;
        protected SpriteFont Font;
        protected SpriteFont titleFont;
        Color textColor = Color.Black;
        int lineDist;

        int screenWidth;
        int screenHeight;

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

        // Box for Options and Control description
        protected Rectangle TextBox;

        // Title Menu
        int buttonWidth;
        int buttonHeight;
        int titleLeftAlign;
        int titleTopAlign;
        int activeTitleButtonIndex = 1;
        int numOfTitleButtons = 5;
        Texture2D buttonTexture;
        Rectangle selectedButton;

        // Graphics menu
        CheckBox checkBoxFullscreen;
        CheckBox checkBoxWindow;
        SlideControl slideControlShadow;
        int activeGraphicOptionIndex = 1;
        int numOfGraphicOptions = 4;

        // Audio menu
        SlideControl slideControlMusic;
        SlideControl slideControlSound;
        int activeAudioOptionIndex = 1;
        int numOfAudioOptions = 3;

        // Tutorial menu
        CheckBox checkBoxTutorial;
        int activeTutorialOptionIndex = 1;
        int numOfTutorialOptions = 2;

        // Which Menu is used
        bool insideTitleMenu = true;
        bool insideGraphics = false;
        bool insideAudio = false;
        bool insideTutorial = false;

        // Buttons
        Button saveButton;
        Button backButton;

        // Slider Comments
        string string_off = "-aus-";
        string string_notAny = "-keine-";
        string string_low = "-niedrig-";
        string string_medium = "-mittel-";
        string string_high = "-hoch-";
        string string_max = "-max-";

        #endregion

        #region Initialize

        public OptionsScreen()
        {
            this.isFullscreen = true;
        }

        public override void Open(Game game, AssetManager assets)
        {
            settings = ScreenManager.Settings;

            caption = assets.captionOptions;
            buttonTexture = assets.menuSelection;

            BorderTopLeft = assets.dialogTL;
            BorderTopRight = assets.dialogTR;
            BorderBottomLeft = assets.dialogBL;
            BorderBottomRight = assets.dialogBR;
            BorderLeft = assets.dialogL;
            BorderRight = assets.dialogR;
            BorderTop = assets.dialogT;
            BorderBottom = assets.dialogB;
            BorderMiddle = assets.dialogC;

            font = assets.mainText;
            Font = assets.mainTextFullscreen;
            titleFont = assets.titleText;

            lineDist = font.LineSpacing;

            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            int offset = 5;

            int MenuBoxWidth = ScreenManager.PrefScreenWidth - 2 * offset;
            int MenuBoxHeight = ScreenManager.PrefScreenHeight - 2 * offset;

            MakeBorderBox(new Rectangle((screenWidth - MenuBoxWidth) / 2, (screenHeight - MenuBoxHeight) / 2, MenuBoxWidth, MenuBoxHeight),
                out MenuBoxTL, out MenuBoxT, out MenuBoxTR, out MenuBoxR,
                out MenuBoxBR, out MenuBoxB, out MenuBoxBL, out MenuBoxL, out MenuBoxM);

            TextBox = new Rectangle(MenuBoxL.Left + 240, MenuBoxT.Top + 60, 600, 280);

            buttonWidth = (int)Font.MeasureString("Steuerung").X + 20;
            buttonHeight = Font.LineSpacing;

            titleLeftAlign = MenuBoxL.Right;
            titleTopAlign = MenuBoxT.Bottom + 100;

            selectedButton = new Rectangle(titleLeftAlign - 10, titleTopAlign, buttonWidth, buttonHeight);

            checkBoxFullscreen = new CheckBox(settings.isFullscreen, new Vector2(TextBox.Left + 315, TextBox.Top + 55), assets, this);
            checkBoxWindow = new CheckBox(!settings.isFullscreen, new Vector2(TextBox.Left + 65, TextBox.Top + 55), assets, this);
            slideControlShadow = new SlideControl(2, settings.shadowQuality, new Vector2(TextBox.Left + 85, TextBox.Top + 205), assets);

            slideControlMusic = new SlideControl(GameConstants.numberOfVolumeSteps, settings.musicVolume, new Vector2(TextBox.Left + 85, TextBox.Top + 55), assets);
            slideControlSound = new SlideControl(GameConstants.numberOfVolumeSteps, settings.soundVolume, new Vector2(TextBox.Left + 85, TextBox.Top + 205), assets);

            checkBoxTutorial = new CheckBox(settings.showTutorial, new Vector2(TextBox.Left + 65, TextBox.Top + 145), assets, this);

            saveButton = new Button("Speichern", new Vector2(MenuBoxR.Left - 90, MenuBoxB.Top - 20), assets, this);
            backButton = new Button("Zurück", new Vector2(MenuBoxL.Right - 25, MenuBoxB.Top - 20), assets, this);
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            // Inside title options
            if (insideTitleMenu)
            {
                UpdateTitleMenu();
                return;
            }  
                
            // Inside graphics menu
            if (insideGraphics)
            {
                UpdateGraphicsMenu();
                return;
            }

            // Inside audio menu
            if (insideAudio)
            {
                UpdateAudioMenu();
                return;
            }

            // Inside tutorial menu
            if (insideTutorial)
            {
                UpdateTutorialMenu();
                return;
            }

        }

        private void UpdateTitleMenu()
        {
            bool enterPressed = false;

            switch (ScreenManager.Input)
            {
                case InputState.Continue: enterPressed = true; break;
                case InputState.Up: activeTitleButtonIndex--; break;
                case InputState.Down: activeTitleButtonIndex++; break;
                case InputState.Right: enterPressed = true; break;
                case InputState.Back: ScreenManager.ResumeLast(this); break;
            }

            if (activeTitleButtonIndex > numOfTitleButtons) activeTitleButtonIndex = 1;
            if (activeTitleButtonIndex < 1) activeTitleButtonIndex = numOfTitleButtons;

            if (activeTitleButtonIndex == numOfTitleButtons) backButton.selected = true;

            if (enterPressed)
            {
                switch (activeTitleButtonIndex)
                {
                    case 1: break;
                    case 2: insideTitleMenu = false; insideGraphics = true; break;
                    case 3: insideTitleMenu = false; insideAudio = true; break;
                    case 4: insideTitleMenu = false; insideTutorial = true; break;
                    case 5: ScreenManager.ResumeLast(this); break;
                }
            }
        }

        private void UpdateGraphicsMenu()
        {
            bool enterPressed = false;

            if (slideControlShadow.active)
            {
                switch (ScreenManager.Input)
                {
                    case InputState.Continue: enterPressed = true; break;
                    case InputState.Right: slideControlShadow.value++; break;
                    case InputState.Left: slideControlShadow.value--; break;
                }
            }
            else
            {
                switch (ScreenManager.Input)
                {
                    case InputState.Continue: enterPressed = true; break;
                    case InputState.Up: if (activeGraphicOptionIndex == 2) activeGraphicOptionIndex -= 2; else activeGraphicOptionIndex--; break;
                    case InputState.Down: if (activeGraphicOptionIndex == 1) activeGraphicOptionIndex += 2; else activeGraphicOptionIndex++; break;
                    case InputState.Right: if (activeGraphicOptionIndex == 1) activeGraphicOptionIndex++; break;
                    case InputState.Back: insideGraphics = false; insideTitleMenu = true; break;
                    case InputState.Left: if (activeGraphicOptionIndex == 2) activeGraphicOptionIndex--;
                        else { insideGraphics = false; insideTitleMenu = true; } break;
                }
            }

            if (activeGraphicOptionIndex > numOfGraphicOptions) activeGraphicOptionIndex = 1;
            if (activeGraphicOptionIndex < 1) activeGraphicOptionIndex = numOfGraphicOptions;

            switch (activeGraphicOptionIndex)
            {
                case 1: checkBoxWindow.selected = true; break;
                case 2: checkBoxFullscreen.selected = true; break;
                case 3: slideControlShadow.selected = true; break;
                case 4: saveButton.selected = true; break;
            }

            // set index to first one, when leaving
            if(insideTitleMenu)
            {
                activeAudioOptionIndex = 1;
                return;
            }

            if (enterPressed)
            {
                switch (activeGraphicOptionIndex)
                {
                    case 1: checkBoxFullscreen.checkedOff = false; checkBoxWindow.checkedOff = true; break;
                    case 2: checkBoxFullscreen.checkedOff = true; checkBoxWindow.checkedOff = false; break;
                    case 3: if (slideControlShadow.active) slideControlShadow.active = false; else slideControlShadow.active = true; break;
                    case 4: ScreenManager.ActivateNewScreen(new SaveSettingsGraphicQuestion(checkBoxFullscreen.checkedOff, slideControlShadow.value)); break;
                }
            }
        }

        private void UpdateAudioMenu()
        {
            bool enterPressed = false;

            if (slideControlMusic.active)
            {
                switch (ScreenManager.Input)
                {
                    case InputState.Continue: enterPressed = true; break;
                    case InputState.Right: slideControlMusic.value++; break;
                    case InputState.Left: slideControlMusic.value--; break;
                }
            }
            else if (slideControlSound.active)
            {
                switch (ScreenManager.Input)
                {
                    case InputState.Continue: enterPressed = true; break;
                    case InputState.Right: slideControlSound.value++; break;
                    case InputState.Left: slideControlSound.value--; break;
                }
            }
            else
            {
                switch (ScreenManager.Input)
                {
                    case InputState.Continue: enterPressed = true; break;
                    case InputState.Up: activeAudioOptionIndex--; break;
                    case InputState.Down: activeAudioOptionIndex++; break;
                    case InputState.Back: insideAudio = false; insideTitleMenu = true; break;
                    case InputState.Left: insideAudio = false; insideTitleMenu = true; break;
                }
            }

            if (activeAudioOptionIndex > numOfAudioOptions) activeAudioOptionIndex = 1;
            if (activeAudioOptionIndex < 1) activeAudioOptionIndex = numOfAudioOptions;

            switch (activeAudioOptionIndex)
            {
                case 1: slideControlMusic.selected = true; break;
                case 2: slideControlSound.selected = true; break;
                case 3: saveButton.selected = true; break;
            }

            // set index to first one, when leaving
            if (insideTitleMenu)
            {
                activeAudioOptionIndex = 1;
                return;
            }

            if (enterPressed)
            {
                switch (activeAudioOptionIndex)
                {
                    case 1: if (slideControlMusic.active) slideControlMusic.active = false; else slideControlMusic.active = true; break;
                    case 2: if (slideControlSound.active) slideControlSound.active = false; else slideControlSound.active = true; break;
                    case 3: ScreenManager.ActivateNewScreen(new SaveSettingsAudioQuestion(slideControlMusic.value, slideControlSound.value)); break;
                }
            }
        }

        private void UpdateTutorialMenu()
        {
            bool enterPressed = false;

            switch (ScreenManager.Input)
            {
                case InputState.Continue: enterPressed = true; break;
                case InputState.Up: activeTutorialOptionIndex--; break;
                case InputState.Down: activeTutorialOptionIndex++; break;
                case InputState.Back: insideTutorial = false; insideTitleMenu = true; break;
                case InputState.Left: insideTutorial = false; insideTitleMenu = true; break;
            }

            if (activeTutorialOptionIndex > numOfTutorialOptions) activeTutorialOptionIndex = 1;
            if (activeTutorialOptionIndex < 1) activeTutorialOptionIndex = numOfTutorialOptions;

            switch (activeTutorialOptionIndex)
            {
                case 1: checkBoxTutorial.selected = true; break;
                case 2: saveButton.selected = true; break;
            }

            // set index to first one, when leaving
            if (insideTitleMenu)
            {
                activeTutorialOptionIndex = 1;
                return;
            }

            if (enterPressed)
            {
                switch (activeTutorialOptionIndex)
                {
                    case 1: if (checkBoxTutorial.checkedOff) checkBoxTutorial.checkedOff = false; else checkBoxTutorial.checkedOff = true; break;
                    case 2: ScreenManager.ActivateNewScreen(new SaveSettingsTutorialQuestion(checkBoxTutorial.checkedOff)); break;
                }
            }
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            int screenWidth = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            int screenHeight = ScreenManager.Game.GraphicsDevice.Viewport.Height;

            ScreenManager.GraphicsDevice.Clear(GameConstants.BackgroundColorMenu);

            SpriteBatch m_sprite = ScreenManager.SpriteBatch;

            m_sprite.Begin();

            DrawMenuBorder(m_sprite);

            DrawTitleMenu(m_sprite);

            // Draw control description
            if (insideTitleMenu && activeTitleButtonIndex == 1)
            {
                m_sprite.DrawString(font, GameConstants.controlDescription1, new Vector2(TextBox.X, TextBox.Y), textColor);
                m_sprite.DrawString(font, GameConstants.controlDescription2, new Vector2(TextBox.X + 345, TextBox.Y), textColor);
            }

            if (insideGraphics || (insideTitleMenu && activeTitleButtonIndex == 2))
            {
                DrawGraphicsMenu(m_sprite);
                ScreenManager.SpriteBatch.End();
                return;
            }

            if (insideAudio || (insideTitleMenu && activeTitleButtonIndex == 3))
            {
                DrawAudioMenu(m_sprite);
                ScreenManager.SpriteBatch.End();
                return;
            }

            if (insideTutorial || (insideTitleMenu && activeTitleButtonIndex == 4))
            {
                DrawTutorialMenu(m_sprite);
                ScreenManager.SpriteBatch.End();
                return;
            }

            ScreenManager.SpriteBatch.End();
        }

        private void DrawGraphicsMenu(SpriteBatch m_sprite)
        {
            int left = TextBox.X + 130;
            int top = TextBox.Y;
            int titleEndX = selectedButton.Right - 10;
            // Fullscreen options
            m_sprite.DrawString(titleFont, "Anzeige",
                    new Vector2(titleEndX+ (TextBox.Width - (int)titleFont.MeasureString("Anzeige").X) / 2, top), Color.Black);
            m_sprite.DrawString(font, "im Fenster", new Vector2(left, top + 2 * lineDist + 5), Color.Black);
            m_sprite.DrawString(font, "als Vollbild", new Vector2(left + 250, top + 2 * lineDist + 5), Color.Black);
            m_sprite.DrawString(font, "Änderung tritt erst nach Spielneustart ein!",
                new Vector2(TextBox.Left + 65, top + 4 * lineDist - 10), Color.Black);
            checkBoxFullscreen.Draw(m_sprite);
            checkBoxWindow.Draw(m_sprite);
            // Shadow Options
            m_sprite.DrawString(titleFont, "Schattenqualitität",
                    new Vector2(titleEndX + (TextBox.Width - (int)titleFont.MeasureString("Schattenqualität").X) / 2, top + 5 * lineDist), Color.Black);

            string comment = "";
            switch (slideControlShadow.value)
            {
                case 0: comment = string_notAny; break;
                case 1: comment = string_low; break;
                case 2: comment = string_high; break;
            }
            slideControlShadow.comment = comment;

            slideControlShadow.Draw(m_sprite);

            // save button
            if(insideGraphics) saveButton.Draw(m_sprite);
        }

        private void DrawAudioMenu(SpriteBatch m_sprite)
        {
            int left = TextBox.X + 130;
            int top = TextBox.Y;
            int titleEndX = selectedButton.Right - 10;
            // Volumes
            m_sprite.DrawString(titleFont, "Lautstärke",
                    new Vector2(titleEndX + (TextBox.Width - (int)titleFont.MeasureString("Lautstärke").X) / 2, top), Color.Black);
            m_sprite.DrawString(font, "Musik", new Vector2(left + 300, top + 80), Color.Black);
            m_sprite.DrawString(font, "Sound", new Vector2(left + 300, top + 230), Color.Black);

            string commentMusic = "";
            switch (slideControlMusic.value)
            {
                case 0: commentMusic = string_off; break;
                case GameConstants.numberOfVolumeSteps: commentMusic = string_max; break;
            }
            slideControlMusic.comment = commentMusic;
            string commentSound = "";
            switch (slideControlSound.value)
            {
                case 0: commentSound = string_off; break;
                case GameConstants.numberOfVolumeSteps: commentSound = string_max; break;
            }
            slideControlSound.comment = commentSound;

            slideControlMusic.Draw(m_sprite);
            slideControlSound.Draw(m_sprite);
            
            // save button
            if(insideAudio) saveButton.Draw(m_sprite);
        }

        private void DrawTutorialMenu(SpriteBatch m_sprite)
        {
            int left = TextBox.X + 150;
            int top = TextBox.Y;
            int titleEndX = selectedButton.Right - 10;
            // Show introduction
            m_sprite.DrawString(titleFont, "Tutorialhinweise anzeigen", new Vector2(left, top + 150), Color.Black);
            checkBoxTutorial.Draw(m_sprite);
            // save button
            if(insideTutorial) saveButton.Draw(m_sprite);
        }

        private void DrawTitleMenu(SpriteBatch m_sprite)
        {
            // Draw title options
            m_sprite.DrawString(Font, "Steuerung", new Vector2(titleLeftAlign, titleTopAlign), textColor);
            m_sprite.DrawString(Font, "Grafik", new Vector2(titleLeftAlign, titleTopAlign + Font.LineSpacing), textColor);
            m_sprite.DrawString(Font, "Audio", new Vector2(titleLeftAlign, titleTopAlign + 2 * Font.LineSpacing), textColor);
            m_sprite.DrawString(Font, "Tutorial", new Vector2(titleLeftAlign, titleTopAlign + 3 * Font.LineSpacing), textColor);
            // Draw button selection
            backButton.Draw(m_sprite);
            if (insideTitleMenu && activeTitleButtonIndex < numOfTitleButtons)
            {
                selectedButton.Y = titleTopAlign + (buttonHeight * (activeTitleButtonIndex - 1));
                m_sprite.Draw(buttonTexture, selectedButton, Color.White);
            }
        }

        private void DrawMenuBorder(SpriteBatch m_sprite)
        {
            m_sprite.Draw(BorderTopLeft, MenuBoxTL, Color.White);
            m_sprite.Draw(BorderTopRight, MenuBoxTR, Color.White);
            m_sprite.Draw(BorderBottomLeft, MenuBoxBL, Color.White);
            m_sprite.Draw(BorderBottomRight, MenuBoxBR, Color.White);
            m_sprite.Draw(BorderLeft, MenuBoxL, Color.White);
            m_sprite.Draw(BorderRight, MenuBoxR, Color.White);
            m_sprite.Draw(BorderTop, MenuBoxT, Color.White);
            m_sprite.Draw(BorderBottom, MenuBoxB, Color.White);
            m_sprite.Draw(BorderMiddle, MenuBoxM, Color.White);
            m_sprite.Draw(caption, new Rectangle(MenuBoxL.Left + 5, MenuBoxT.Top + 5, (int)(caption.Width * 0.8f), (int)(caption.Height * 0.8f)), Color.White);
        }

        #endregion
    }
}
