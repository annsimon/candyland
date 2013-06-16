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

        public PlatformTeleporter(String id, Vector3 pos, UpdateInfo updateInfo, Vector3 target)
        {

        }
            

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
    }
}
