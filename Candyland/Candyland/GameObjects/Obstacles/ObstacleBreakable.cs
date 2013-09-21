using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using SkinnedModel;

namespace Candyland
{
    /// <summary>
    /// Obstacle, that can be destroyed by the PlayerHelper.
    /// </summary>
    class ObstacleBreakable : Obstacle
    {
        private SoundEffect sound;
        protected bool isBreaking;
        protected double timeSinceBroken;
        private bool nowStartedBreaking = true;

        public ObstacleBreakable(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            initialize(id, pos, updateInfo, visible);
        }

        #region initialization

        protected void initialize(string id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            base.initialize(id, pos, updateInfo, visible);
            this.isBreaking = false;
        }

        public override void load(ContentManager content, AssetManager assets)
        {
            sound = assets.obstacleBreakSound;
            this.m_texture = assets.obstacleBreakTexture;
            this.m_original_texture = this.m_texture;
            this.effect = assets.commonShader;
            this.m_model = assets.obstacleBreakable;
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            base.load(content, assets);

            AnimationClip clip = m_skinningData.AnimationClips["ArmatureAction"];

            animationPlayer.StartClip(clip);
        }

        #endregion

        #region collision

        // no special collision so far

        #endregion

        #region collision related

        public override void hasCollidedWith(GameObject obj)
        {
            if (isBreaking) return;

            if (obj.GetType() == typeof(CandyHelper))
            {
                helperIsClose = true;
                if (m_updateInfo.currentpushedKeys.Contains(Microsoft.Xna.Framework.Input.Keys.Space)
                && !m_updateInfo.candyselected)
                {
                    isBreaking = true;
                }
            }

        }

        public override void isNotCollidingWith(GameObject obj)
        {
        }

        #endregion

        public override void update()
        {
            if (isBreaking)
            {
                // start playing only once
                if (nowStartedBreaking)
                {
                    float pitch = 0.0f;
                    float pan = 0.0f;
                    sound.Play(((float)m_updateInfo.soundVolume) / 10, pitch, pan);
                    nowStartedBreaking = false;
                }

                timeSinceBroken += m_updateInfo.gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (timeSinceBroken >= 0.25f)
            {
                this.isVisible = false;
            }

            if (isBreaking && timeSinceBroken < 1)
            {
                animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, true, Matrix.Identity);
            }
            else
            {
                animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, false, Matrix.Identity);
            }
            base.update();
            fall();
            isonground = false;
        }

        public override void Reset()
        {
            isBreaking = false;
            timeSinceBroken = 0;
            nowStartedBreaking = true;
            base.Reset();
        }

    }
}
