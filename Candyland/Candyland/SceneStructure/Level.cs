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
        // this dictionary contains all switch objects of the level
        Dictionary<string, GameObject> m_switchObjects;
        // this list contains all game objects of the level
        // which are static (e.g. platforms)
        List<GameObject> m_staticObjects;
        // this list contains all events of the level
        List<SwitchEvent> m_events;

        List<GameObject> m_allGameObjects;

        Platform m_start_player;
        Platform m_start_companion;

        public Level( string id, Vector3 level_start, UpdateInfo info, string xml, BonusTracker bonusTracker, ActionTracker actionTracker )
        {
            m_updateInfo = info;
            this.start = level_start;
            var tempDictionaryList = ObjectParser.ParseObjects(level_start, xml, info, bonusTracker, actionTracker);
            m_gameObjects = tempDictionaryList[0];
            m_switchObjects = tempDictionaryList[1];
            m_staticObjects = ObjectParser.ParseStatics(level_start, xml, info);
            m_events = new List<SwitchEvent>();

            m_allGameObjects = new List<GameObject>();
            foreach (var entry in m_gameObjects)
                m_allGameObjects.Add(entry.Value);
            foreach (GameObject o in m_staticObjects)
                m_allGameObjects.Add(o);
            foreach (var entry in m_switchObjects)
                m_allGameObjects.Add(entry.Value);

            //try 
            //{
                m_events = EventParser.ParseEvents(id, m_gameObjects, m_switchObjects);
                ActionParser.ParseActions(id, level_start, m_gameObjects, actionTracker);
           /* }
            catch(Exception e) 
            {
                e.GetType();
            }*/
        }

        public void Load(ContentManager manager, AssetManager assets)
        {
            foreach (var gameObject in m_gameObjects)
                gameObject.Value.load(manager, assets);
            foreach (var gameObject in m_switchObjects)
                gameObject.Value.load(manager, assets);
            foreach (GameObject staticObject in m_staticObjects)
                staticObject.load(manager, assets);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var gameObject in m_gameObjects)
            {
                gameObject.Value.update();
            }
            foreach (var gameObject in m_switchObjects)
            {
                gameObject.Value.update();
            }
            foreach (var gameObject in m_switchObjects)
            {
                ((PlatformSwitch)gameObject.Value).setTouched(GameConstants.TouchedState.notTouched);
            }

            foreach( var gameObject in m_gameObjects )
            {
                if (gameObject.Value.isVisible)
                    Collide(gameObject.Value);
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
            foreach (var gameObject in m_switchObjects)
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
            foreach (var gameObject in m_switchObjects)
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
            foreach (var gameObject in m_switchObjects)
            {
                gameObject.Value.endIntersection();
            }
        }

        /// <summary>
        ///  called when a savegame is being loaded to update the isCollected Attribute of the ChocoChips
        ///  and the original visibility of action actors
        /// </summary>
        public void Load()
        {
            foreach (var gameObject in m_gameObjects)
            {
                if (gameObject.Value.GetType() == typeof(ChocoChip))
                {
                    gameObject.Value.initialize();
                }
                if (gameObject.Value.GetType() == typeof(ActionActor))
                {
                    ActionActor actor = (ActionActor)gameObject.Value;
                    actor.SetVisibilityAfterLoadingSavegame();
                }
            }
            foreach (var gameObject in m_switchObjects)
            {
                if (gameObject.Value.GetType() == typeof(ChocoChip))
                {
                    gameObject.Value.initialize();
                }
            }
        }

        public List<GameObject> getObjects()
        {
            return m_allGameObjects;
        }

        // called to set the platform the player
        public void setStartPositions(string playerStartID)
        {
            m_start_player = (Platform)m_gameObjects[playerStartID];
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
