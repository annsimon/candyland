using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class TitleScreen : GameScreen
    {
        Texture2D texture;
        int timePastSinceStart = 0;

        public override void Open(Game game)
        {
            this.isFullscreen = true;

            texture = ScreenManager.Content.Load<Texture2D>("ScreenTextures/Main");
        }

        public override void Update(GameTime gameTime)
        {
            // timeout
            timePastSinceStart += gameTime.ElapsedGameTime.Milliseconds;

            if (ScreenManager.Input.Equals(InputState.Continue) || timePastSinceStart > 3000)
            {
                ScreenManager.RemoveScreen(this);
                ScreenManager.ActivateNewScreen(new MainMenu());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            int screenWidth = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            int screenHeight = ScreenManager.Game.GraphicsDevice.Viewport.Height;

            ScreenManager.GraphicsDevice.Clear(Color.BlanchedAlmond);

            ScreenManager.SpriteBatch.Begin();

            // Scaling
            float scalingFactor = 1;
            int picWidth, picHeight;
            if (texture.Height > screenHeight)
            {
                scalingFactor = ((float)screenHeight / (float)texture.Height);
            }
            picWidth = (int)(texture.Width*scalingFactor);
            picHeight = (int)(texture.Height*scalingFactor);

            Rectangle textureBox = new Rectangle(0, 0, picWidth, picHeight);
            textureBox.Offset(screenWidth / 2 - picWidth / 2, screenHeight / 2 - picHeight/2);
            ScreenManager.SpriteBatch.Draw(texture, textureBox, Color.White);

            ScreenManager.SpriteBatch.End();
        }

    }
}
