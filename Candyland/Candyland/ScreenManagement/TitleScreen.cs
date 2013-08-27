using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class TitleScreen : GameScreen
    {
        Texture2D texture;

        public override void Open(Game game)
        {
            this.isFullscreen = true;

            texture = ScreenManager.Content.Load<Texture2D>("ScreenTextures/Main");
        }

        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.Input.Equals(InputState.Continue))
            {
                ScreenManager.RemoveScreen(this);
                ScreenManager.ActivateNewScreen(new MainMenu());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            int screenWidth = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            int screenHeight = ScreenManager.Game.GraphicsDevice.Viewport.Height;

            ScreenManager.SpriteBatch.Begin();
            Rectangle textureBox = new Rectangle(0, 0, texture.Width, texture.Height);
            textureBox.Offset(screenWidth / 2 - texture.Width / 2, screenHeight / 2 - texture.Height/2);
            ScreenManager.SpriteBatch.Draw(texture, textureBox, Color.White);
            ScreenManager.SpriteBatch.End();
        }

    }
}
