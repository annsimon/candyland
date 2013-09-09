using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Candyland
{
    class EndGameQuestion : YesNoScreen
    {
        public override void Open(Game game, AssetManager assets)
        {
            base.Open(game, assets);

            question = "Bist du sicher?";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (enterPressed && answer)
            {
                ScreenManager.Game.Exit();
            }
        }
    }
}