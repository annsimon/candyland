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
    /// Parent Class for all Objects that appear in the Game World (Platforms, Obstacles, Characters...)
    /// </summary>
    abstract class GameObject : GameElement
    {
        Vector3 Position;
        Model Model;
        Texture2D Texture;
        BoundingBox BoundingBox;
        bool isActive;

        public abstract void Load();
        public void Draw()
        {

        }
    }
}
