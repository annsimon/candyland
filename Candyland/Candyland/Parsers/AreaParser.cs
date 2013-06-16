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
    /// This class is used to extract the id and area data for each area
    /// from the level file.
    /// The level data will be processed further by LevelParser upon creation
    /// of each level.
    /// </summary>
    public class AreaParser
    {
        public static Dictionary<string, Area> ParseAreas(UpdateInfo info, BonusTracker bonusTracker)
        {
            Dictionary<string, Area> areaList = new Dictionary<string, Area>();

            XmlDocument scene = new XmlDocument();

            scene.Load("Content\\SceneTest2.xml");

            XmlNodeList id = scene.GetElementsByTagName("area_id");
            XmlNodeList prev = scene.GetElementsByTagName("area_prev");
            XmlNodeList next = scene.GetElementsByTagName("area_next");
            XmlNodeList start = scene.GetElementsByTagName("area_starting_position");
            XmlNodeList areaContent = scene.GetElementsByTagName("levels");

            int count = 0;

            foreach( XmlNode node in id )
            {
                // create Vector3 from contents of "area_starting_position" (x,y,z)
                Vector3 startPos = new Vector3();
                startPos.X = float.Parse(start[count].SelectSingleNode("x").InnerText);
                startPos.Y = float.Parse(start[count].SelectSingleNode("y").InnerText);
                startPos.Z = float.Parse(start[count].SelectSingleNode("z").InnerText);

                // create a new area of id, starting position, update info, camera and the xml in "levels"
                Area area = new Area(node.InnerText, startPos, info, areaContent[count].InnerXml, bonusTracker);
                
                // set previous
                string prevID = prev[count].InnerText;
                if (prevID == "x")
                    area.hasPrevious = false;
                else
                {
                    area.hasPrevious = true;
                    area.previousID = prevID;
                }
                
                // set next
                string nextID = next[count].InnerText;
                if (nextID == "x")
                    area.hasNext = false;
                else
                {
                    area.hasNext = true;
                    area.nextID = nextID;
                }

                // add completed area to area list
                areaList.Add(node.InnerText, area);

                // increase count as it is used to access the not-id xml elements of the correct area
                // (the one currently being parsed)
                count++;
            }

            return areaList;
        }
    }
}
