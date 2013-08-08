using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class MainGame : GameScreen
    {
        // the scene manager, most stuff happens in there
        SceneManager m_sceneManager;

        public override void Open()
        {
            this.ScreenState = ScreenState.Active;

            m_sceneManager = new SceneManager(ScreenManager.GraphicsDevice);

            Song song = ScreenManager.Content.Load<Song>("bgmusic");  // background music from http://longzijun.wordpress.com/2012/12/26/upbeat-background-music-free-instrumentals/
           // MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;

            // Load all content required by the scene
            m_sceneManager.Load(ScreenManager.Content);
        }

        public override void Update(GameTime gameTime)
        {
            m_sceneManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.Orange);

            m_sceneManager.Draw(gameTime);
            m_sceneManager.Draw2D(ScreenManager.SpriteBatch);
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }
    }
}
