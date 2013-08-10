using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Candyland
{
    public class GameConstants
    {
        /// <summary>
        /// Y Coordinate for Lower Boundary of the Game World
        /// </summary>
        public const float endOfWorld_Y = -8.0f;

        /// <summary>
        /// Gravity of all dynamic Objects
        /// </summary>
        public const float gravity = -0.004f;

        /// <summary>
        /// speed of moving obstacles
        /// </summary>
        public const float obstacleSpeed = 0.05f;

        /// <summary>
        /// time a timed switch stays activated
        /// </summary>
        public const double switchActiveTime = 4;

        /// <summary>
        /// state a switch is in
        /// </summary>
        public enum TouchedState
        {
            notTouched = 0,
            touched = 1,
            stillTouched = 2
        }

        public enum SubActionType
        {
            appear = 0,
            movement = 1,
            dialog = 2,
            disappear = 3
        }

        /// <summary>
        /// bounding box rendering on/off
        /// </summary>
        public const bool boundingBoxRendering = false;
        public const bool singlestepperOFF = true;
        public const int framerate = 1;

        public const int inputManagerMode = InputManager.KEYBOARDMOUSE;

        // data regarding the scene
        //public const string sceneFile = "Content\\sceneNew.xml";
        public const string sceneFile = "Content\\sceneNew.xml";
        //public const string sceneFile = "Content\\HoehlenTest.xml";
        //public const string sceneFile = "Content\\SceneSchieberaetselPrototyp.xml.xml";
        public const string eventFile = "Content\\EventTest.xml";
        public const string actionsFile = "Content\\ActionTest.xml";
        public const string startAreaID = "15";
        public const string startLevelID = "15.0";
        //public const string startAreaID = "66";
        //public const string startLevelID = "66.0";
        //public const string startAreaID = "6";
        //public const string startLevelID = "6.0";
        //public const string startAreaID = "7";
        //public const string startLevelID = "7.0";


        /// <summary>
        /// Strings
        /// </summary>
        /// 
        // Dialogue
        public const string tradesmanGreeting = "Ah, ein Kunde. Guten Tag, Reisender! Womit kann ich behilflich sein? Kann ich dich fuer meine Waren begeistern oder suchst du eine Transportmoeglichkeit?";
    }
}
