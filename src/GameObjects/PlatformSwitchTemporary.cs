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
    /// Switch is only activated, when a Player is standing on the Platform.
    /// </summary>
    class PlatformSwitchTemporary : PlatformSwitch
    {
        public PlatformSwitchTemporary(String id, Vector3 pos)
        {
            this.ID = id;
            this.Position = pos;
            this.isActive = false;
            this.isActivated = false;
        }

        public override void Load(ContentManager content)
        {
            this.Model = content.Load<Model>("plattformschalter");
            this.BoundingBox = calculateBoundingBox(this.Model, this.Position);
        }


        /// <summary>
        /// Updates the Switch's states.
        /// </summary>
        public override void Update()
        {
            // TODO Decide when switch is being touched

            
            if (this.isTouched)
            {
                // Activate when touch occurs and was deactivated before
                if(!this.isActivated)
                    this.setActivated(true);
                // Deactivate when touch occurs and was activated before
                else
                    this.setActivated(false);
            }
        }


    }
}
