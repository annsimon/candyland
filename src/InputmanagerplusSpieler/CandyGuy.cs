

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


namespace Controller_test
{
    class CandyGuy : Playable
    {
        bool isonground = true;
        bool istargeting;
        Vector3 target;
      //  bool iswalking;

       


        public CandyGuy(Vector3 position, Vector3 direction, float aspectRatio)
        {
            this.position = position;
            this.direction = direction;
            this.bbox = new BoundingBox(position,position+new Vector3(1,2,1));
            this.cam = new Camera(position, MathHelper.PiOver4, aspectRatio, 0.1f, 100);
            this.currentspeed = 0;
            this.gravity = 1;
            this.upvelocity = 0;
        }



        public override void loadContent(ContentManager content)
        {
            model = content.Load<Model>("player");
        }

        
        public override void Draw()
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateTranslation(position);
                    effect.View = cam.getviewMatrix();
                    effect.Projection = cam.getProjectionMatrix();
                    effect.EnableDefaultLighting();
                }

                mesh.Draw();
            }
        }


        public override void jump()
        {
            if (!isonground)
            {
                position.Y += upvelocity + gravity;
                upvelocity -= 0.1f;
            }
            if (upvelocity < 0) upvelocity = 0;
        }


        public override void moveTo(Vector3 goalpoint)
        {
            istargeting = true;
            target = goalpoint;
        }

       
        

        public override void movementInput(float movex, float movey, float camx, float camy)
        {
            if (istargeting)
            {
                float dx = target.X - position.X;
                float dz = target.Z - position.Z;
                float len = (float)Math.Sqrt(dx * dx + dz * dz);
                move(0.8f * dx / len, 0.8f * dz / len);
                if (len < 1) istargeting = false;
            }
            else
            {
                move(movex, movey);
                cam.changeAngle(camx, camy);
            }
        }

        
     


    }
}
