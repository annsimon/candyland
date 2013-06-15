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
            this.ID = id;
            this.m_position = pos;
            this.isActive = false;
            this.m_updateInfo = updateInfo;
        }


        public override void load(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("plattformtextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Toon");
            this.m_model = content.Load<Model>("plattform");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
            Console.WriteLine("Min " + this.m_boundingBox.Min + " Max " + this.m_boundingBox.Max);
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
            // TODO decide when Obstacle should be broken
            //this.breakObstacle();          
        }


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
