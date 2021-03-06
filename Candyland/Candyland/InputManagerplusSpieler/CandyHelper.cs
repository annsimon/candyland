﻿using System;
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
    class CandyHelper : Playable
    {
        private bool isInAction;
        private float actionTimer;
        private bool wasOnSlippery;


        public CandyHelper(Vector3 position, Vector3 direction, float aspectRatio, UpdateInfo info, BonusTracker bonusTracker)
        {
            isInAction = false;
            wasOnSlippery = false;
            m_updateInfo = info;
            m_bonusTracker = bonusTracker;
            this.m_position = position;
            this.direction = direction;
            this.m_original_position = this.m_position;
            this.isVisible = true;
            this.original_isVisible = isVisible;
            this.cam = new Camera(position, MathHelper.PiOver4, aspectRatio, 0.1f, GameConstants.cameraFarPlane, m_updateInfo);
            this.currentspeed = 0;
            this.upvelocity = 0;
            this.modelArray = new Model[3];
            this.skinningArray = new SkinningData[3];
            this.clipArray = new AnimationClip[3];

            this.m_material = new Material();
            this.m_material.ambient = GameConstants.ambient;
            this.m_material.diffuse = GameConstants.diffuse;
            this.m_modelTextures = new Dictionary<int, Texture2D>();
        }

        public override void update()
        {
            KeyboardState keystate = Keyboard.GetState();

            if (isInAction)
            {
                animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, true, Matrix.Identity);
                actionTimer++;

                if (actionTimer > 20)
                {
                    isInAction = false;
                    m_model = modelArray[0];
                    animationPlayer.StartClip(clipArray[0]);
                }
            }
            else if (!m_updateInfo.locked && keystate.IsKeyDown(Keys.Space)
                  && isthirdpersoncam && !m_updateInfo.candyselected && !isOnSlipperyGround)
            {
                m_model = modelArray[1];
                animationPlayer.StartClip(clipArray[1]);
                isInAction = true;
                actionTimer = 0;
                animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, true, Matrix.Identity);
                actionTimer++;
            }
            else
            {
                if (!wasOnSlippery)
                {
                    if (!isOnSlipperyGround)
                    {
                        if (!m_updateInfo.locked && (keystate.IsKeyDown(Keys.W)
                            || keystate.IsKeyDown(Keys.A) || keystate.IsKeyDown(Keys.D)
                            || (keystate.IsKeyDown(Keys.S)))
                            && isthirdpersoncam && !m_updateInfo.candyselected && isonground)
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
                        m_model = modelArray[2];
                        animationPlayer.StartClip(clipArray[2]);
                        wasOnSlippery = true;
                        animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, true, Matrix.Identity);
                    }
                }
                else
                {
                    if (isOnSlipperyGround)
                    {
                        if (currentspeed != 0)
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
                        m_model = modelArray[0];
                        animationPlayer.StartClip(clipArray[0]);
                        wasOnSlippery = false;
                        animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, true, Matrix.Identity);
                    }
                }
            }
            base.update();
            fall();
            if(!m_updateInfo.candyselected)
            cam.updatevMatrix();
        }

        public override void initialize() { }

        public override void load(ContentManager content, AssetManager assets)
        {
            effect = assets.commonShader;
            m_texture = assets.buddyTexture;
            modelArray[0] = assets.buddy;
            modelArray[1] = assets.buddybreaking;
            modelArray[2] = assets.buddyslip;
            m_model = modelArray[0];
            calculateBoundingBox();
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
                clipArray[i] = skinningArray[i].AnimationClips["ArmatureAction"];
            }

            clipArray[0] = skinningArray[0].AnimationClips["ArmatureAction"];

            animationPlayer.StartClip(clipArray[0]);
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
                if (isOnSlipperyGround && currentspeed != 0)
                {
                    movex = direction.X;
                    movey = direction.Z;
                }
                move(movex,0, movey);
                if (cam.isInThirdP())
                    cam.changeAngle(camx, camy);
                else
                    cam.changeAngle(movex, movey);
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
