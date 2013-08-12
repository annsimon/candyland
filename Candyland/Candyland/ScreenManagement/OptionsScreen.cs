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
        Texture2D background;

        public override void Open(Game game)
        {
            isFullscreen = true;

            ContentManager content = ScreenManager.Content;

            background = content.Load<Texture2D>("ScreenTextures/optionsScreen");
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
            ScreenManager.SpriteBatch.Draw(background, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            ScreenManager.SpriteBatch.End();
        }
    }
}
