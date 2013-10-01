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
    /// This contains all the important data regarding updates.
    /// </summary>
    public class UpdateInfo
    {
        public string currentguyAreaID;
        public string currentguyLevelID;

        public string nextguyLevelID;

        // value between 0 and 2, 0 means no shadows, 2 means best shadows
        public int shadowQuality { get; set; }

        // is tutorial enabled or not?
        public bool tutorialActive { get; set; }

        public int musicVolume { get; set; }

        public int soundVolume { get; set; }
        // if the player is on the last platform before a level change
        // the bool is true and the int tells which level exit it is
        // (this is used to start updating the next level early enough)
        public bool playerIsOnAreaExit { get; set; }

        // if the player is on the last platform before a level change
        // the bool is true and the int tells which level exit it is
        // (this is used to start updating the next area early enough)
        public bool playerIsOnLevelExit { get; set; }

        /// <summary>
        /// used to have only the first collided platform update level IDs
        /// otherwise they switch around while two a touched at the same time
        /// </summary>
        public bool guyHasTouchedDoorInThisUpdate { get; set; }

        // if this is true we are currently processing a reset
        // which moves the player to the level start position
        // and resets the dynamic elements in the level
        public bool reset { get; set; }

        // if this is true we are watching an action be performed
        // in "movie mode" and can't move (can click through a conversation though)
        // movie mode is a TODO!
        public bool locked { get; set; }

        public Matrix viewMatrix { get; set; }
        public Matrix projectionMatrix { get ; set; }

        public List<Keys> currentpushedKeys { get; set; }
        public List<GameObject> objectsWithBillboards { get; set; }

        public GameTime gameTime { get; set; }

        public ScreenManager m_screenManager { get; set; }

        public bool actionInProgress = false;
        public bool helperActionInProgress = false;
        public bool alwaysRun = false;
        public bool finaledistance = false;

        // we do not use this after all, probably; remove later!
        //public Dictionary<String, GameObject> currentObjectsToBeCollided { get; set; }

        /********************************************************************************
        Debugging Area
         * GraphicsDevice needed to render the Bounding Boxes
         * Parameter in Constructor can be removed, when rendering is no longer needed*/

        public GraphicsDevice graphics;

        /********************************************************************************/


        public UpdateInfo(GraphicsDevice graphicsDevice, ScreenManager screenManager)
        {
            currentguyAreaID = GameConstants.startAreaID;
            currentguyLevelID = GameConstants.startLevelID;

            shadowQuality = screenManager.Settings.shadowQuality;
            tutorialActive = screenManager.Settings.showTutorial;
            musicVolume = screenManager.Settings.musicVolume;
            soundVolume = screenManager.Settings.soundVolume;

            playerIsOnAreaExit = false;

            playerIsOnLevelExit = false;

            guyHasTouchedDoorInThisUpdate = false;

            reset = false;
            currentpushedKeys = new List<Keys>();

            // currently not in use, remove later!
            //currentObjectsToBeCollided = new Dictionary<String, GameObject>();

            m_screenManager = screenManager;

            /**********************************************************************/
            graphics = graphicsDevice;
            /**********************************************************************/

            objectsWithBillboards = new List<GameObject>();
        }

    }
}
