using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Candyland
{
    class PlatformSwitchTimed : PlatformSwitch
    {
        protected double m_activeTime;

        public PlatformSwitchTimed(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            initialize(id, pos, updateInfo, visible);
        }

        #region initialization

        public void initialize(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            base.init(id, pos, updateInfo, visible);

            this.isActivated = false;
            this.m_switchGroups = new List<SwitchGroup>();
            m_activeTime = 0;
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
            // Needed here?
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
        }

        #endregion


        /// <summary>
        /// Updates the Switch's states.
        /// </summary>
        public override void update()
        {
            if (this.isActivated)
                m_activeTime += m_updateInfo.gameTime.ElapsedGameTime.TotalSeconds;

            // Activate when first touch occurs
            if (this.isTouched == GameConstants.TouchedState.touched)
            {
                if (!this.isActivated)
                {
                    this.setActivated(true);
                    m_activeTime = 0;
                    this.isTouched = GameConstants.TouchedState.stillTouched;
                }
            }
            // Deactivate when timeout
            if ((m_activeTime > GameConstants.switchActiveTime))
            {
                this.setActivated(false);
            }
        }

    }
}
