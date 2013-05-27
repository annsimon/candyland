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
            this.Position = pos;
            this.isActive = false;
        }


        public override void Load(ContentManager content)
        {
            this.Model = content.Load<Model>("chocolatebreakable"); ;
        }


        public override void Update()
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
