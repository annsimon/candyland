using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Candyland
{
    class GameConstants
    {
        /// <summary>
        /// Y Coordinate for Lower Boundary of the Game World
        /// </summary>
        public const float endOfWorld_Y = -4.0f;

        /// <summary>
        /// Gravity of all dynamic Objects
        /// </summary>
        public const float gravity = -0.004f;

        /// <summary>
        /// speed of moving obstacles
        /// </summary>
        public const float obstacleSpeed = 0.1f;

        /// <summary>
        /// time a timed switch stays activated
        /// </summary>
        public const double switchActiveTime = 15;

        /// <summary>
        /// bounding box rendering on/off
        /// </summary>
        public const bool boundingBoxRendering = false;
        public const bool singlestepperOFF = true;
        public const int framerate = 1;

        public const int inputManagerMode = InputManager.KEYBOARDMOUSE;

        // data regarding the scene
        public const string sceneFile = "Content\\Scene.xml";
        public const string startAreaID = "3";
        public const string startLevelID = "3.0";
    }
}
