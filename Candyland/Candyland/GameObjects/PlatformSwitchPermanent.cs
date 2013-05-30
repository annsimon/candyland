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
    /// Once stepped on this Switch, it stays activated or deactivated.
    /// </summary>
    class PlatformSwitchPermanent : PlatformSwitch
    {
        public PlatformSwitchPermanent(String id, Vector3 pos)
        {
            this.ID = id;
            this.position = pos;
            this.isActive = false;
            this.isActivated = false;
        }

        public override void load(ContentManager content)
        {
            this.model = content.Load<Model>("plattformschalter");
            this.calculateBoundingBox();
        }


        /// <summary>
        /// Updates the Switch's states.
        /// </summary>
        public override void update()
        {
            // TODO Decide when switch is being touched

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
