using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    /// <summary>
    /// Switch is only activated, when a Player (or an Obstacle) is standing on the Platform.
    /// </summary>
    class PlatformSwitchTemporary : PlatformSwitch
    {
        public PlatformSwitchTemporary(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            initialize(id, pos, updateInfo);
        }

        #region initialization

        protected void initialize(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            base.init(id, pos, updateInfo);
            this.isActivated = false;
            this.m_switchGroups = new List<SwitchGroup>();
        }

        public override void load(ContentManager content)
        {
            this.m_activated_texture = content.Load<Texture2D>("schaltertextur");
            this.m_notActivated_texture = content.Load<Texture2D>("schaltertexturinaktiv");
            this.m_texture = this.m_notActivated_texture;
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Toon");
            this.m_model = content.Load<Model>("plattform");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
        }

        #endregion


        /// <summary>
        /// Updates the Switch's states.
        /// </summary>
        public override void update()
        {
            if (this.isTouched)
            {
                // Activate when touch occurs and was deactivated before
                if(!this.isActivated)
                {
                    this.setActivated(true);
                    this.m_texture = m_activated_texture;
                }
            }
            // Deactivate when not touched
            else if(this.isActivated && !this.isTouched)
            {
                this.setActivated(false);
                this.m_texture = m_notActivated_texture;
            }
        }

    }
}
