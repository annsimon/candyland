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
            
            MediaPlayer.Volume = ((float)m_updateInfo.musicVolume) / 10;

            if (m_updateInfo.activateHelperNow)
            {
                m_updateInfo.currenthelperAreaID = m_updateInfo.currentguyAreaID;
                m_updateInfo.currenthelperLevelID = m_updateInfo.currentguyLevelID;
                m_updateInfo.nexthelperLevelID = m_updateInfo.nextguyLevelID;
                Vector3 resetPos2 = m_areas[m_updateInfo.currenthelperLevelID.Split('.')[0]].GetCompanionStartingPosition(player2);
                resetPos2.Y -= 0.42f;
                player2.setPosition(resetPos2);
                m_updateInfo.helperavailable = true;
                m_updateInfo.activateHelperNow = false;
            }
            if (m_updateInfo.loseHelperNow)
            {
                m_updateInfo.helperavailable = false;
                m_updateInfo.loseHelperNow = false;
            }

            if (m_updateInfo.reset)
            {

                m_updateInfo.actionInProgress = false;
                m_updateInfo.locked = false;

                m_updateInfo.finaledistance = false;
                m_updateInfo.alwaysRun = false;
                distanceToBoss = 0.0f;

                // reset player to start position of current level
                if (m_updateInfo.candyselected || m_updateInfo.currentguyLevelID == m_updateInfo.currenthelperLevelID)
                {
                    player.Reset();
                    Vector3 resetPos = m_areas[m_updateInfo.currentguyLevelID.Split('.')[0]].GetPlayerStartingPosition(player);
                    resetPos.Y += 0.6f;
                    player.setPosition(resetPos);
                }

                if (!m_updateInfo.candyselected || ( m_updateInfo.helperavailable && m_updateInfo.currentguyLevelID == m_updateInfo.currenthelperLevelID))
                {
                    player2.Reset();
                    Vector3 resetPos2 = m_areas[m_updateInfo.currenthelperLevelID.Split('.')[0]].GetCompanionStartingPosition(player2);
                    resetPos2.Y += 0.3f;
                    player2.setPosition(resetPos2);
                }


                // reset world *!*| MAYBE NOT NEEDED |*!*
                /* foreach (var area in m_areas)
                     area.Value.Reset();*/
                if (m_updateInfo.candyselected)
                    m_areas[m_updateInfo.currentguyLevelID.Split('.')[0]].Reset(player);

                if (!m_updateInfo.candyselected)
                    m_areas[m_updateInfo.currenthelperLevelID.Split('.')[0]].Reset(player2);

                m_updateInfo.reset = false;
            }

            m_inputManager.update(player, player2);

            if (m_updateInfo.candyselected || m_updateInfo.currentguyLevelID == m_updateInfo.currenthelperLevelID)
            player.update();
            if(!m_updateInfo.candyselected || (m_updateInfo.helperavailable && m_updateInfo.currentguyLevelID == m_updateInfo.currenthelperLevelID))
            player2.update();

            if( m_updateInfo.candyselected )
                sun.Update(m_graphics, player.getPosition(), gameTime);
            else
                sun.Update(m_graphics, player2.getPosition(), gameTime);


            player.resetCloseEnoughToInteract();

            player.startIntersection();
            player2.startIntersection();


            // check for Collision between the Player and all Game Objects in the current Level
            if (m_updateInfo.candyselected || m_updateInfo.currentguyLevelID == m_updateInfo.currenthelperLevelID)
            {
                string currArea = m_updateInfo.currentguyLevelID.Split('.')[0];
                m_areas[currArea].Collide(player);
                if (m_updateInfo.playerIsOnAreaExit && m_updateInfo.nextguyLevelID != null)
                {
                    string nextArea = m_updateInfo.nextguyLevelID.Split('.')[0];
                    if( !currArea.Equals(nextArea) )
                        m_areas[nextArea].Collide(player);
                }
            }
            // check for Collision between the Player2 and all Game Objects in the current Level
            if(!m_updateInfo.candyselected || (m_updateInfo.helperavailable && m_updateInfo.currentguyLevelID == m_updateInfo.currenthelperLevelID))
            {
                string currArea = m_updateInfo.currenthelperLevelID.Split('.')[0];
                m_areas[currArea].Collide(player2);
                if (m_updateInfo.playerIsOnAreaExit && m_updateInfo.nexthelperLevelID != null)
                {
                    string nextArea = m_updateInfo.nexthelperLevelID.Split('.')[0];
                    if (!currArea.Equals(nextArea))
                        m_areas[nextArea].Collide(player2);
                }
            }


            // update the area the player currently is in
            // and the next area if the player is about to leave the current area
            if (m_updateInfo.candyselected)
            {
                string currArea = m_updateInfo.currentguyLevelID.Split('.')[0];
                m_areas[currArea].Update(gameTime);
                if (m_updateInfo.playerIsOnAreaExit && m_updateInfo.nextguyLevelID != null)
                {
                    string nextArea = m_updateInfo.nextguyLevelID.Split('.')[0];
                    if (!currArea.Equals(nextArea))
                        m_areas[nextArea].Update(gameTime);
                }
            }
            if(!m_updateInfo.candyselected)
            {
                string currArea = m_updateInfo.currenthelperLevelID.Split('.')[0];
                m_areas[currArea].Update(gameTime);
                if (m_updateInfo.playerIsOnAreaExit && m_updateInfo.nexthelperLevelID != null)
                {
                    string nextArea = m_updateInfo.nexthelperLevelID.Split('.')[0];
                    if (!currArea.Equals(nextArea))
                        m_areas[nextArea].Update(gameTime);
                }
            }

            player.endIntersection();
            player2.endIntersection();

            m_areas[m_updateInfo.currentguyLevelID.Split('.')[0]].endIntersection();
            m_areas[m_updateInfo.currenthelperLevelID.Split('.')[0]].endIntersection();

            // only update if in use
            if( m_updateInfo.shadowQuality != 0 )
                UpdateShadowMap();

            if( m_updateInfo.alwaysRun )
            {
                float dx = m_updateInfo.bossPosition.X - player.getPosition().X;
                float dz = m_updateInfo.bossPosition.Z - player.getPosition().Z;
                float dy = m_updateInfo.bossPosition.Y - player.getPosition().Y;
                distanceToBoss = (float)Math.Sqrt(dx * dx + dz * dz + dy * dy);

                if (distanceToBoss > 16)
                {
                    m_updateInfo.m_screenManager.ActivateNewScreen(new DialogListeningScreen("Hahaha, ich hab's dir doch gesagt. Du holst mich nicht mehr ein. Pech gehabt!", "Boss"));
                    m_updateInfo.reset = true;
                }
            }

            if (m_updateInfo.finaledistance) {
                distanceToBoss = (m_updateInfo.bossPosition - m_updateInfo.bossTarget).Length();
                if (distanceToBoss < 1)
                {
                    m_updateInfo.m_screenManager.ActivateNewScreen(new DialogListeningScreen("Das wars fuer dich, jetzt steht der Entstehung von Lakritzland nichts mehr im Wege!", "Boss"));
                    m_updateInfo.reset = true;
                }
            }

            
            if (((m_updateInfo.alwaysRun) || (m_updateInfo.currentguyAreaID == "255")) && !boss)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(song2); 
                MediaPlayer.IsRepeating = true;
                boss = true;
            }
            else if (boss && (!m_updateInfo.alwaysRun && !(m_updateInfo.currentguyAreaID == "255")))
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(song1);
                MediaPlayer.IsRepeating = true;
                boss = false;
            }
        }

        private void UpdateShadowMap()
        {
            Vector3 playerPos;
            if (m_updateInfo.candyselected)
                playerPos = player.getPosition();
            else
                playerPos = player2.getPosition();
            m_shadowMap.Update(m_globalLight.direction, playerPos);
        }
    }
}
