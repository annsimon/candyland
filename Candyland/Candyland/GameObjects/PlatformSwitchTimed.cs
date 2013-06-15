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
                public PlatformSwitchTimed(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            this.ID = id;
            this.m_position = pos;
            this.isActive = false;
            this.isActivated = false;
            this.m_updateInfo = updateInfo;
            this.m_switchGroups = new List<SwitchGroup>();
        }

        public override void load(ContentManager content)
        {
            this.m_model = content.Load<Model>("plattformschalter");
            this.calculateBoundingBox();
        }


        /// <summary>
        /// Updates the Switch's states.
        /// </summary>
        public override void update()
        {
            // TODO insert timeout

            // Activate when first touch occurs
            if (!this.isActivated && this.isTouched)
            {
                this.setActivated(true);
            }
            // Deactivate when touch ends
            if (this.isActivated && !this.isTouched)
            {
                this.setActivated(false);
            }
        }

    }
}
