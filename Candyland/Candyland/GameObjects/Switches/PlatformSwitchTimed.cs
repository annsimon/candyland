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
        protected double activeTime;

        public PlatformSwitchTimed(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            initialize(id, pos, updateInfo);
        }

        #region initialization

        public void initialize(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            base.init(id, pos, updateInfo);

            this.isActivated = false;
            this.m_switchGroups = new List<SwitchGroup>();
            activeTime = 0;
        }

        public override void load(ContentManager content)
        {
            this.m_model = content.Load<Model>("plattformschalter");
            this.calculateBoundingBox();
        }

        #endregion


        /// <summary>
        /// Updates the Switch's states.
        /// </summary>
        public override void update()
        {
            if (this.isActivated)
                activeTime += m_updateInfo.gameTime.ElapsedGameTime.TotalSeconds;

            // Activate when first touch occurs
            if (!this.isActivated && this.isTouched)
            {
                this.setActivated(true);
                activeTime = 0;
            }
            // Deactivate when touch ends or timeout
            if ((activeTime > GameConstants.switchActiveTime) || this.isActivated && !this.isTouched)
            {
                this.setActivated(false);
            }
        }

    }
}
