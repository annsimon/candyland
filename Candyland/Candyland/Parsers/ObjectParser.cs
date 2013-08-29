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
        public static List<Dictionary<string, GameObject>> ParseObjects(Vector3 lvl_start, string xml, UpdateInfo info, 
                                                                  BonusTracker bonusTracker, ActionTracker actionTracker)
        {
            Dictionary<string, GameObject> dynamicObjects = new Dictionary<string, GameObject>();
            Dictionary<string, GameObject> switches = new Dictionary<string, GameObject>();

            XmlDocument scene = new XmlDocument();

            scene.LoadXml(xml);

            XmlNodeList objects = scene.GetElementsByTagName("dynamic_objects");

            scene.LoadXml(objects[0].InnerXml);

            XmlNodeList objs = scene.GetElementsByTagName("object");

            foreach (XmlNode node in objs)
            {
                // create Vector3 from contents of "object_position" (x,y,z)
                Vector3 pos = new Vector3();
                pos.X = float.Parse(node.Attributes["posX"].InnerText);
                pos.Y = float.Parse(node.Attributes["posY"].InnerText);
                pos.Z = float.Parse(node.Attributes["posZ"].InnerText);
                pos += lvl_start; // add level position for correct global position

                // get bool value for visible
                bool isVisible = bool.Parse(node.Attributes["visible"].InnerText);

                // create the new object
                string object_type = node.Attributes["type"].InnerText;

                if (object_type == "platform")
                {
                    if (dynamicObjects.ContainsKey(node.InnerText))
                    {
                        Console.WriteLine("Key " + node.InnerText + " duplicated");
                        continue;
                    }
                    int object_size;
                    try
                    {
                        object_size = int.Parse(node.Attributes["size"].InnerText);
                    }
                    catch
                    {
                        object_size = 1;
                    }
                    int slip;
                    try
                    {
                        slip = int.Parse(node.Attributes["slippery"].InnerText);
                    }
                    catch
                    {
                        slip = 0;
                    }
                    string id = node.Attributes["id"].InnerText;
                    Platform obj = new Platform(id, pos, slip, node.Attributes["is_door_to_area"].InnerText, node.Attributes["is_door_to_level"].InnerText, info, isVisible, object_size);
                    dynamicObjects.Add(id, obj);
                }
				else
                if (object_type == "movingPlatform")
                {
                    string id = node.Attributes["id"].InnerText;
                    if (dynamicObjects.ContainsKey(id))
                    {
                        Console.WriteLine("Key " + id + " duplicated");
                        continue;
                    }
                    int object_size;
                    try
                    {
                        object_size = int.Parse(node.Attributes["size"].InnerText);
                    }
                    catch
                    {
                        object_size = 1;
                    }
                    Vector3 endpos = new Vector3(0, 0, 0);
                    try
                    {
                        endpos.X = float.Parse(node.Attributes["endPosX"].InnerText);
                        endpos.Y = float.Parse(node.Attributes["endPosY"].InnerText);
                        endpos.Z = float.Parse(node.Attributes["endPosZ"].InnerText);
                        endpos += lvl_start; // add level position for correct global position
                    }
                    catch { }
                    MovingPlatform obj = new MovingPlatform(id, pos, endpos, info, isVisible, object_size);
                    dynamicObjects.Add(id, obj);
                }
                else
                if (object_type == "obstacle")
                {
                    string id = node.Attributes["id"].InnerText; 
                    if (dynamicObjects.ContainsKey(id))
                    {
                        Console.WriteLine("Key " + id + " duplicated");
                        continue;
                    }
                    int object_size;
                    try
                    {
                        object_size = int.Parse(node.Attributes["size"].InnerText);
                    }
                    catch
                    {
                        object_size = 1;
                    }
                    Obstacle obj = new Obstacle(id, pos, info, isVisible, object_size);
                    dynamicObjects.Add(id, obj);
                }
                else
                if (object_type == "breakable")
                {
                    string id = node.Attributes["id"].InnerText;
                    if (dynamicObjects.ContainsKey(id))
                    {
                        Console.WriteLine("Key " + id + " duplicated");
                        continue;
                    }
                    ObstacleBreakable obj = new ObstacleBreakable(id, pos, info, isVisible);
                    dynamicObjects.Add(id, obj);
                }
                else
                if (object_type == "obstacleForSwitch" || object_type == "obstacleForFalling")
                {
                    string id = node.Attributes["id"].InnerText;
                    if (dynamicObjects.ContainsKey(id))
                    {
                        Console.WriteLine("Key " + id + " duplicated");
                        continue;
                    }
                    ObstacleForSwitch obj = new ObstacleForSwitch(id, pos, info, isVisible);
                    dynamicObjects.Add(id, obj);
                }
                else
                if (object_type == "movableObstacle")
                {
                    string id = node.Attributes["id"].InnerText;
                    if (dynamicObjects.ContainsKey(id))
                    {
                        Console.WriteLine("Key " + id + " duplicated");
                        continue;
                    }
                    ObstacleMoveable obj = new ObstacleMoveable(id, pos, info, isVisible);
                    dynamicObjects.Add(id, obj);
                }
                else
                if (object_type == "switchPermanent")
                {
                    string id = node.Attributes["id"].InnerText;
                    if (dynamicObjects.ContainsKey(node.InnerText))
                    {
                        Console.WriteLine("Key " + id + " duplicated");
                        continue;
                    }
                    PlatformSwitch obj = new PlatformSwitchPermanent(id, pos, info, isVisible);
                    switches.Add(id, obj);
                }
                else
                if (object_type == "switchTimed")
                {
                    string id = node.Attributes["id"].InnerText;
                    if (dynamicObjects.ContainsKey(id))
                    {
                        Console.WriteLine("Key " + id + " duplicated");
                        continue;
                    }
                    PlatformSwitch obj = new PlatformSwitchTimed(id, pos, info, isVisible);
                    switches.Add(id, obj);
                }
                else
                if (object_type == "switchTemporary")
                {
                    string id = node.Attributes["id"].InnerText;
                    if (dynamicObjects.ContainsKey(id))
                    {
                        Console.WriteLine("Key " + id + " duplicated");
                        continue;
                    }
                    PlatformSwitch obj = new PlatformSwitchTemporary(id, pos, info, isVisible);
                    switches.Add(id, obj);
                }
                else
                if (object_type == "breakingPlatform")
                {
                    string id = node.Attributes["id"].InnerText;
                    if (dynamicObjects.ContainsKey(id))
                    {
                        Console.WriteLine("Key " + id + " duplicated");
                        continue;
                    }
                    BreakingPlatform obj = new BreakingPlatform(id, pos, info, isVisible);
                    dynamicObjects.Add(id, obj);
                }
                else
                if (object_type == "chocoChip")
                {
                    string id = node.Attributes["id"].InnerText;
                    if (dynamicObjects.ContainsKey(id))
                    {
                        Console.WriteLine("Key " + id + " duplicated");
                        continue;
                    }
                    ChocoChip obj = new ChocoChip(id, pos, info,  isVisible,bonusTracker);
                    dynamicObjects.Add(id, obj);
                }
                else
                if (object_type == "teleportPlatform")
                {
                    string id = node.Attributes["id"].InnerText;
                    if (dynamicObjects.ContainsKey(id))
                    {
                        Console.WriteLine("Key " + id + " duplicated");
                        continue;
                    }
                    Vector3 endpos = new Vector3(0, 0, 0);
                    try
                    {
                        endpos.X = float.Parse(node.Attributes["endPosX"].InnerText);
                        endpos.Y = float.Parse(node.Attributes["endPosY"].InnerText);
                        endpos.Z = float.Parse(node.Attributes["endPosZ"].InnerText);
                        endpos += lvl_start; // add level position for correct global position
                    }
                    catch { }
                    PlatformTeleporter obj = new PlatformTeleporter(id, pos, info, isVisible, endpos);
                    dynamicObjects.Add(id, obj);
                }
                else
                if (object_type == "HelperTest"|| object_type == "actionActor")
                {
                    string id = node.Attributes["id"].InnerText;
                    if (dynamicObjects.ContainsKey(id))
                    {
                        Console.WriteLine("Key " + id + " duplicated");
                        continue;
                    }
                    ActionActor obj = new ActionActor(id, pos, actionTracker, info, isVisible);
                    dynamicObjects.Add(id, obj);
                }
            }

            List<Dictionary<String, GameObject>> retList = new List<Dictionary<String, GameObject>>();
            retList.Add(dynamicObjects);
            retList.Add(switches);
            return retList;
        }

        public static List<GameObject> ParseStatics(Vector3 lvl_start, string xml, UpdateInfo info)
        {
            List<GameObject> objectList = new List<GameObject>();
            XmlDocument scene = new XmlDocument();

            scene.LoadXml(xml);

            XmlNodeList objects = scene.GetElementsByTagName("static_objects");

            scene.LoadXml(objects[0].InnerXml);

            XmlNodeList objs = scene.GetElementsByTagName("object");

            foreach (XmlNode node in objs)
            {
                // create Vector3 from contents of "object_position" (x,y,z)
                Vector3 pos = new Vector3();
                pos.X = float.Parse(node.Attributes["posX"].InnerText);
                pos.Y = float.Parse(node.Attributes["posY"].InnerText);
                pos.Z = float.Parse(node.Attributes["posZ"].InnerText);
                pos += lvl_start; // add level position for correct global position

                // get bool value for visible
                bool isVisible = bool.Parse(node.Attributes["visible"].InnerText);

                // create the new object
                string object_type = node.Attributes["type"].InnerText;

                if (object_type == "platform")
                {
                    int object_size;
                    try
                    {
                        object_size = int.Parse(node.Attributes["size"].InnerText);
                    }
                    catch
                    {
                        object_size = 1;
                    }
                    int slip;
                    try
                    {
                        slip = int.Parse(node.Attributes["slippery"].InnerText);
                    }
                    catch
                    {
                        slip = 0;
                    }
                    Platform obj = new Platform(node.Attributes["id"].InnerText, pos, slip, "x", "x", info, isVisible, object_size);
                    objectList.Add(obj);
                }
                else
                if (object_type == "obstacle")
                {
                    int object_size;
                    try
                    {
                        object_size = int.Parse(node.Attributes["size"].InnerText);
                    }
                    catch
                    {
                        object_size = 1;
                    }
                    Obstacle obj = new Obstacle(node.Attributes["id"].InnerText, pos, info, isVisible, object_size);
                    objectList.Add(obj);
                }
                else
                if (object_type == "bonbon")
                {
                    string text = node.Attributes["dialog"].InnerText;
                    BonbonFairy obj = new BonbonFairy(node.Attributes["id"].InnerText, pos, info, isVisible, text);
                    objectList.Add(obj);
                }
                else
                if (object_type == "salesman")
                {
                    string text = node.Attributes["dialog"].InnerText;
                    Salesman obj = new Salesman(node.Attributes["id"].InnerText, pos, info, isVisible, text);
                    objectList.Add(obj);
                }
            }

            return objectList;
        }
    }
}
