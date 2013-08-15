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
        public CandyHelper(Vector3 position, Vector3 direction, float aspectRatio, UpdateInfo info, BonusTracker bonusTracker)
        {
            m_updateInfo = info;
            m_bonusTracker = bonusTracker;
            this.m_position = position;
            this.direction = direction;
            this.m_original_position = this.m_position;
            this.isVisible = true;
            this.original_isVisible = isVisible;
            this.cam = new Camera(position, MathHelper.PiOver4, aspectRatio, 0.1f, 100, m_updateInfo);
            this.currentspeed = 0;
            this.upvelocity = 0;

            this.m_material = new Material();
            this.m_material.ambient = GameConstants.ambient;
            this.m_material.diffuse = GameConstants.diffuse;
            this.m_modelTextures = new Dictionary<int, Texture2D>();
        }

        public override void isNotCollidingWith(GameObject obj) { }

        public override void hasCollidedWith(GameObject obj) { }

        public override void update()
        {
            base.update();
            fall();
            if(!m_updateInfo.candyselected)
            cam.updatevMatrix();
        }

        public override void initialize() { }

        public override void load(ContentManager content)
        {
            effect = content.Load<Effect>("Shaders/Shader");
            m_texture = content.Load<Texture2D>("NPCs/Helper/partnertextur");
            m_model = content.Load<Model>("NPCs/Helper/partnerneu");
            calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            base.load(content);
        }

        public override void movementInput(float movex, float movey, float camx, float camy)
        {
            if (istargeting)
            {
                float dx = target.X - m_position.X;
                float dz = target.Z - m_position.Z;
                float length = (float)Math.Sqrt(dx * dx + dz * dz);
                move(0.8f * dx / length,0, 0.8f * dz / length);
                if (length < 1) istargeting = false;
                fall();
            }
            else
            {
                fall();
                move(movex,0, movey);
                cam.changeAngle(camx, camy);
            }
        }

        protected override void fall()
        {
            upvelocity += GameConstants.gravity ;
            if (isonground) upvelocity = 0;
            this.m_position.Y += upvelocity;
            this.m_boundingBox.Max.Y += upvelocity;
            this.m_boundingBox.Min.Y += upvelocity;
            cam.changeposition(m_position);
        }

        public override void uniqueskill(){}

        #region collision

        // Needed for destruction^^
        protected override void collideWithBreakable(GameObject obj)
        {
            if (obj.isVisible && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }

        #endregion

        public override Matrix prepareForDrawing()
        {
            if (m_updateInfo.helperavailable)
            {
                Matrix view = m_updateInfo.viewMatrix;
                Matrix projection = m_updateInfo.projectionMatrix;
                // Copy any parent transforms.
                Matrix[] transforms = new Matrix[m_model.Bones.Count];
                m_model.CopyAbsoluteBoneTransformsTo(transforms);

                Matrix translateMatrix = Matrix.CreateTranslation(m_position);
                Matrix worldMatrix = translateMatrix;
                Matrix rotation;
                if (direction.X > 0)
                {
                    rotation = Matrix.CreateRotationY((float)Math.Acos(direction.Z));
                }
                else
                {
                    rotation = Matrix.CreateRotationY((float)-Math.Acos(direction.Z));
                }

                return rotation * worldMatrix;
            }
            return new Matrix();
        }
        
    }
}
