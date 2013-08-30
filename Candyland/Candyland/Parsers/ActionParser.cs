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
    /// This class is used to extract the id and event data for each event
    /// from the event file.
    /// </summary>
    public class ActionParser
    {
        public static void ParseActions(String levelID, Vector3 levelStart, 
                                        Dictionary<string,GameObject> objects, 
                                        ActionTracker actionTracker)
        {
            XmlDocument actions = new XmlDocument();

            actions.Load(GameConstants.actionsFile);

            XmlNodeList levelActionsTemp = actions.GetElementsByTagName("L" + levelID);
            if (levelActionsTemp.Count == 0) return;

            actions.LoadXml(levelActionsTemp[0].InnerXml);

            XmlNodeList actionNodes = actions.GetElementsByTagName("action");

            foreach(XmlNode eventNode in actionNodes)
            {
                actions.LoadXml(eventNode.InnerXml);

                XmlNode actionID = actions.GetElementsByTagName("id")[0];
                XmlNode oneTimeAction = actions.GetElementsByTagName("oneTimeAction")[0];

                XmlNodeList subActions = actions.GetElementsByTagName("subaction");
                List<SubAction> sActions = new List<SubAction>();

                foreach( XmlNode node in subActions )
                {
                    // create Vector3 from contents of "goal" (x,y,z)
                    Vector3 pos = new Vector3(0,0,0);
                    try
                    {
                        pos.X = float.Parse(node.Attributes["goalX"].InnerText);
                        pos.Y = float.Parse(node.Attributes["goalY"].InnerText);
                        pos.Z = float.Parse(node.Attributes["goalZ"].InnerText);
                        pos += levelStart; // add level position for correct global position
                    }
                    catch
                    {
                        Console.WriteLine("Error parsing actions: invalid position - using default!");
                    }

                    GameConstants.SubActionType sActionType = GameConstants.SubActionType.appear;
                    string type = node.Attributes["type"].InnerText;
                    if (type == "disappear")
                        sActionType = GameConstants.SubActionType.disappear;
                    else if (type == "movement")
                        sActionType = GameConstants.SubActionType.movement;
                    else if (type == "dialog")
                        sActionType = GameConstants.SubActionType.dialog;

                    string text = node.Attributes["text"].InnerText;
                    text = text.Replace("!ae!", "ä");
                    text = text.Replace("!ue!", "ü");
                    text = text.Replace("!oe!", "ö");
                    text = text.Replace("!ss!", "ß");

                    sActions.Add(new SubAction(sActionType, pos,
                                                text, bool.Parse(node.Attributes["locks"].InnerText)));
                }

                Action tempAction = new Action(actionID.InnerText, sActions);

                if (bool.Parse(oneTimeAction.InnerText))
                    actionTracker.actionState.Add(actionID.InnerText, false);

                // register action with trigger and triggered object
                XmlNode triggerID = actions.GetElementsByTagName("trigger")[0];
                XmlNode triggeredID = actions.GetElementsByTagName("triggered")[0];
                objects[triggerID.InnerText].setTrigger(actionID.InnerText, objects[triggeredID.InnerText]);
                objects[triggeredID.InnerText].addAction(tempAction);
            }
        }
    }
}
