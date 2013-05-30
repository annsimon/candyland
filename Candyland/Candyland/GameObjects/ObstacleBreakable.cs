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
        public ObstacleBreakable(Vector3 pos)
        {
            this.position = pos;
            this.isActive = false;
        }


        public override void load(ContentManager content)
        {
            this.model = content.Load<Model>("chocolatebreakable"); ;
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
