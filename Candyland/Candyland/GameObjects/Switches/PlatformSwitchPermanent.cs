﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    /// <summary>
    /// Once stepped on this Switch, it stays activated or deactivated.
    /// </summary>
    class PlatformSwitchPermanent : PlatformSwitch
    {
        public PlatformSwitchPermanent(String id, Vector3 pos, UpdateInfo updateInfo)
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
            // Activate when first touch occurs
            if (!this.isActivated && this.isTouched)
            {
                this.setActivated(true);
                this.m_texture = m_activated_texture;
            }
        }

    }
}