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

                XmlNodeList type = actions.GetElementsByTagName("type");
                XmlNodeList goalX = actions.GetElementsByTagName("goalX");
                XmlNodeList goalY = actions.GetElementsByTagName("goalY");
                XmlNodeList goalZ = actions.GetElementsByTagName("goalZ");
                XmlNodeList text = actions.GetElementsByTagName("text");
                XmlNodeList locks = actions.GetElementsByTagName("locks");

                int count = 0;

                foreach( XmlNode node in subActions )
                {
                    // create Vector3 from contents of "goal" (x,y,z)
                    Vector3 pos = new Vector3();
                    pos.X = float.Parse(goalX[count].InnerText);
                    pos.Y = float.Parse(goalY[count].InnerText);
                    pos.Z = float.Parse(goalZ[count].InnerText);
                    pos += levelStart; // add level position for correct global position

                    GameConstants.SubActionType sActionType = GameConstants.SubActionType.appear;
                    if (type[count].InnerText == "disappear")
                        sActionType = GameConstants.SubActionType.disappear;
                    else if (type[count].InnerText == "movement")
                        sActionType = GameConstants.SubActionType.movement;
                    else if (type[count].InnerText == "dialog")
                        sActionType = GameConstants.SubActionType.dialog;

                    sActions.Add(new SubAction(sActionType, pos,
                                                text[count].InnerText, bool.Parse(locks[count].InnerText)));
                    count++;
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
