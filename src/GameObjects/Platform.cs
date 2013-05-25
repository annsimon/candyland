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
    /// Platforms are Objects in the Game World on which the Player can walk.
    /// </summary>
    class Platform : GameObject
    {
        public Platform()
        {
        }
        public Platform(Vector3 pos)
        {
            this.Position = pos;
            this.isActive = false;
        }

        public override void Initialize()
        {
        }

        public override void Load(ContentManager content)
        {
            this.Model = content.Load<Model>("plattform");
        }

        public override void Update()
        {
        }
    }
}
