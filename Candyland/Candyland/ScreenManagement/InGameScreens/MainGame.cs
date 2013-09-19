using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using Microsoft.Xna.Framework.Content;

namespace Candyland
{
    class MainGame : GameScreen
    {
        // the scene manager, most stuff happens in there
        SceneManager m_sceneManager;

        public MainGame()
        {
            this.isFullscreen = true;
        }

        public override void Open(Game game, AssetManager assets)
        {
            // tell screen manager that a game is already running
            ScreenManager.gameIsRunning = true;

            m_sceneManager = ScreenManager.SceneManager;
        }


        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.Input.Equals(InputState.Back))
            {
                // automatically save the game, when exiting to main menu
                ScreenManager.SceneManager.SaveGame();

                ScreenManager.ActivateNewScreen(new MainMenu());
            }

            if (ScreenManager.Game.IsActive && (gameTime.TotalGameTime.Milliseconds % GameConstants.framerate == 0)
                && ((ScreenManager.Input.Equals(InputState.Continue)) || GameConstants.singlestepperOFF))
            {
                m_sceneManager.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(GameConstants.backgroundColor);

            m_sceneManager.Draw(gameTime);
            m_sceneManager.Draw2D();
        }

        public override void Close()
        {

        }
    }
}
