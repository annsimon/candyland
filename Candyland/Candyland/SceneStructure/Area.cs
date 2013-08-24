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

        public void Update(GameTime gameTime, Playable player, Playable player2)
        {
            // update the level the player currently is in
            // and the next level if the player is about to leave the current level
            if( m_levels.ContainsKey(player.getCurrentLevelId()) )
                m_levels[player.getCurrentLevelId()].Update(gameTime);
            if (m_updateInfo.playerIsOnLevelExit && player.getNextLevelId() != null && m_levels.ContainsKey(player.getNextLevelId()))
                m_levels[player.getNextLevelId()].Update(gameTime);

            if(player.getCurrentLevelId() != player2.getCurrentLevelId()){
                if (m_levels.ContainsKey(player2.getCurrentLevelId()))
                    m_levels[player2.getCurrentLevelId()].Update(gameTime);
                if (m_updateInfo.playerIsOnLevelExit && player2.getNextLevelId() != null && m_levels.ContainsKey(player2.getNextLevelId()))
                    m_levels[player2.getNextLevelId()].Update(gameTime);

            }
        }

        public void Collide(GameObject obj)
        {
            if (m_levels.ContainsKey(((Playable)obj).getCurrentLevelId()))
                m_levels[((Playable)obj).getCurrentLevelId()].Collide(obj);
            if (((Playable)obj).getNextLevelId() != null&& m_levels.ContainsKey(((Playable)obj).getNextLevelId()))
                m_levels[((Playable)obj).getNextLevelId()].Collide(obj);
        }

        public Vector3 GetPlayerStartingPosition(Playable player)
        {
            return m_levels[player.getCurrentLevelId()].getPlayerStartingPosition();
        }

        public Vector3 GetCompanionStartingPosition(Playable player2)
        {
            return m_levels[player2.getCurrentLevelId()].getCompanionStartingPosition();
        }

        public List<GameObject> GetObjects()
        {
            return m_allObjects;
        }

        public void Reset()
        {
            foreach (var lvl in m_levels)
                lvl.Value.Reset();
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
