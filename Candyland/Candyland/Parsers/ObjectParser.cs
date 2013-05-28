using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Candyland
{
    /// <summary>
    /// This class is used to read a level from its related files.
    /// </summary>
    public class ObjectParser
    {
        public static Dictionary<int, GameObject> ParseObjects()
        {
            return new Dictionary<int, GameObject>();
        }

        public static List<GameObject> ParseStatics()
        {
            List<GameObject> objectList = new List<GameObject>();
            objectList.Add(new Platform(Vector3.Zero));
            return objectList;
        }
    }
}
