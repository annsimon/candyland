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
    /// Obstacle, that can be destroyed by the Player. Under which Condition?
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
            this.m_model = content.Load<Model>("chocolatebreakable");

            this.calculateBoundingBox();
            Console.WriteLine("Min " + this.m_boundingBox.Min + " Max " + this.m_boundingBox.Max);
        }


        public override void update()
        {
            // TODO decide when Obstacle should be broken
            //this.breakObstacle();          
        }


        public void breakObstacle()
        {
            // TODO start animation and get rid of Obstacle, so the Player can move forward
        }
    }
}
