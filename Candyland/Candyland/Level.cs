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
        public Vector3 start { get; set; }

        UpdateInfo m_updateInfo;
        // this dictionary contains all game objects of the level
        // with which the player can interact
        Dictionary<string, GameObject> m_gameObjects;
        // this list contains all game objects of the level
        // which are static (e.g. platforms)
        List<GameObject> m_staticObjects;

        public Level( string id, Vector3 level_start, UpdateInfo info, string xml )
        {
            m_updateInfo = info;
            this.start = level_start;
            m_gameObjects = ObjectParser.ParseObjects(level_start, xml, info);
            m_staticObjects = ObjectParser.ParseStatics(level_start, xml, info);
        }

        public void Load(ContentManager manager)
        {
            foreach (var gameObject in m_gameObjects)
                gameObject.Value.load(manager);
            foreach (GameObject staticObject in m_staticObjects)
                staticObject.load(manager);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var gameObject in m_gameObjects)
            {
                gameObject.Value.update();
            }
        }

        public void Draw(GraphicsDevice graphics)
        {
            foreach (GameObject staticObject in m_staticObjects)
            {
                staticObject.draw();
            }
            foreach (var gameObject in m_gameObjects)
            {
                gameObject.Value.draw();
            }
        }

        public void Collide(GameObject obj)
        {
            foreach (GameObject staticObject in m_staticObjects)
            {
                obj.collide(staticObject);
            }
            foreach (var gameObject in m_gameObjects)
            {
                obj.collide(gameObject.Value);
            }
        }
    }
}
