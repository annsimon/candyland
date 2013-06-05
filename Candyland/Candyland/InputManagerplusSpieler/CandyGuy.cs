

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Candyland
{
    class CandyGuy : Playable
    {
        bool isonground = false;
        bool istargeting;
        Vector3 target;
       


        public CandyGuy(Vector3 position, Vector3 direction, float aspectRatio, UpdateInfo info)
        {
            m_updateInfo = info;
            this.m_position = position;
            this.direction = direction;
            this.cam = new Camera(position, MathHelper.PiOver4, aspectRatio, 0.1f, 100, m_updateInfo);
            this.currentspeed = 0;
            this.gravity = -0.0005f;
            this.upvelocity = 0;
        }

        public override void isNotCollidingWith(GameObject obj)
        {

        }

        public override void hasCollidedWith(GameObject obj)
        {

        }

        public override void update()
        {
             throw new NotImplementedException();
        }

        public override void initialize()
        {
            throw new NotImplementedException();
        }

        public override void load(ContentManager content)
        {
            m_model = content.Load<Model>("spielerbeta");
            calculateBoundingBox();
        }

        

        public override void jump()
        {
            if (isonground)
            {
                upvelocity = 0.02f;
                isonground = false;
            }

        }


        public override void moveTo(Vector3 goalpoint)
        {
            istargeting = true;
            target = goalpoint;
        }

        public override void collide(GameObject obj)
        {
            if (obj.GetType() == typeof(Platform)) 
            {
                ContainmentType contain = obj.getBoundingBox().Contains(this.m_boundingBox);
                if ( contain == ContainmentType.Intersects
                    && obj.getPosition().Y < this.m_position.Y) 
                { 
                    isonground = true;
                    float upvec = this.m_boundingBox.Min.Y - obj.getBoundingBox().Max.Y;
                    this.m_position.Y -= upvec;
                    this.m_boundingBox.Max.Y -= upvec;
                    this.m_boundingBox.Min.Y -= upvec;
                    obj.hasCollidedWith(this);
                }
                else
                { 
                    isonground = isonground || false;
                    obj.isNotCollidingWith(this);
                }

                if (obj.GetType() == typeof(PlatformSwitch)){}
            }

            if (obj.GetType() == typeof(Obstacle)) 
            {
                if(obj.GetType() == typeof(ObstacleBreakable)){}
                if(obj.GetType() == typeof(ObstacleMoveable)){}
            }
        }

        public override void startIntersection()
        {
            this.isonground = false;
        }

        

        public override void movementInput(float movex, float movey, float camx, float camy)
        {
            if (istargeting)
            {
                float dx = target.X - m_position.X;
                float dz = target.Z - m_position.Z;
                float length = (float)Math.Sqrt(dx * dx + dz * dz);
                move(0.8f * dx / length, 0.8f * dz / length);
                if (length < 1) istargeting = false;
                fall();
            }
            else
            {
                fall();
                move(movex, movey);
                cam.changeAngle(camx, camy);
            }
        }

        private void fall() 
        {

            upvelocity += gravity;
            if (isonground) upvelocity = 0;
            this.m_position.Y += upvelocity;
            this.m_boundingBox.Max.Y += upvelocity;
            this.m_boundingBox.Min.Y += upvelocity;
            if (/*upvelocity < 0*/ true) cam.changeposition(m_position);
        }
     


    }
}
