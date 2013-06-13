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

        // this object is used to keep track of 
        // the ChocoChip collection and of which extras the player activated
        BonusTracker m_bonusTracker;

        // the update info, this object is used for communication
        UpdateInfo m_updateInfo;

        // the player
        CandyGuy player;
        CandyHelper player2;
        /*************************************************************/
        // graphics device needed for drawing the bounding boxes
        GraphicsDevice m_graphics;
        /*************************************************************/

        // font used for writing tests to screen
        SpriteFont screenFont;

        InputManager m_inputManager;

        public SceneManager(GraphicsDevice graphics, GraphicsDeviceManager graphicDeviceManager)
        {
           

            m_bonusTracker = new BonusTracker(); // load this one from xml as serialized object?

            m_updateInfo = new UpdateInfo(graphics);

            m_inputManager = new InputManager(0, graphicDeviceManager, m_updateInfo);
            /****************************************************************/
            m_graphics = graphics;
            /****************************************************************/

            player = new CandyGuy(new Vector3(0, 0.4f, 0), Vector3.Up,graphics.Viewport.AspectRatio, m_updateInfo, m_bonusTracker);
            player2 = new CandyHelper(new Vector3(0, 0.4f, 0.8f), Vector3.Up, graphics.Viewport.AspectRatio, m_updateInfo,m_bonusTracker);
            
            m_areas = AreaParser.ParseAreas(m_updateInfo, m_bonusTracker);
        }

        public void Load(ContentManager manager)
        {
            foreach (var area in m_areas)
                area.Value.Load(manager);

            player.load(manager);
            player2.load(manager);

            screenFont = manager.Load<SpriteFont>("MainText");
        }

        public void Update(GameTime gameTime)
        {
            System.Console.Out.WriteLine("currLevel = " + m_updateInfo.currentLevelID);
            if( m_updateInfo.playerIsOnLevelExit)
                System.Console.Out.WriteLine("nextLevel = " + m_updateInfo.levelAfterExitID);


            m_inputManager.update(player,player2);
            player.update();
            player2.update();

            player.startIntersection();
            player2.startIntersection();
            // check for Collision between the Player and all Game Objects in the current Level
            m_areas[m_updateInfo.currentAreaID].Collide(player);
            if (m_updateInfo.playerIsOnAreaExit)
                m_areas[m_updateInfo.areaAfterExitID].Collide(player);
            // check for Collision between the Player2 and all Game Objects in the current Level
            m_areas[m_updateInfo.currentAreaID].Collide(player2);
            if (m_updateInfo.playerIsOnAreaExit)
                m_areas[m_updateInfo.areaAfterExitID].Collide(player2);
            // check for Collision between all Objects in the currentObjectsToBeCollided List inside UpdateInfo
            Dictionary<String, GameObject> currentObjectsToBeCollided = m_updateInfo.currentObjectsToBeCollided;
            foreach (var obj in currentObjectsToBeCollided )
                m_areas[m_updateInfo.currentAreaID].Collide(obj.Value); 

            // update the area the player currently is in
            // and the next area if the player is about to leave the current area
            m_areas[m_updateInfo.currentAreaID].Update(gameTime);
            if (m_updateInfo.playerIsOnAreaExit)
                m_areas[m_updateInfo.areaAfterExitID].Update(gameTime);
           
            player.endIntersection();
            player2.endIntersection();
          
        }

        public void Draw(GameTime gameTime)
        {
            player.draw();
            player2.draw();

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

        public void Draw2D(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(screenFont, "Linsen: " + m_bonusTracker.chocoCount.ToString()
               + "/" + m_bonusTracker.chocoTotal.ToString(), new Vector2(5f, 5f), Color.White);
            spriteBatch.End();

            // we need the following as spriteBatch.Begin() sets them to None and AlphaBlend
            // which breaks our model rendering
            m_graphics.DepthStencilState = DepthStencilState.Default;
            m_graphics.BlendState = BlendState.Opaque;
        }
    }
}
