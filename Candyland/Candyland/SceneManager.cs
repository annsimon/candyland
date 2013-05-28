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
    /// This class takes care of the game, this includes visual aspects (Area) and
    /// the game's logic (Communications).
    /// </summary>
    public class SceneManager
    {
        // areas is a dictionary (works like a map) that saves the area
        // every area can be accessed by its id
        Dictionary<int, Area> m_areas;

        // the update info, this object is used for communication
        UpdateInfo m_updateInfo;

        // the camera
        Camera m_gameCamera;

        public SceneManager(Camera camera)
        {
            m_gameCamera = camera;
            m_updateInfo = new UpdateInfo();

            m_areas = AreaParser.ParseAreas(m_updateInfo, m_gameCamera);
        }

        public void Load(ContentManager manager)
        {
            foreach (var area in m_areas)
                area.Value.Load(manager);
        }

        public void Update(GameTime gameTime)
        {
            // update the area the player currently is in
            // and the next area if the player is about to leave the current area
            m_areas[m_updateInfo.currentAreaID].Update(gameTime);
            if (m_updateInfo.playerIsOnAreaExit)
                m_areas[m_updateInfo.areaAfterExitID].Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            // draw the area the player currently is in and the two
            // adjacent ones
            int currentArea = m_updateInfo.currentAreaID;
            if (currentArea > 0)
                m_areas[currentArea-1].Draw();
            m_areas[currentArea].Draw();
            if (currentArea < m_areas.Count-1 )
                m_areas[currentArea + 1].Draw();
        }
    }
}
