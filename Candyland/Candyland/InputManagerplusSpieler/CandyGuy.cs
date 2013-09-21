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
        CandyHelper m_CandyHelper;
        public CandyHelper getCandyHelper() { return m_CandyHelper; }
        private bool wasOnSlippery;

        public CandyGuy(Vector3 position, Vector3 direction, float aspectRatio, UpdateInfo info, BonusTracker bonusTracker, CandyHelper helper)
        {
            m_updateInfo = info;
            m_bonusTracker = bonusTracker;
            m_CandyHelper = helper;
            wasOnSlippery = false;
            this.m_position = position;
            this.direction = direction;
            this.m_original_position = this.m_position;
            this.isVisible = true;
            this.original_isVisible = isVisible;
            this.cam = new Camera(position, MathHelper.PiOver4, aspectRatio, 0.1f, GameConstants.cameraFarPlane, m_updateInfo);
            this.currentspeed = 0;
            this.upvelocity = 0;
            this.modelArray = new Model[2];
            this.skinningArray = new SkinningData[2];
            this.clipArray = new AnimationClip[2];

            this.m_material = new Material();
            this.m_material.ambient = GameConstants.ambient;
            this.m_material.diffuse = GameConstants.diffuse;
            this.m_modelTextures = new Dictionary<int, Texture2D>();
            this.modelArray = new Model[2];
        }

        public override void isNotCollidingWith(GameObject obj){ }

        public override void hasCollidedWith(GameObject obj){ }

        public override void update()
        {
            KeyboardState keystate = Keyboard.GetState();

            if (!wasOnSlippery)
            {
                if (!isOnSlipperyGround)
                {
                    if (!m_updateInfo.locked && (keystate.IsKeyDown(Keys.W)
                    || keystate.IsKeyDown(Keys.A) || keystate.IsKeyDown(Keys.D)
                    || (keystate.IsKeyDown(Keys.S) && !m_updateInfo.alwaysRun)
                    || (m_updateInfo.alwaysRun && !keystate.IsKeyDown(Keys.S)))
                    && isthirdpersoncam && m_updateInfo.candyselected && isonground)
                    {
                        animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, true, Matrix.Identity);
                    }
                    else
                    {
                        animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, false, Matrix.Identity);
                    }
                }
                else
                {
                    m_model = modelArray[1];
                    animationPlayer.StartClip(clipArray[1]);
                    wasOnSlippery = true;
                    animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, true, Matrix.Identity);
                }
            }
            else
            {
                if (isOnSlipperyGround)
                {
                    animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, true, Matrix.Identity);
                }
                else
                {
                    m_model = modelArray[0];
                    animationPlayer.StartClip(clipArray[0]);
                    wasOnSlippery = false;
                    animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, true, Matrix.Identity);
                }
            }
            base.update();
            fall();
            if (m_updateInfo.candyselected)
            cam.updatevMatrix();
        }

        public override void initialize(){ }

        public override void load(ContentManager content, AssetManager assets)
        {
            effect = assets.commonShader;
            m_texture = assets.heroTexture;
            modelArray[0] = assets.hero;
            modelArray[1] = assets.heroslipping;
            m_model = modelArray[0];
            // custom made bounding box
            m_boundingBox = new BoundingBox(this.m_position - new Vector3(0.3f, 0.35f, 0.3f), this.m_position + new Vector3(0.3f, 0.25f, 0.3f));
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;

            base.load(content, assets);

            if (m_model == null)
                return;

            // Look up our custom skinning information.
            skinningArray[0] = modelArray[0].Tag as SkinningData;

            for (int i = 1; i < modelArray.Length; i++)
            {
                foreach (ModelMesh mesh in modelArray[i].Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        BasicEffect basicEffect = part.Effect as BasicEffect;

                        part.Effect = m_material.effect;
                    }
                }

                // Look up our custom skinning information.
                skinningArray[i] = modelArray[i].Tag as SkinningData;

                if (skinningArray[i] == null)
                {
                    return;
                    throw new InvalidOperationException
                        ("This model does not contain a SkinningData tag.");

                }
                clipArray[i] = skinningArray[i].AnimationClips["ArmatureAction_001"];
            }

            clipArray[0] = skinningArray[0].AnimationClips["ArmatureAction_001"];

            animationPlayer.StartClip(clipArray[0]);
        }

        public override void uniqueskill()
        {
             if (isonground && !isCloseEnoughToInteract)
            {
                upvelocity = 0.08f;
                isonground = false;
            }
             isCloseEnoughToInteract = false;
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
                if (isOnSlipperyGround && currentspeed !=0 ) {
                    movex = direction.X;
                    movey = direction.Z;
                }
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
