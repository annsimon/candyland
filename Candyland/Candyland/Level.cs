using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Candyland
{
    /// <summary>
    /// This is a level.
    /// </summary>
    public class Level
    {
        UpdateInfo m_updateInfo;
        Camera m_camera;
        // this dictionary contains all game objects of the level
        // with which the player can interact
        Dictionary<int, GameObject> m_gameObjects;
        // this list contains all game objects of the level
        // which are static (e.g. platforms)
        List<GameObject> m_staticObjects;

        public Level( UpdateInfo info, Camera camera )
        {
            m_updateInfo = info;
            m_camera = camera;
            m_gameObjects = ObjectParser.ParseObjects();
            m_staticObjects = ObjectParser.ParseStatics();
        }

        public void Load(ContentManager manager)
        {
            foreach (var gameObject in m_gameObjects)
                gameObject.Value.Load(manager);
            foreach (GameObject staticObject in m_staticObjects)
                staticObject.Load(manager);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var gameObject in m_gameObjects)
            {
                gameObject.Value.Update();
            }
        }

        public void Draw()
        {
            foreach (GameObject staticObject in m_staticObjects)
            {
                staticObject.Draw(m_camera.getviewMatrix(), m_camera.getProjectionMatrix());
            }
            foreach (var gameObject in m_gameObjects)
            {
                gameObject.Value.Draw(m_camera.getviewMatrix(), m_camera.getProjectionMatrix());
            }
        }
    }
}
