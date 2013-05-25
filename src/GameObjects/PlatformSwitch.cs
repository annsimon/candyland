using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Candyland
{
    /// <summary>
    /// Platform, that works as a switch, when the Player steps on it.
    /// </summary>
    class PlatformSwitch : Platform
    {
        String ID;
        bool isActivated;

        public PlatformSwitch()
        {
        }
    }
}
