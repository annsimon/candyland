using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Candyland
{
    /// <summary>
    /// Platforms are Objects in the Game World on which the Player can walk.
    /// </summary>
    class Platform : GameObject
    {
        public void Initialize()
        {
        }

        public void Load()
        {
            this.Model = Content.Load<Model>("plattform");
            this.Texture = Content.Load<Texture2D>("plattformtextur");
        }

        public void Update()
        {
        }

        public void Draw()
        {
        }
    }
}
