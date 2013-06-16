using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class ObstacleForSwitch : Obstacle
    {
        public ObstacleForSwitch(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            this.ID = id;
            this.m_position = pos;
            this.m_position.Y += 1.25f;
            this.m_original_position = this.m_position;
            this.isActive = false;
            this.m_updateInfo = updateInfo;
            m_original_position = pos;
        }

        public override void load(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("blockmovabletextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Toon");
            this.m_model = content.Load<Model>("blockmovable");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
        }


        public override void update()
        {
            // let the Object fall, if no collision with lower Objects
            fall();
            isonground = false;
        }

        #region collision

        public override void collide(GameObject obj)
        {
            if (obj.GetType() == typeof(Platform)) collideWithPlatform(obj);
            if (obj.GetType() == typeof(Obstacle)) collideWithObstacle(obj);
            if (obj.GetType() == typeof(ObstacleBreakable)) collideWithBreakable(obj);
            if (obj.GetType() == typeof(ObstacleMoveable)) collideWithMovable(obj);
            if (obj.GetType() == typeof(PlatformSwitchPermanent)) collideWithSwitchPermanent(obj);
            if (obj.GetType() == typeof(PlatformSwitchTemporary)) collideWithSwitchTemporary(obj);
            if (obj.GetType() == typeof(ChocoChip)) collideWithChocoChip(obj);
            if (obj.GetType() == typeof(ObstacleForSwitch)) collideWithObstacleForSwitch(obj);
        }

        private void collideWithPlatform(GameObject obj)
        {
            // Obstacle sits on a Platform
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
            }
        }
        private void collideWithObstacle(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
            }
        }
        private void collideWithSwitchPermanent(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }
        private void collideWithSwitchTemporary(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }
        private void collideWithBreakable(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox) && !obj.isdestroyed)
            {
                preventIntersection(obj);
            }
        }
        private void collideWithMovable(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }

        private void collideWithChocoChip(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
            }
        }

        private void collideWithObstacleForSwitch(GameObject obj)
        {
            if (!obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
            }
        }

        #endregion
    }
}
