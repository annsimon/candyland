using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Candyland
{
    public abstract class DynamicGameObjects : GameObject
    {
        protected float upvelocity;             //beschleunigungsfaktor in y richtung
        protected bool isonground = false;

        // used for moveTo
        protected bool istargeting = false;
        protected Vector3 target;

        public virtual void moveTo(Vector3 goalpoint)
        {
            moveTo(goalpoint, GameConstants.slippingSpeed);
        }


        protected float m_moveToSpeed;
        public virtual void moveTo(Vector3 goalpoint, float speed) {
            istargeting = true;
            target = goalpoint;
            m_moveToSpeed = speed;
        }

        public override void update()
        {
            // set invisible, when fallen too deep
            if (m_position.Y < GameConstants.endOfWorld_Y)
                this.isVisible = false;
            else
            if (istargeting)
            {
                float dx = target.X - m_position.X;
                float dz = target.Z - m_position.Z;
                float dy = target.Y - m_position.Y;
                float length = (float)Math.Sqrt(dx * dx + dz * dz + dy * dy);
                Vector3 tempdir = new Vector3(dx, dy, dz);
                tempdir.Normalize();
                move(tempdir.X * m_moveToSpeed, tempdir.Y * m_moveToSpeed, tempdir.Z * m_moveToSpeed);
                if (length < m_moveToSpeed+0.01f) istargeting = false;
            }
        }

        /// <summary>
        /// Moves the Object, input should be between [-1,1]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected virtual void move(float x, float y, float z)
        {
            if ((x != 0 || y != 0 || z!=0))
            {
                direction = new Vector3(x, y, z);                   

                m_position += direction ;             //Change ObjectPosition

                m_boundingBox.Min += direction ;
                m_boundingBox.Max += direction ;
            }
        }

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
            if (obj.GetType() == typeof(Platform)) collideWithPlatform(obj);
            if (obj.GetType() == typeof(Obstacle)) collideWithObstacle(obj);
            if (obj.GetType() == typeof(ObstacleBreakable)) collideWithBreakable(obj);
            if (obj.GetType() == typeof(ObstacleMoveable)) collideWithMovable(obj);
            if (obj.GetType() == typeof(ObstacleForSwitch)) collideWithObstacleForSwitch(obj);
            if (obj.GetType() == typeof(PlatformSwitchPermanent)) collideWithSwitchPermanent(obj);
            if (obj.GetType() == typeof(PlatformSwitchTemporary)) collideWithSwitchTemporary(obj);
            if (obj.GetType() == typeof(PlatformSwitchTimed)) collideWithSwitchTimed(obj);
            if (obj.GetType() == typeof(ChocoChip)) collideWithChocoChip(obj);
            if (obj.GetType() == typeof(PlatformTeleporter)) collideWithTeleporter(obj);
            if (obj.GetType() == typeof(MovingPlatform)) collideWithMovingPlatform(obj);
            if (obj.GetType() == typeof(BreakingPlatform)) collideWithBreakingPlatform(obj);
            if (obj.GetType() == typeof(Salesman)) collideWithSalesman(obj);
        }

        protected virtual void collideWithSalesman(GameObject obj)
        {
            if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
        }

        protected virtual void collideWithBreakingPlatform(GameObject obj)
        {
            // Object sits on a Platform
            if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
        }

        protected virtual void collideWithMovingPlatform(GameObject obj)
        {
            // Object sits on a Platform
            if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                Vector3 diff = obj.getDirection();

                if (diff.Y > 0) diff.Y = 0;

                m_boundingBox.Min += diff;
                m_boundingBox.Max += diff;
                m_position += diff;

                obj.hasCollidedWith(this);
            }
        }

        protected virtual void collideWithPlatform(GameObject obj)
        {
            // Object sits on a Platform
            if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
        }

        protected virtual void collideWithObstacle(GameObject obj)
        {
            if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
            }
        }
        protected virtual void collideWithSwitchPermanent(GameObject obj)
        {
            if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
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
            if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }
        protected virtual void collideWithSwitchTimed(GameObject obj)
        {
            if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
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
            if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
            }
        }
        protected virtual void collideWithMovable(GameObject obj)
        {
            if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                //if (!(obj is Playable))
                //    System.Console.WriteLine("collideMovable");
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
            if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
            }
        }
        protected virtual void collideWithObstacleForSwitch(GameObject obj)
        {
            if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
            }
        }
        protected virtual void collideWithTeleporter(GameObject obj)
        {
            if (obj.isVisible && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }

        #endregion

        #region collision related

        public override void endIntersection()
        {
 	         base.endIntersection();
        }

        protected void preventIntersection(GameObject obj)
        {
            //if (obj.isVisible)
            //    return;
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

                if (m_minYold >= maxY-0.1f)
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

        #endregion

        public override void Reset()
        {
            base.Reset();
            upvelocity = 0;
        }

    }
}
