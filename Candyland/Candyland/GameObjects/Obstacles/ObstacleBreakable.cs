using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Candyland
{
    /// <summary>
    /// Obstacle, that can be destroyed by the PlayerHelper.
    /// </summary>
    class ObstacleBreakable : Obstacle
    {
        private SoundEffect sound;

        public ObstacleBreakable(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            initialize(id, pos, updateInfo, visible);
        }

        #region initialization

        protected void initialize(string id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            base.initialize(id, pos, updateInfo, visible);
        }

        public override void load(ContentManager content)
        {
            sound = content.Load<SoundEffect>("Sfx/CrackingBlock8bit");
            this.m_texture = content.Load<Texture2D>("Objekte/Obstacles/Breakable/blockmovabletexture");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Shaders/Shader");
            this.m_model = content.Load<Model>("Objekte/Obstacles/Breakable/blockbreakable");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            base.load(content);
        }

        #endregion

        public override void update()
        {
            if (!isVisible)
                return;
            base.update();
            // let the Object fall, if no collision with lower Objects
            fall();
            isonground = false;
        }

        #region collision

        // no special collision so far

        #endregion

        #region collision related

        public override void hasCollidedWith(GameObject obj)
        {
            if (obj.GetType() == typeof(CandyHelper))
            {
                helperIsClose = true;
                if (m_updateInfo.currentpushedKeys.Contains(Microsoft.Xna.Framework.Input.Keys.Space)
                && !m_updateInfo.candyselected)
                {
                    breakObstacle();
                }
            }
                
        }

        public override void isNotCollidingWith(GameObject obj)
        {
        }

        #endregion

        #region actions

        private void breakObstacle()
        {
            // TODO start animation and get rid of Obstacle, so the Player can move forward
            float volume = 0.3f;
            float pitch = 0.0f;
            float pan = 0.0f;
            sound.Play(volume, pitch, pan);
            isVisible = false;
        }

        #endregion
    }
}
