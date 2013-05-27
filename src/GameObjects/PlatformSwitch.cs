using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Candyland
{
    /// <summary>
    /// Platform, that works as a switch, when the Player steps on it.
    /// </summary>
    abstract class PlatformSwitch : Platform
    {
        protected String ID;
        public String getID() { return this.ID; }

        protected bool isActivated;
        public bool getActivated() { return this.isActivated; }
        public void setActivated(bool value) { this.isActivated = value; }

        protected bool isTouched;
        public bool getTouched() { return this.isTouched; }
        public void setTouched(bool value) { this.isTouched = value; }

    }
}
