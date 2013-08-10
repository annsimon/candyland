using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class CreditsScreen : GameScreen
    {
        Texture2D texture;

        public override void Open(Game game)
        {
            this.isFullscreen = true;

            texture = ScreenManager.Content.Load<Texture2D>("ScreenTextures/creditScreen");
        }

        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.Input.Equals(InputState.Enter))
            {
                ScreenManager.ResumeLast(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            int screenWidth = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            int screenHeight = ScreenManager.Game.GraphicsDevice.Viewport.Height;

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(texture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            ScreenManager.SpriteBatch.End();
        }
    }
}
