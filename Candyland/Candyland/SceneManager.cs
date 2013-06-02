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
        Dictionary<string, Area> m_areas;

        // the update info, this object is used for communication
        UpdateInfo m_updateInfo;

        // the player
        CandyGuy player;

        // graphics device needed for drawing the bounding boxes
        GraphicsDevice m_graphics;

        InputManager m_inputManager;

        public SceneManager(GraphicsDevice graphics, GraphicsDeviceManager graphicDeviceManager)
        {
            m_inputManager = new InputManager(0, graphicDeviceManager);

            m_updateInfo = new UpdateInfo(graphics);

            m_graphics = graphics;

            player = new CandyGuy(new Vector3(0,0.2f,0), Vector3.Up, 1.0f,m_updateInfo);

            m_areas = AreaParser.ParseAreas(m_updateInfo);
        }

        public void Load(ContentManager manager)
        {
            foreach (var area in m_areas)
                area.Value.Load(manager);

            player.load(manager);
        }

        public void Update(GameTime gameTime)
        {

            m_inputManager.movePlayable(player, GamePad.GetState(0), Mouse.GetState(), Keyboard.GetState());

            // check for Collision between the Player and all Game Objects in the current Level
            m_areas[m_updateInfo.currentAreaID].Collide(player);  

            // check for Collision between all Objects in the currentObjectsToBeCollided List inside UpdateInfo
            Dictionary<String, GameObject> currentObjectsToBeCollided = m_updateInfo.currentObjectsToBeCollided;
            foreach (var obj in currentObjectsToBeCollided )
                m_areas[m_updateInfo.currentAreaID].Collide(obj.Value); 

            // update the area the player currently is in
            // and the next area if the player is about to leave the current area
            m_areas[m_updateInfo.currentAreaID].Update(gameTime);
            if (m_updateInfo.playerIsOnAreaExit)
                m_areas[m_updateInfo.areaAfterExitID].Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            player.draw();


            // draw the area the player currently is in and the two
            // adjacent ones
            string currentArea = m_updateInfo.currentAreaID;
            Area currArea = m_areas[currentArea];
            currArea.Draw(m_graphics);
            if (m_areas[currentArea].hasPrevious)
                m_areas[currArea.previousID].Draw(m_graphics);
            if (m_areas[currentArea].hasNext)
                m_areas[currArea.nextID].Draw(m_graphics);
        }
    }
}
