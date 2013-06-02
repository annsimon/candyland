

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
        bool isonground = true;
        bool istargeting;
        Vector3 target;


        public CandyGuy(Vector3 position, Vector3 direction, float aspectRatio)
        {
            this.m_position = position;
            this.direction = direction;
            this.m_boundingBox = new BoundingBox(position,position+new Vector3(1,2,1));
            this.cam = new Camera(position, MathHelper.PiOver4, aspectRatio, 0.1f, 100);
            this.currentspeed = 0;
            this.gravity = -0.01f;
            this.upvelocity = 0;
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
            model = content.Load<Model>("spielerbeta");
        }

        
        public override void Draw()
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateTranslation(m_position);
                    effect.View = cam.getviewMatrix();
                    effect.Projection = cam.getProjectionMatrix();
                    effect.EnableDefaultLighting();
                }

                mesh.Draw();
            }
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
                if (obj.getPosition().Y < this.m_position.Y) isonground = true;

                if (obj.GetType() == typeof(PlatformSwitch)){}
            }

            if (obj.GetType() == typeof(Obstacle)) 
            {
                if(obj.GetType() == typeof(ObstacleBreakable)){}
                if(obj.GetType() == typeof(ObstacleMoveable)){}
            }
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
            if (upvelocity < 0) cam.changeposition(m_position);
        }
     


    }
}
