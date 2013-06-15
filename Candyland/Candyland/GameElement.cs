using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Candyland
{
    /// <summary>
    /// Basic Class from which all GameObjects, UI-Elements and the Camera are derived
    /// </summary>
    public abstract class GameElement
    {
        public abstract void initialize();
        public abstract void update();
    }
}
