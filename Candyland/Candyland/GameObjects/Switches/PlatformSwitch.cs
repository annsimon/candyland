using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

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

        protected SoundEffect m_errorSound;

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
        protected bool belongsToOrdered = false;
        public void setGroup( SwitchGroup group )
        {
            if (group is OrderedSwitchGroup)
                belongsToOrdered = true;
            m_switchGroups.Add(group);
        }

        #endregion

        #region initialization
        public override void load(ContentManager content, AssetManager assets)
        {
            m_errorSound = assets.menuButtonError;
            base.load(content, assets);
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

        public virtual void playActivated(bool direct)
        {
        }

        public virtual void playDeactivated(bool direct)
        {
        }

        public virtual void playError()
        {
            float pitch = 0.0f;
            float pan = 0.0f;
            m_errorSound.Play(((float)m_updateInfo.soundVolume) / 15, pitch, pan);
        }

    }
}
