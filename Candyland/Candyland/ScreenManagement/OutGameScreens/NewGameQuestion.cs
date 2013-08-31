using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Candyland
{
    class NewGameQuestion : YesNoScreen
    {
        public override void Open(Game game)
        {
            base.Open(game);

            question = "Neues Spiel starten?\nAlter Spielstand geht verloren!";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (enterPressed && answer)
            {
                ScreenManager.RemoveScreen(this);
                ScreenManager.StartNewGame();
            }
        }
    }
}
