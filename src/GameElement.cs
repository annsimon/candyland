using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Candyland
{
    /// <summary>
    /// Basic Class from which all GameObjects, UI-Elements and the Camera are derived
    /// </summary>
    abstract class GameElement
    {
        public abstract void Initialize();
        public abstract void Update();
    }
}
