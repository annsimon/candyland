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
        // this list contains all events of the level
        List<SwitchEvent> m_events;

        Platform m_start_player;
        Platform m_start_companion;

        public Level( string id, Vector3 level_start, UpdateInfo info, string xml, BonusTracker bonusTracker )
        {
            m_updateInfo = info;
            this.start = level_start;
            m_gameObjects = ObjectParser.ParseObjects(level_start, xml, info, bonusTracker);
            m_staticObjects = ObjectParser.ParseStatics(level_start, xml, info);
            m_events = new List<SwitchEvent>();

            try 
            {
                m_events = EventParser.ParseEvents(id, m_gameObjects);
            }
            catch(Exception e) 
            {
                e.GetType();
            }
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
                if(gameObject.Value.isVisible)
                    gameObject.Value.update();
            }

            foreach( var gameObject in m_gameObjects )
            {
                if (gameObject.Value is PlatformSwitch)
                    ((PlatformSwitch)gameObject.Value).setTouched(GameConstants.TouchedState.notTouched);
                if (gameObject.Value.isVisible)
                    Collide(gameObject.Value);
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

        public void Reset()
        {
            foreach (var gameObject in m_gameObjects)
            {
                gameObject.Value.Reset();
            }
            foreach( SwitchEvent currEvent in m_events )
            {
                currEvent.Reset();
            }
        }

        // might be called for too many objects (only those which use preventIntersection need it)
        public void endIntersection()
        {
            foreach (var gameObject in m_gameObjects)
            {
                gameObject.Value.endIntersection();
            }
        }

        // called when a savegame is being loaded to update the isCollected Attribute of the ChocoChips
        public void Load()
        {
            foreach (var gameObject in m_gameObjects)
            {
                if (gameObject.Value.GetType() == typeof(ChocoChip))
                {
                    gameObject.Value.initialize();
                }
            }
        }

        // called to set the platform the player and companion each start at (for reset)
        public void setStartPositions(string playerStartID, string companionStartID)
        {
            m_start_player = (Platform)m_gameObjects[playerStartID];
            m_start_companion = (Platform)m_gameObjects[companionStartID];
        }

        public Vector3 getPlayerStartingPosition()
        {
            return m_start_player.getPosition();
        }

        public Vector3 getCompanionStartingPosition()
        {
            return m_start_companion.getPosition();
        }
    }
}
