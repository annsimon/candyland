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
using SkinnedModel;


namespace Candyland
{
    class CandyGuy : Playable
    {        
        public CandyGuy(Vector3 position, Vector3 direction, float aspectRatio, UpdateInfo info, BonusTracker bonusTracker)
        {
             
            m_updateInfo = info;
            m_bonusTracker = bonusTracker;
            this.m_position = position;
            this.direction = direction;
            this.m_original_position = this.m_position;
            this.isVisible = true;
            this.original_isVisible = isVisible;
            this.cam = new Camera(position, MathHelper.PiOver4, aspectRatio, 0.1f, 30, m_updateInfo);
            this.currentspeed = 0;
            this.upvelocity = 0;

            this.m_material = new Material();
            this.m_material.ambient = GameConstants.ambient;
            this.m_material.diffuse = GameConstants.diffuse;
            this.m_modelTextures = new Dictionary<int, Texture2D>();
        }

        public override void isNotCollidingWith(GameObject obj){ }

        public override void hasCollidedWith(GameObject obj){ }

        public override void update()
        {
            KeyboardState keystate = Keyboard.GetState();
            if (!keystate.IsKeyDown(Keys.W) && !keystate.IsKeyDown(Keys.A) && !keystate.IsKeyDown(Keys.D) && !keystate.IsKeyDown(Keys.S))
            {
                animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, false, Matrix.Identity);

            }
            else
            {
                animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, true, Matrix.Identity);
            }
            base.update();
            fall();
            if (m_updateInfo.candyselected)
            cam.updatevMatrix();
        }

        public override void initialize(){ }

        public override void load(ContentManager content)
        {
            effect = content.Load<Effect>("Shaders/Shader");
            m_texture = content.Load<Texture2D>("NPCs/Spieler/Candyguytextur");
            m_model = content.Load<Model>("NPCs/Spieler/candyguy");
            calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;

            base.load(content);

            AnimationClip clip = m_skinningData.AnimationClips["ArmatureAction_001"];

            animationPlayer.StartClip(clip);
        }

        public override void uniqueskill()
        {
             if (isonground)
            {
                upvelocity = 0.08f;
                isonground = false;
            }
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
            }
            else
            {
                move(movex,0, movey);
                cam.changeAngle(camx, camy);
            }
        }

        protected override void fall() 
        {

            upvelocity += GameConstants.gravity;
            if (isonground) upvelocity = 0;
            this.m_position.Y += upvelocity;
            this.m_boundingBox.Max.Y += upvelocity;
            this.m_boundingBox.Min.Y += upvelocity;
            cam.changeposition(m_position);
        }

        #region collision
        public override void collide(GameObject obj)
        {
            // only one to collide with bonbonFairy
            if (obj.GetType() == typeof(BonbonFairy)) collideWithBonbonFairy(obj);

            base.collide(obj);
        }

        private void collideWithBonbonFairy(GameObject obj)
        {
            if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                obj.hasCollidedWith(this);
            }
        }

        #endregion

        public override Matrix prepareForDrawing()
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
    }
}
