using System;
using System.Xml;
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
        public static Dictionary<string, Level> ParseLevels(string xml, Vector3 area_start, UpdateInfo info, BonusTracker bonusTracker, ActionTracker actionTracker)
        {
            Dictionary<string, Level> levelList = new Dictionary<string, Level>();

            XmlDocument scene = new XmlDocument();

            scene.LoadXml(xml);

            XmlNodeList id = scene.GetElementsByTagName("level_id");
            XmlNodeList start = scene.GetElementsByTagName("level_starting_position");
            XmlNodeList idStartMain = scene.GetElementsByTagName("main_start_platform");
            XmlNodeList idStartSecondary = scene.GetElementsByTagName("secondary_start_platform");
            XmlNodeList levelContent = scene.GetElementsByTagName("objects");

            int count = 0;

            foreach (XmlNode node in id)
            {
                // create Vector3 from contents of "area_starting_position" (x,y,z)
                Vector3 startPos = new Vector3();
                startPos.X = float.Parse(start[count].SelectSingleNode("x").InnerText, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                startPos.Y = float.Parse(start[count].SelectSingleNode("y").InnerText, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                startPos.Z = float.Parse(start[count].SelectSingleNode("z").InnerText, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                startPos += area_start; // add area position for correct global position

                // create a new area of id, starting position, update info, camera and the xml in "levels"
                Level level = new Level(node.InnerText, startPos, info, levelContent[count].InnerXml, bonusTracker, actionTracker);
                
                // set start platform for player and companion
                level.setStartPositions(idStartMain[count].InnerText);

                // add completed level to level list
                levelList.Add(node.InnerText, level);

                // increase count as it is used to access the not-id xml elements of the correct level
                // (the one currently being parsed)
                count++;
            }

            return levelList;
        }
    }
}
