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
        public Area(string id, Vector3 area_start, UpdateInfo info, string xml, BonusTracker bonusTracker )
        {
            this.id = id;
            m_updateInfo = info;
            this.start = area_start;

            m_levels = LevelParser.ParseLevels(xml, start, info, bonusTracker);
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
            if( m_levels.ContainsKey(m_updateInfo.currentLevelID) )
                m_levels[m_updateInfo.currentLevelID].Update(gameTime);
            if (m_updateInfo.playerIsOnLevelExit && m_levels.ContainsKey(m_updateInfo.levelAfterExitID))
                m_levels[m_updateInfo.levelAfterExitID].Update(gameTime);
        }

        public void Draw(GraphicsDevice graphics)
        {
            foreach (var lvl in m_levels)
            {
                lvl.Value.Draw(graphics);
            }
        }

        public void Collide(GameObject obj)
        {
            if (m_levels.ContainsKey(m_updateInfo.currentLevelID))
                m_levels[m_updateInfo.currentLevelID].Collide(obj);
            if (m_levels.ContainsKey(m_updateInfo.levelAfterExitID))
                m_levels[m_updateInfo.levelAfterExitID].Collide(obj);
        }

        public Vector3 GetPlayerStartingPosition()
        {
            return m_levels[m_updateInfo.currentLevelID].getPlayerStartingPosition();
        }

        public Vector3 GetCompanionStartingPosition()
        {
            return m_levels[m_updateInfo.currentLevelID].getCompanionStartingPosition();
        }

        public void Reset()
        {
            foreach (var lvl in m_levels)
                lvl.Value.Reset();
        }

        public void Load()
        {
            foreach (var lvl in m_levels)
                lvl.Value.Load();
        }
    }
}
