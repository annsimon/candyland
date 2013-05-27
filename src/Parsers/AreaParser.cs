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
    /// This class is used to extract the id and area data for each area
    /// from the level file.
    /// The level data will be processed further by LevelParser upon creation
    /// of each level.
    /// </summary>
    public class AreaParser
    {
        public static Dictionary<int, Area> ParseAreas(UpdateInfo info, Camera cam)
        {
            Dictionary<int, Area> areaList = new Dictionary<int, Area>();
            areaList.Add(0, new Area(info, cam));
            return areaList;
        }
    }
}
