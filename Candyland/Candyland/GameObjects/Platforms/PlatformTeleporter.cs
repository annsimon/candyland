using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Candyland
{
    class PlatformTeleporter : Platform
    {
        protected Vector3 teleportTarget;

        public PlatformTeleporter(String id, Vector3 pos, UpdateInfo updateInfo, bool visible, Vector3 target)
        {
            initialize(id, pos, updateInfo, visible, target);
        }

        #region initialization

        public void initialize(String id, Vector3 pos, UpdateInfo updateInfo, bool visible, Vector3 target)
        {
            base.init(id, pos, updateInfo, visible);

            this.teleportTarget = target;
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
                obj.setPosition(teleportTarget);
            }
        }

        #endregion
    }
}
