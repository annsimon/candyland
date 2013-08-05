using System;
using Microsoft.Xna.Framework;
using Ruminate.GUI.Framework;
using Ruminate.GUI.Content;

namespace Candyland
{
    class MainMenu : GameScreen
    {
        Gui _gui;
        SingleLineTextBox _label, _padding, _width;

        public override void Open(Game game)
        {
            var testSkin = new Skin(ScreenManager.TestImageMap, ScreenManager.TestMap);
            var testText = new Text(ScreenManager.Font, Color.Black);

            var testSkins = new[] { new Tuple<string, Skin>("testSkin", testSkin) };
            var testTexts = new[] { new Tuple<string, Text>("testText", testText) };

            _gui = new Gui(game, testSkin, testText, testSkins, testTexts)
            {
                Widgets = new Widget[] {
                    new Button(10, 10 + (40 * 0), "Button"),
                    new Button(10, 10 + (40 * 1), "Skin") { Skin = "testSkin", Text = "testText" },
                    
                    new Button(10, 10 + (40 * 2), "Change Label", buttonEvent: delegate(Widget widget) {
                        ((Button)widget).Label = _label.Value;
                    }),
                    _label = new SingleLineTextBox(220, 10 + (40 * 2), 100, 10),

                    new Button(10, 10 + (40 * 4), "TextPadding = 25", 25),
                    new Button(10, 10 + (40 * 5), "TextPadding = 25") { TextPadding = 25 },
                    new Button(10, 10 + (40 * 6), "Change TextPadding", buttonEvent: delegate(Widget widget) {
                        int value;
                        if (int.TryParse(_padding.Value, out value)) {
                            ((Button)widget).TextPadding = value;
                        }
                    }),
                    _padding = new SingleLineTextBox(220, 10 + (40 * 6), 100, 10),

                    new Button(10, 10 + (40 * 8), 200, "Width = 200"),
                    new Button(10, 10 + (40 * 9), "Width = 200") { Width = 200 },
                    new Button(10, 10 + (40 * 10), "Change Width", buttonEvent: delegate(Widget widget) {
                        int value;
                        if (int.TryParse(_width.Value, out value)) {
                            ((Button)widget).Width = value;
                        }
                    }),
                    _width = new SingleLineTextBox(220, 10 + (40 * 10), 100, 10)
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
            ;
        }
    }
}
