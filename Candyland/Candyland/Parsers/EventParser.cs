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
        public static List<Event> ParseEvents(string levelID, Dictionary<string,GameObject> objects)
        {
            List<Event> eventList = new List<Event>();

            XmlDocument events = new XmlDocument();
            XmlDocument switchGroups = new XmlDocument();

            events.Load("Content\\EventTest.xml");

            events.LoadXml(events.GetElementsByTagName("L"+levelID)[0].InnerXml);

            XmlNodeList eventNodes = events.GetElementsByTagName("event");

            foreach(XmlNode eventNode in eventNodes)
            {
                events.LoadXml(eventNode.InnerXml);
                XmlNode triggerableID = events.GetElementsByTagName("triggerable")[0];

                XmlNode switchGroup = events.GetElementsByTagName("switchGroup")[0];

                switchGroups.LoadXml(switchGroup.InnerXml);

                XmlNodeList switchIDNodes = events.GetElementsByTagName("switch_id");

                List<string> switchIDs = new List<string>();
                foreach( XmlNode node in switchIDNodes )
                {
                    switchIDs.Add(node.InnerText);
                }

                Event evnt = new Event(triggerableID.InnerText, switchIDs, objects);

                eventList.Add(evnt);

            }

            return eventList;
        }
    }
}
