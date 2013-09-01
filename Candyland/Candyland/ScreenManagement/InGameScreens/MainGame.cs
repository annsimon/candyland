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

        // has its own content manager
        ContentManager content;

        public override void Open(Game game)
        {
            // tell screen manager that a game is already running
            ScreenManager.gameIsRunning = true;

            this.isFullscreen = true;

            m_sceneManager = ScreenManager.SceneManager;

            if (content == null)
                content = ScreenManager.gameContent;

            ScreenManager.gameContent = content;

            Song song = content.Load<Song>("Music/bgmusic");  // background music from http://longzijun.wordpress.com/2012/12/26/upbeat-background-music-free-instrumentals/
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
        }


        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.Input.Equals(InputState.Back))
            {
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
            // Unload content
            content.Unload();
        }
    }
}
