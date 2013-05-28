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
        // Obstacles will slide over slippery platforms, when being pushed
        protected bool isSlippery;
        public bool getSlippery() { return this.isSlippery; }
        public void setSlippery(bool value) { this.isSlippery = value; }


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
            this.BoundingBox = calculateBoundingBox(this.Model, this.Position);
            //this.BoundingBox = new BoundingBox(new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(0.5f,0.5f,0.5f));
        }

        public override void Update()
        {
        }
    }
}
