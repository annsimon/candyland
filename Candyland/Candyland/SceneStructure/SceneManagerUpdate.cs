using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace Candyland
{
    public partial class SceneManager
    {
        public void Update(GameTime gameTime)
        {
            /*
            System.Console.Out.WriteLine("currLevel = " + m_updateInfo.currentLevelID);
            if( m_updateInfo.playerIsOnLevelExit)
                System.Console.Out.WriteLine("nextLevel = " + m_updateInfo.levelAfterExitID);
            */

            // Update gameTime in UpdateInfo
            m_updateInfo.gameTime = gameTime;

            // not yet touched in this update
            m_updateInfo.guyHasTouchedDoorInThisUpdate = false;

            if (m_updateInfo.reset)
            {

                m_updateInfo.actionInProgress = false;
                m_updateInfo.helperActionInProgress = false;
                m_updateInfo.locked = false;

                m_updateInfo.finaledistance = false;
                //m_updateInfo.alwaysRun = false;

                // reset player to start position of current level
                    player.Reset();
                    Vector3 resetPos = m_areas[m_updateInfo.currentguyLevelID.Split('.')[0]].GetPlayerStartingPosition(player);
                    resetPos.Y += 0.4f;
                    player.setPosition(resetPos);


                // reset world *!*| MAYBE NOT NEEDED |*!*
                /* foreach (var area in m_areas)
                     area.Value.Reset();*/
                m_areas[m_updateInfo.currentguyLevelID.Split('.')[0]].Reset(player);

                m_updateInfo.reset = false;
            }

            m_inputManager.update(player);

            player.update();

            sun.Update(m_graphics, player.getPosition(), gameTime);

            player.resetCloseEnoughToInteract();

            player.startIntersection();


            // check for Collision between the Player and all Game Objects in the current Level
                string currArea = m_updateInfo.currentguyLevelID.Split('.')[0];
                m_areas[currArea].Collide(player);
                if (m_updateInfo.playerIsOnAreaExit && m_updateInfo.nextguyLevelID != null)
                {
                    string nextArea = m_updateInfo.nextguyLevelID.Split('.')[0];
                    if( !currArea.Equals(nextArea) )
                        m_areas[nextArea].Collide(player);
                }


            // update the area the player currently is in
            // and the next area if the player is about to leave the current area
                m_areas[currArea].Update(gameTime);
                if (m_updateInfo.playerIsOnAreaExit && m_updateInfo.nextguyLevelID != null)
                {
                    string nextArea = m_updateInfo.nextguyLevelID.Split('.')[0];
                    if (!currArea.Equals(nextArea))
                        m_areas[nextArea].Update(gameTime);
                }


            player.endIntersection();

            m_areas[m_updateInfo.currentguyLevelID.Split('.')[0]].endIntersection();

            // only update if in use
            if( m_updateInfo.shadowQuality != 0 )
                UpdateShadowMap();
        }

        public void UpdateOnce()
        {
            // Update gameTime in UpdateInfo
            GameTime gameTime = new GameTime(new TimeSpan(0), new TimeSpan(0));

            m_updateInfo.gameTime = gameTime;

            m_inputManager.update(player);

            player.update();

            sun.Update(m_graphics, player.getPosition(), gameTime);


            player.resetCloseEnoughToInteract();

            player.startIntersection();


            // check for Collision between the Player and all Game Objects
            foreach (var area in m_areas)
                area.Value.Collide(player);
            
            // update all areas
            foreach (var area in m_areas)
                area.Value.UpdateAll(gameTime);

            player.endIntersection();

            m_areas[m_updateInfo.currentguyLevelID.Split('.')[0]].endIntersection();
        }

        private void UpdateShadowMap()
        {
            Vector3 playerPos;
            playerPos = player.getPosition();
            m_shadowMap.Update(m_globalLight.direction, playerPos);
        }
    }
}
