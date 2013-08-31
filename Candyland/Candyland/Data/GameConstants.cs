using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
        public const float slippingSpeed = 0.03f;

        /// <summary>
        /// time a timed switch stays activated
        /// </summary>
        public const double switchActiveTime = 4;

        /// <summary>
        /// time a breakable plattform takes for breaking
        /// </summary>
        public const double breakTime = 2;

        // material params
        public static Vector4 ambient = new Vector4(0.8f, 0.8f, 0.8f, 1.0f);
        public static Vector4 diffuse = new Vector4(0.4f, 0.4f, 0.4f, 1.0f);
        public static Vector4 specular = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
        public static float shiny = 0;

        public static float depthBias = 0.00013f;

        /// <summary>
        /// state a switch is in
        /// </summary>
        public enum TouchedState
        {
            notTouched = 0,
            touched = 1,
            stillTouched = 2
        }

        /// <summary>
        /// distance a movable obstacle will move when pushed
        /// </summary>
        public const float obstacleMoveDistance = 0.1f;

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

        public const float cameraFarPlane = 70;

        public const int inputManagerMode = InputManager.KEYBOARDMOUSE;

        // data regarding the scene
        public const bool helperAvailableAtGameStart = false;

        public const string sceneFile = @"Content\Scenes\theWorld.xml";
        public const string eventFile = @"Content\Scenes\Events\Event.xml";
        public const string actionsFile = @"Content\Scenes\Actions\Action.xml";
        public const string startAreaID = "5";
        public const string startLevelID = "5.0";

        //public const string sceneFile = @"Content\Scenes\theWorld.xml";
        //public const string eventFile = @"Content\Scenes\Events\Event.xml";
        //public const string actionsFile = @"Content\Scenes\Actions\Action.xml";
        //public const string startAreaID = "0";
        //public const string startLevelID = "0.Tutorial";



        /// <summary>
        /// Background color for full screen menu screens
        /// </summary>
        public static Color BackgroundColorMenu = Color.Peru;
        public static Color BackgroundColorLoading = new Color(Color.Peru.R - 40, Color.Peru.G - 40, Color.Peru.B - 40);

        public static Color backgroundColor = new Color(86,131, 227);

        /// <summary>
        /// Strings
        /// </summary>
        /// 
        // Dialogue
        public const string tradesmanGreeting = "Ah, ein Kunde. Guten Tag, Reisender! Womit kann ich behilflich sein? Kann ich dich für meine Waren begeistern oder suchst du eine Transportmöglichkeit? Oder möchtest du ein paar Neuigkeiten über Candyland hören? Ich höre viel und rede gern :) Hast du z.B. schon von der neuen Bedrohung durch die größenwahnsinnige Lakritze gehört?";
    }
}
