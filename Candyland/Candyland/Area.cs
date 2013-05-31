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
        // every level can be accessed by its id
        Dictionary<int, Level> m_levels;

        // the update info, this object is used for communication
        UpdateInfo m_updateInfo;
        public Area(UpdateInfo info, Camera camera)
        {
            m_updateInfo = info;

            m_levels = LevelParser.ParseLevels(info, camera);
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
            m_levels[m_updateInfo.currentLevelID].Update(gameTime);
            if (m_updateInfo.playerIsOnLevelExit)
                m_levels[m_updateInfo.levelAfterExitID].Update(gameTime);
        }

        public void Draw(GraphicsDevice graphics)
        {
            foreach (var lvl in m_levels)
            {
                lvl.Value.Draw(graphics);
            }
        }
    }
}
