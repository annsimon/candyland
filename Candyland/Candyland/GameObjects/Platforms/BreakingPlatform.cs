using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkinnedModel;
using Microsoft.Xna.Framework.Audio;

namespace Candyland
{
    /// <summary>
    /// Platform that starts breaking apart when stepped on
    /// </summary>
    class BreakingPlatform : Platform
    {
        protected bool isBreaking;
        protected double timeSincedSteppedOn;
        private SoundEffect sound;

        private bool nowStartedBreaking = true;

        public BreakingPlatform(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            initialize(id, pos, updateInfo, visible);
        }

        public void initialize(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            base.init(id, pos, updateInfo, visible);

            this.isBreaking = false;
            timeSincedSteppedOn = 0;
        }

        public override void load(ContentManager content)
        {
            sound = content.Load<SoundEffect>("Sfx/CrackingPlatform8bit");  
            this.m_texture = content.Load<Texture2D>("Objekte/Plattformen/breakingplatformtextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Shaders/Shader");
            this.m_model = content.Load<Model>("Objekte/Plattformen/breakingplatform");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;

            base.load(content);

            AnimationClip clip = m_skinningData.AnimationClips["ArmatureAction"];

            animationPlayer.StartClip(clip);

            
        }

        public override void hasCollidedWith(GameObject obj)
        {
            if (isBreaking) return;

            // Position of the collided object (and therefore it's middle) is on the platform
            if (obj.getPosition().X < m_boundingBox.Max.X
                && obj.getPosition().X > m_boundingBox.Min.X
                && obj.getPosition().Z < m_boundingBox.Max.Z
                && obj.getPosition().Z > m_boundingBox.Min.Z)
            {
                isBreaking = true;
            }
           
        }

        public override void update()
        {
            if (isBreaking)
            {
                // start playing only once
                if (nowStartedBreaking)
                {
                    float volume = 0.2f;
                    float pitch = 0.0f;
                    float pan = 0.0f;
                    sound.Play(volume, pitch, pan);
                    nowStartedBreaking = false;
                }

                timeSincedSteppedOn += m_updateInfo.gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (timeSincedSteppedOn >= GameConstants.breakTime)
            {
                this.isVisible = false;
            }

            if (isBreaking && timeSincedSteppedOn < GameConstants.breakTime)
            {
                animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, true, Matrix.Identity);
            }
            else
            {
                animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, false, Matrix.Identity);
            }

        }

        public override void Reset()
        {
            isBreaking = false;
            timeSincedSteppedOn = 0;
            nowStartedBreaking = true;
            base.Reset();
        }
    }
}
