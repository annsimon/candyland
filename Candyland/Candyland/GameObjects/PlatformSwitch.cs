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
        public void setActivated(bool value) { this.isActivated = value; }

        protected bool isTouched = false;
        public bool getTouched() { return this.isTouched; }
        public void setTouched(bool value) { this.isTouched = value; }


        public override void hasCollidedWith(GameObject obj)
        {
            // Position of the collided object (and therefore it's middle) is on the switch
            if (obj.getPosition().X > m_boundingBox.Max.X
                && obj.getPosition().X < m_boundingBox.Min.X
                && obj.getPosition().Y > m_boundingBox.Max.Y
                && obj.getPosition().Y < m_boundingBox.Min.Y)
            {
                isTouched = true;
            }
            else isTouched = isTouched || false; // TODO Find out what this does :)
        }

    }
}
