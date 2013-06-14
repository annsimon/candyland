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
    class CandyHelper : Playable
    {
        private bool istargeting = false;
        private Vector3 target;


        public CandyHelper(Vector3 position, Vector3 direction, float aspectRatio, UpdateInfo info, BonusTracker bonusTracker)
        {
            m_updateInfo = info;
            m_bonusTracker = bonusTracker;
            this.m_position = position;
            this.direction = direction;
            this.cam = new Camera(position, MathHelper.PiOver4, aspectRatio, 0.1f, 100, m_updateInfo);
            this.currentspeed = 0;
            this.gravity = -0.004f;
            this.upvelocity = 0;
        }


        public override void isNotCollidingWith(GameObject obj) { }

        public override void hasCollidedWith(GameObject obj) { }

        public override void update() {
            fall();
            if(!m_updateInfo.candyselected)
            cam.updatevMatrix();
        }

        public override void initialize() { }

        public override void load(ContentManager content)
        {
            m_model = content.Load<Model>("partnerneu");
            calculateBoundingBox();
        }

        public override void moveTo(Vector3 goalpoint)
        {
            throw new NotImplementedException();
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

        protected override void fall()
        {
            upvelocity += gravity;
            if (isonground) upvelocity = 0;
            this.m_position.Y += upvelocity;
            this.m_boundingBox.Max.Y += upvelocity;
            this.m_boundingBox.Min.Y += upvelocity;
            cam.changeposition(m_position);
        }

        public override void uniqueskill(){ }

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
        }

        private void collideWithPlatform(GameObject obj)
        {
            ContainmentType contain = obj.getBoundingBox().Contains(m_boundingBox);

            if (contain == ContainmentType.Intersects)
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                isonground = isonground || false;
                obj.isNotCollidingWith(this);
            }
        }
        private void collideWithObstacle(GameObject obj)
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
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
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
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }

        #endregion

        public override void draw()
        {
            Matrix view = m_updateInfo.viewMatrix;
            Matrix projection = m_updateInfo.projectionMatrix;
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[m_model.Bones.Count];
            m_model.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix translateMatrix = Matrix.CreateTranslation(m_position);
            Matrix worldMatrix = translateMatrix;


            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in m_model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    Matrix rotation;
                    if (direction.X > 0)
                    {
                        rotation = Matrix.CreateRotationY((float)Math.Acos(direction.Z));
                    }
                    else
                    {
                        rotation = Matrix.CreateRotationY((float)-Math.Acos(direction.Z));
                    }

                    effect.World = rotation *
                        worldMatrix * transforms[mesh.ParentBone.Index];
                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
                BoundingBoxRenderer.Render(this.m_boundingBox, m_updateInfo.graphics, view, projection, Color.White);

            }
        }
        
    }
}
