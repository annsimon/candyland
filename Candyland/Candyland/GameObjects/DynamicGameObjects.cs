using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Candyland
{
    public abstract class DynamicGameObjects : GameObject
    {
        protected float upvelocity;             //beschleinigungsfaktor in y richtung
        protected bool isonground = false;
        protected Vector3 minOld;
        protected Vector3 maxOld;

        protected virtual void fall()
        {
            upvelocity += GameConstants.gravity;
            if (isonground) upvelocity = 0;
            this.m_position.Y += upvelocity;
            this.m_boundingBox.Max.Y += upvelocity;
            this.m_boundingBox.Min.Y += upvelocity;
        }

        #region collision

        public override void collide(GameObject obj)
        {
            // may not be called for itself!!!
            if (obj.GetType() == typeof(Platform)) this.collideWithPlatform(obj);
            if (obj.GetType() == typeof(Obstacle)) this.collideWithObstacle(obj);
            if (obj.GetType() == typeof(ObstacleBreakable)) this.collideWithBreakable(obj);
            if (obj.GetType() == typeof(ObstacleMoveable)) this.collideWithMovable(obj);
            if (obj.GetType() == typeof(ObstacleForSwitch)) this.collideWithObstacleForSwitch(obj);
            if (obj.GetType() == typeof(PlatformSwitchPermanent)) this.collideWithSwitchPermanent(obj);
            if (obj.GetType() == typeof(PlatformSwitchTemporary)) this.collideWithSwitchTemporary(obj);
            if (obj.GetType() == typeof(ChocoChip)) this.collideWithChocoChip(obj);
            if (obj.GetType() == typeof(PlatformTeleporter)) this.collideWithTeleporter(obj);
        }

        protected virtual void collideWithPlatform(GameObject obj)
        {
            // Object sits on a Platform
            if (!obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
            }
        }
        protected virtual void collideWithObstacle(GameObject obj)
        {
            if (!obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
            }
        }
        protected virtual void collideWithSwitchPermanent(GameObject obj)
        {
            if (!obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }
        protected virtual void collideWithSwitchTemporary(GameObject obj)
        {
            if (!obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }
        protected virtual void collideWithBreakable(GameObject obj)
        {
            if (!obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox) && !obj.isdestroyed)
            {
                preventIntersection(obj);
            }
        }
        protected virtual void collideWithMovable(GameObject obj)
        {
            if (!obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }
        protected virtual void collideWithChocoChip(GameObject obj)
        {
            if (!obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
            }
        }
        protected virtual void collideWithObstacleForSwitch(GameObject obj)
        {
            if (!obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
            }
        }
        protected virtual void collideWithTeleporter(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }

        #endregion

        public override void  endIntersection()
        {
 	         base.endIntersection();
             minOld = m_boundingBox.Min;
             maxOld = m_boundingBox.Max;
        }


        protected void preventIntersection(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {


                float m_minX = Math.Min(m_boundingBox.Min.X, m_boundingBox.Max.X);
                float m_minY = Math.Min(m_boundingBox.Min.Y, m_boundingBox.Max.Y);
                float m_minZ = Math.Min(m_boundingBox.Min.Z, m_boundingBox.Max.Z);
                float m_maxX = Math.Max(m_boundingBox.Min.X, m_boundingBox.Max.X);
                float m_maxY = Math.Max(m_boundingBox.Min.Y, m_boundingBox.Max.Y);
                float m_maxZ = Math.Max(m_boundingBox.Min.Z, m_boundingBox.Max.Z);
                float minX = Math.Min(obj.getBoundingBox().Min.X, obj.getBoundingBox().Max.X);
                float minY = Math.Min(obj.getBoundingBox().Min.Y, obj.getBoundingBox().Max.Y);
                float minZ = Math.Min(obj.getBoundingBox().Min.Z, obj.getBoundingBox().Max.Z);
                float maxX = Math.Max(obj.getBoundingBox().Min.X, obj.getBoundingBox().Max.X);
                float maxY = Math.Max(obj.getBoundingBox().Min.Y, obj.getBoundingBox().Max.Y);
                float maxZ = Math.Max(obj.getBoundingBox().Min.Z, obj.getBoundingBox().Max.Z);

                float m_minXold = minOld.X;
                float m_minYold = minOld.Y;
                float m_minZold = minOld.Z;
                float m_maxXold = maxOld.X;
                float m_maxYold = maxOld.Y;
                float m_maxZold = maxOld.Z;

                if (m_minYold >= maxY)
                {
                    isonground = true;

                    float m_boxheight = m_maxY - m_minY;
                    float upvec = m_position.Y - m_minY;

                    m_boundingBox.Max.Y = maxY + m_boxheight;
                    m_boundingBox.Min.Y = maxY;
                    m_position.Y = m_boundingBox.Min.Y + upvec;
                }

                if (m_maxYold <= minY)
                {

                    float m_boxheight = m_maxY - m_minY;
                    float upvec = m_position.Y - m_minY;

                    m_boundingBox.Max.Y = minY;
                    m_boundingBox.Min.Y = minY - m_boxheight;
                    m_position.Y = m_boundingBox.Min.Y + upvec;
                    upvelocity = 0;
                }

                if (m_minXold >= maxX
                    && !((m_minY >= maxY) || (m_maxY <= minY)))
                {
                    float m_boxwidth = m_maxX - m_minX;
                    float xvector = m_position.X - m_minX;


                    m_boundingBox.Max.X = maxX + m_boxwidth;
                    m_boundingBox.Min.X = maxX;
                    m_position.X = m_boundingBox.Min.X + xvector;
                }
                if (m_maxXold <= minX
                    && !((m_minY >= maxY) || (m_maxY <= minY)))
                {
                    float m_boxwidth = m_maxX - m_minX;
                    float xvector = m_position.X - m_minX;


                    m_boundingBox.Max.X = minX;
                    m_boundingBox.Min.X = minX - m_boxwidth;
                    m_position.X = m_boundingBox.Min.X + xvector;
                }

                if (m_minZold >= maxZ
                    && !((m_minY >= maxY) || (m_maxY <= minY)))
                {
                    float m_boxdepth = m_maxZ - m_minZ;
                    float zvector = m_position.Z - m_minZ;

                    m_boundingBox.Max.Z = maxZ + m_boxdepth;
                    m_boundingBox.Min.Z = maxZ;
                    m_position.Z = m_boundingBox.Min.Z + zvector;
                }
                if (m_maxZold <= minZ
                    && !((m_minY >= maxY) || (m_maxY <= minY)))
                {
                    float m_boxdepth = m_maxZ - m_minZ;
                    float zvector = m_position.Z - m_minZ;

                    m_boundingBox.Max.Z = minZ;
                    m_boundingBox.Min.Z = minZ - m_boxdepth;
                    m_position.Z = m_boundingBox.Min.Z + zvector;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            upvelocity = 0;
        }

    }
}
