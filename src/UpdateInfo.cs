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
        public int currentAreaID { get; set; }
        public int currentLevelID { get; set; }

        // if the player is on the last platform before a level change
        // the bool is true and the int tells which level exit it is
        // (this is used to start updating the next level early enough)
        public bool playerIsOnAreaExit { get; set; }
        public int areaAfterExitID { get; set; }

        // if the player is on the last platform before a level change
        // the bool is true and the int tells which level exit it is
        // (this is used to start updating the next area early enough)
        public bool playerIsOnLevelExit { get; set; }
        public int levelAfterExitID { get; set; }


        public UpdateInfo()
        {
            currentAreaID = 0;
            currentLevelID = 0;

            playerIsOnAreaExit = false;
            areaAfterExitID = 0;

            playerIsOnLevelExit = false;
            levelAfterExitID = 0;
        }

    }
}
