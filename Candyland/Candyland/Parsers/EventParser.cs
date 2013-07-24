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
    public class EventParser
    {
        public static List<SwitchEvent> ParseEvents(string levelID, Dictionary<string,GameObject> objects,
                                                    Dictionary<string, GameObject> switches)
        {
            List<SwitchEvent> eventList = new List<SwitchEvent>();

            XmlDocument events = new XmlDocument();
            XmlDocument switchGroups = new XmlDocument();

            events.Load(GameConstants.eventFile);

            XmlNodeList levelEventsTemp = events.GetElementsByTagName("L" + levelID);
            if (levelEventsTemp.Count == 0) return new List<SwitchEvent>();

            events.LoadXml(levelEventsTemp[0].InnerXml);

            XmlNodeList eventNodes = events.GetElementsByTagName("event");

            foreach(XmlNode eventNode in eventNodes)
            {
                events.LoadXml(eventNode.InnerXml);
                XmlNode triggerableID = events.GetElementsByTagName("triggerable")[0];

                XmlNode switchGroup = events.GetElementsByTagName("switchGroup")[0];

                XmlNode switchGroupType = events.GetElementsByTagName("switchGroupType")[0];

                switchGroups.LoadXml(switchGroup.InnerXml);

                XmlNodeList switchIDNodes = events.GetElementsByTagName("switch_id");

                List<string> switchIDs = new List<string>();
                foreach( XmlNode node in switchIDNodes )
                {
                    switchIDs.Add(node.InnerText);
                }

                SwitchEvent evnt = new SwitchEvent(triggerableID.InnerText, switchGroupType.InnerText, switchIDs, 
                                                    objects, switches);

                eventList.Add(evnt);
            }

            return eventList;
        }
    }
}
