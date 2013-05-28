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
    /// Obstacles are Objects in the Game World, placed on the Platforms and block the Players movement.
    /// The basic Obstacle cannot be moved or destroyed by the Player.
    /// </summary>
    class Obstacle : GameObject
    {
        public Obstacle()
        {
        }
        public Obstacle(Vector3 pos)
        {
            this.Position = pos;
            this.isActive = false;
        }

        public override void Initialize()
        {
        }

        public override void Load(ContentManager content)
        {
            this.Model = content.Load<Model>("chocolate(unmovable)"); // nothing is drawn. something different with this model?
        }

        public override void Update()
        {
        }
    }
}
