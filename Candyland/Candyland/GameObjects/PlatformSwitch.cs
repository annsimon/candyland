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
        protected bool isActivated;
        public bool getActivated() { return this.isActivated; }
        public void setActivated(bool value) 
        { 
            this.isActivated = value;
            m_switchGroup.Changed();
        }

        protected bool isTouched;
        public bool getTouched() { return this.isTouched; }
        public void setTouched(bool value) { this.isTouched = value; }

        protected SwitchGroup m_switchGroup;
        public void setGroup( SwitchGroup group )
        {
            m_switchGroup = group;
        }

    }
}
