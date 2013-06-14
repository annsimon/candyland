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
    /// This class is used to read a level from its related files.
    /// </summary>
    public class ObjectParser
    {
        public static Dictionary<string, GameObject> ParseObjects(Vector3 lvl_start, string xml, UpdateInfo info, BonusTracker bonusTracker)
        {
            Dictionary<string, GameObject> dynamicObjects = new Dictionary<string, GameObject>();

            XmlDocument scene = new XmlDocument();

            scene.LoadXml(xml);

            XmlNodeList objects = scene.GetElementsByTagName("dynamic_objects");

            scene.LoadXml(objects[0].InnerXml);

            XmlNodeList id = scene.GetElementsByTagName("object_id");
            XmlNodeList type = scene.GetElementsByTagName("object_type");
            XmlNodeList position = scene.GetElementsByTagName("object_position");
            XmlNodeList door_to_area = scene.GetElementsByTagName("is_door_to_area");
            XmlNodeList door_to_level = scene.GetElementsByTagName("is_door_to_level");
            XmlNodeList slippery = scene.GetElementsByTagName("slippery");

            int count = 0;

            foreach (XmlNode node in id)
            {
                // create Vector3 from contents of "object_position" (x,y,z)
                Vector3 pos = new Vector3();
                pos.X = float.Parse(position[count].SelectSingleNode("x").InnerText);
                pos.Y = float.Parse(position[count].SelectSingleNode("y").InnerText);
                pos.Z = float.Parse(position[count].SelectSingleNode("z").InnerText);
                pos += lvl_start; // add level position for correct global position

                // get bool value for slippery
                bool slip = bool.Parse(slippery[count].InnerText);

                // create the new object
                string object_type = type[count].InnerText;

                if (object_type == "platform")
                {
                    Platform obj = new Platform(node.InnerText, pos, slip, door_to_area[count].InnerText, door_to_level[count].InnerText, info);
                    dynamicObjects.Add(node.InnerText, obj);
                }
                if (object_type == "breakable")
                {
                    ObstacleBreakable obj = new ObstacleBreakable(node.InnerText, pos, info);
                    dynamicObjects.Add(node.InnerText, obj);
                }
                if (object_type == "movableObstacle")
                {
                    ObstacleMoveable obj = new ObstacleMoveable(node.InnerText, pos, info);
                    dynamicObjects.Add(node.InnerText, obj);
                }
                if (object_type == "chocoChip")
                {
                    ChocoChip obj = new ChocoChip(node.InnerText, pos, info, bonusTracker);
                    dynamicObjects.Add(node.InnerText, obj);
                }

                // increase count as it is used to access the not-id xml elements of the correct level
                // (the one currently being parsed)
                count++;
            }

            return dynamicObjects;
        }

        public static List<GameObject> ParseStatics(Vector3 lvl_start, string xml, UpdateInfo info)
        {
            List<GameObject> objectList = new List<GameObject>();
            XmlDocument scene = new XmlDocument();

            scene.LoadXml(xml);

            XmlNodeList objects = scene.GetElementsByTagName("static_objects");

            scene.LoadXml(objects[0].InnerXml);

            XmlNodeList id = scene.GetElementsByTagName("object_id");
            XmlNodeList type = scene.GetElementsByTagName("object_type");
            XmlNodeList position = scene.GetElementsByTagName("object_position");
            XmlNodeList slippery = scene.GetElementsByTagName("object_slippery");

            int count = 0;

            foreach (XmlNode node in id)
            {
                // create Vector3 from contents of "object_position" (x,y,z)
                Vector3 pos = new Vector3();
                pos.X = float.Parse(position[count].SelectSingleNode("x").InnerText);
                pos.Y = float.Parse(position[count].SelectSingleNode("y").InnerText);
                pos.Z = float.Parse(position[count].SelectSingleNode("z").InnerText);
                pos += lvl_start; // add level position for correct global position

                // get bool value for slippery
                //bool slip = bool.Parse(slippery[count].InnerText);

                // create the new object
                string object_type = type[count].InnerText;

                if (object_type == "platform")
                {
                    Platform obj = new Platform(node.InnerText, pos, false , "x", "x", info);
                    objectList.Add(obj);
                }

                // increase count as it is used to access the not-id xml elements of the correct level
                // (the one currently being parsed)
                count++;
            }

            return objectList;
        }
    }
}
