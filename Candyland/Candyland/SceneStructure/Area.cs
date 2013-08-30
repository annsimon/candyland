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
    /// This encapsulates several levels.
    /// </summary>
    public class Area
    {
        public string id { get; set; }
        public bool hasPrevious { get; set; }
        public string previousID { get; set; }
        public bool hasNext { get; set; }
        public string nextID { get; set; }
        public Vector3 start { get; set; }

        // every level can be accessed by its id
        Dictionary<string, Level> m_levels;

        // the update info, this object is used for communication
        UpdateInfo m_updateInfo;

        List<GameObject> m_allObjects;

        public Area(string id, Vector3 area_start, UpdateInfo info, string xml, BonusTracker bonusTracker, ActionTracker actionTracker )
        {
            this.id = id;
            m_updateInfo = info;
            this.start = area_start;

            m_levels = LevelParser.ParseLevels(xml, start, info, bonusTracker, actionTracker);

            m_allObjects = new List<GameObject>();
            foreach (var level in m_levels)
                m_allObjects.AddRange(level.Value.getObjects());
        }
        
        public void Load(ContentManager manager)
        {
            foreach (var lvl in m_levels)
                lvl.Value.Load(manager);
        }

        public void Update(GameTime gameTime)
        {
            // update the level the player currently is in
            // and the next level if the player is about to leave the current level
            if( m_levels.ContainsKey(m_updateInfo.currentguyLevelID) )
                m_levels[m_updateInfo.currentguyLevelID].Update(gameTime);
            if (m_updateInfo.playerIsOnLevelExit && m_updateInfo.nextguyLevelID != null && m_levels.ContainsKey(m_updateInfo.nextguyLevelID))
                m_levels[m_updateInfo.nextguyLevelID].Update(gameTime);

            if (m_updateInfo.currentguyLevelID != m_updateInfo.currenthelperLevelID)
            {
                if (m_levels.ContainsKey(m_updateInfo.currenthelperLevelID) && m_updateInfo.currenthelperLevelID != m_updateInfo.nextguyLevelID)
                    m_levels[m_updateInfo.currenthelperLevelID].Update(gameTime);
                if (m_updateInfo.playerIsOnLevelExit && m_updateInfo.nexthelperLevelID != null && m_levels.ContainsKey(m_updateInfo.nexthelperLevelID)
                    && m_updateInfo.nexthelperLevelID != m_updateInfo.nextguyLevelID && m_updateInfo.nexthelperLevelID != m_updateInfo.currentguyLevelID)
                    m_levels[m_updateInfo.nexthelperLevelID].Update(gameTime);

            }
        }

        public void Collide(GameObject obj)
        {
            if (obj is CandyGuy) {

                if (m_levels.ContainsKey(m_updateInfo.currentguyLevelID))
                    m_levels[m_updateInfo.currentguyLevelID].Collide(obj);
                if (m_updateInfo.nextguyLevelID != null && m_levels.ContainsKey(m_updateInfo.nextguyLevelID))
                    m_levels[m_updateInfo.nextguyLevelID].Collide(obj);
            }
            else
            {
                if (m_levels.ContainsKey(m_updateInfo.currenthelperLevelID))
                    m_levels[m_updateInfo.currenthelperLevelID].Collide(obj);
                if (m_updateInfo.nexthelperLevelID != null && m_levels.ContainsKey(m_updateInfo.nexthelperLevelID))
                    m_levels[m_updateInfo.nexthelperLevelID].Collide(obj);
            }
        }

        public Vector3 GetPlayerStartingPosition(Playable player)
        {
                return m_levels[m_updateInfo.currentguyLevelID].getPlayerStartingPosition();   
        }

        public Vector3 GetCompanionStartingPosition(Playable player2)
        {
            return m_levels[m_updateInfo.currenthelperLevelID].getCompanionStartingPosition();
        }

        public List<GameObject> GetObjects()
        {
            return m_allObjects;
        }

        public void Reset(Playable player)
        {
            if(player is CandyGuy)
            m_levels[m_updateInfo.currentguyLevelID].Reset();
            else
                m_levels[m_updateInfo.currenthelperLevelID].Reset();
        }

        public void endIntersection()
        {
            foreach (var lvl in m_levels)
                lvl.Value.endIntersection();
        }

        public void Load()
        {
            foreach (var lvl in m_levels)
                lvl.Value.Load();
        }
    }
}
