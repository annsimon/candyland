using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    /// <summary>
    /// Platform, that works as a switch, when the Player steps on it.
    /// </summary>
    abstract class PlatformSwitch : Platform
    {
        protected Texture2D m_activated_texture;
        protected Texture2D m_notActivated_texture;
        protected bool isActivated;
        public bool getActivated() { return this.isActivated; }
        public void setActivated(bool value) 
        { 
            this.isActivated = value;
            try{
                foreach( SwitchGroup grp in m_switchGroups )
                    grp.Changed();
            }
            catch{}
        }

        protected bool isTouched = false;
        public bool getTouched() { return this.isTouched; }
        public void setTouched(bool value) { this.isTouched = value; }

        protected List<SwitchGroup> m_switchGroups;
        public void setGroup( SwitchGroup group )
        {
            m_switchGroups.Add(group);
        }


        public override void hasCollidedWith(GameObject obj)
        {
            // Position of the collided object (and therefore it's middle) is on the switch
            if (obj.getPosition().X < m_boundingBox.Max.X
                && obj.getPosition().X > m_boundingBox.Min.X
                && obj.getPosition().Z < m_boundingBox.Max.Z
                && obj.getPosition().Z > m_boundingBox.Min.Z)
            {
                isTouched = true;
            }
            else
                isTouched = false;
        }

        public override void Reset()
        {
            isActivated = false;
            isTouched = false;
            m_texture = m_notActivated_texture;
            base.Reset();
        }

    }
}
