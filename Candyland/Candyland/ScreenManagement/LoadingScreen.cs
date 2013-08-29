using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    /// <summary>
    /// shows an animation while a parallel tread loads the game content
    /// </summary>
    class LoadingScreen : GameScreen
    {
        Texture2D background;
        int screenWidth;
        int screenHeight;

        public override void Open(Game game)
        {
            this.isFullscreen = true;

            ContentManager content = ScreenManager.Content;

            screenWidth = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            screenHeight = ScreenManager.Game.GraphicsDevice.Viewport.Height;

            background = content.Load<Texture2D>("ScreenTextures/optionsScreen");
        }

        public override void Update(GameTime gameTime)
        {
            if (screenWidth < 1) screenWidth = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            else
            {
                screenWidth -= 1;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.Black);

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(background, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            ScreenManager.SpriteBatch.End();
        }

        public void RequestStop()
        {
            shouldStop = true;
        }
        // Volatile is used as hint to the compiler that this data
        // member will be accessed by multiple threads.
        private volatile bool shouldStop;
    }
}
