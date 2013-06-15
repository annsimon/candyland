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
        public const float obstacleSpeed = 0.2f;

        /// <summary>
        /// bounding box rendering on/off
        /// </summary>
        public const bool boundingBoxRendering = true;

        
    }
}
