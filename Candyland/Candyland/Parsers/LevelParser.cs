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
    /// This class is used to extract the id and level data for each area
    /// from the area data.
    /// The data will be processed further by ObjectParser upon creation
    /// of each level.
    /// </summary>
    public class LevelParser
    {
        public static Dictionary<int, Level> ParseLevels(UpdateInfo info, Camera cam)
        {
            Dictionary<int, Level> levelList = new Dictionary<int, Level>();
            levelList.Add(0, new Level(info, cam));
            return levelList;
        }
    }
}
