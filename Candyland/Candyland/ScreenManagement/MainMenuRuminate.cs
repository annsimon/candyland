using System;
using Microsoft.Xna.Framework;
using Ruminate.GUI.Framework;
using Ruminate.GUI.Content;

namespace Candyland
{
    class MainMenuRuminate : GameScreen
    {
        Gui _gui;

        public override void Open(Game game)
        {
            game.IsMouseVisible = true;
            this.ScreenState = ScreenState.Active;
            this.isFullscreen = true;

            int screenWidth = game.GraphicsDevice.Viewport.Width;
            int screenHeight = game.GraphicsDevice.Viewport.Height;

            int numberOfButtons = 6;
            int buttonDistance = 50;
            int buttonWidth = screenWidth / 3;
            int leftAlign = (screenWidth - buttonWidth) / 2;
            int topAlign = (screenHeight - numberOfButtons * buttonDistance) / 2;

            var testSkin = new Skin(ScreenManager.TestImageMap, ScreenManager.TestMap);
            var testText = new Text(ScreenManager.Font, Color.Black);

            var testSkins = new[] { new Tuple<string, Skin>("testSkin", testSkin) };
            var testTexts = new[] { new Tuple<string, Text>("testText", testText) };

            _gui = new Gui(game, testSkin, testText, testSkins, testTexts)
            {
                Widgets = new Widget[] {
                    new Button(leftAlign, topAlign + (0 * buttonDistance), buttonWidth, "Spiel fortsetzen", buttonEvent: delegate (Widget widget) {
                        ScreenManager.ResumeGame();
                    }),
                    new Button(leftAlign, topAlign + (1 * buttonDistance), buttonWidth, "Neues Spiel"),

                    new Button(leftAlign, topAlign + (2 * buttonDistance), buttonWidth, "Optionen"),
                    new Button(leftAlign, topAlign + (3 * buttonDistance), buttonWidth, "Bonusmaterial"),

                    new Button(leftAlign, topAlign + (4 * buttonDistance), buttonWidth, "Credits"),
                    new Button(leftAlign, topAlign + (5 * buttonDistance), buttonWidth, "Beenden", buttonEvent: delegate (Widget widget) {
                        game.Exit();
                    }),
                }
            };
        }

        public override void Update(GameTime gameTime)
        {
            _gui.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            _gui.Draw();
        }

        public override void Close()
        {
            _gui = null;
        }
    }
}
