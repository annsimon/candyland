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
        #region properties

        protected Texture2D m_activated_texture;
        protected Texture2D m_notActivated_texture;

        protected GameConstants.TouchedState isTouched = GameConstants.TouchedState.notTouched;
        public GameConstants.TouchedState getTouched() { return this.isTouched; }
        public void setTouched(GameConstants.TouchedState value) { this.isTouched = value; }

        protected bool isActivated;
        public bool getActivated() { return this.isActivated; }
        public void setActivated(bool activated)
        {
            this.isActivated = activated;
            if (activated)
            {
                this.m_texture = m_activated_texture;
                m_modelTextures[-1] = m_texture;
            }
            else
            {
                this.m_texture = m_notActivated_texture;
                m_modelTextures[-1] = m_texture;
            }
            try
            {
                foreach (SwitchGroup grp in m_switchGroups)
                    grp.Changed( this );
            }
            catch { }
        }

        protected List<SwitchGroup> m_switchGroups;
        public void setGroup( SwitchGroup group )
        {
            m_switchGroups.Add(group);
        }

        #endregion

        #region collision related

        public override void hasCollidedWith(GameObject obj)
        {
            // Position of the collided object (and therefore it's middle) is on the switch
            if (obj.getPosition().X < m_boundingBox.Max.X
                && obj.getPosition().X > m_boundingBox.Min.X
                && obj.getPosition().Z < m_boundingBox.Max.Z
                && obj.getPosition().Z > m_boundingBox.Min.Z)
            {
                if (isTouched == GameConstants.TouchedState.notTouched)
                    isTouched = GameConstants.TouchedState.touched;
                else
                    isTouched = GameConstants.TouchedState.stillTouched;
            }
            else if (isTouched == GameConstants.TouchedState.notTouched)
                isTouched = GameConstants.TouchedState.notTouched;
        }

        #endregion

        public override void Reset()
        {
            isActivated = false;
            isTouched = GameConstants.TouchedState.notTouched;
            m_texture = m_notActivated_texture;
            m_modelTextures[-1] = m_notActivated_texture;
            base.Reset();
        }

        public void setInactive()
        {
            isActivated = false;
            isTouched = GameConstants.TouchedState.notTouched;
            m_texture = m_notActivated_texture;
            m_modelTextures[-1] = m_notActivated_texture;
        }

    }
}
