using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    /// <summary>
    /// Obstacle, that can be destroyed by the PlayerHelper.
    /// </summary>
    class ObstacleBreakable : Obstacle
    {

        public ObstacleBreakable(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            base.initialize(id, pos, updateInfo);
        }


        public override void load(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("blockbreakabletextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Toon");
            this.m_model = content.Load<Model>("blockmovable");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
        }

        public override void hasCollidedWith(GameObject obj)
        {
            if (obj.GetType() == typeof(CandyHelper)
                && m_updateInfo.currentpushedKeys.Contains(Microsoft.Xna.Framework.Input.Keys.Space)
                && !m_updateInfo.candyselected) {
                    breakObstacle();
            }
        }

        public override void update()
        {
            // let the Object fall, if no collision with lower Objects
            fall();
            isonground = false;    
        }

        #region collision

        // no special collision so far

        #endregion

        private void breakObstacle()
        {
            // TODO start animation and get rid of Obstacle, so the Player can move forward
            isdestroyed = true;
        }
        public override void draw()
        {
            if (!isdestroyed)
            {
                base.draw();
            }
        }
    }
}
