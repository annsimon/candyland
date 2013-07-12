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
    /// This class takes care of the game, this includes visual aspects (Area) and
    /// the game's logic (Communications).
    /// </summary>
    class OrderedSwitchGroup : SwitchGroup
    {
        protected string[] m_orderedSwitchIDs;
        protected int m_switchesActivatedInOrder;

        public OrderedSwitchGroup(List<string> switchIds, Dictionary<string, GameObject> objects, SwitchEvent parentEvent)
        {
            m_switches = new Dictionary<string, PlatformSwitch>();
            m_parentEvent = parentEvent;
            m_conditionMet = false;

            m_orderedSwitchIDs = new string[switchIds.Count];
            int count = 0;

            foreach( string switchID in switchIds )
            {
                PlatformSwitch currSwitch = (PlatformSwitch)objects[switchID];
                currSwitch.setGroup(this);
                m_switches.Add(switchID, currSwitch);
                m_orderedSwitchIDs[count] = switchID;
                count++;
            }
        }

        public override void Changed(PlatformSwitch currSwitch)
        {
            if (m_conditionMet)
            {
                foreach (var curSwitch in m_switches)
                    curSwitch.Value.setInactive();
                m_switchesActivatedInOrder = 0;
                m_conditionMet = false;
                m_parentEvent.ResetTrigger();
                return;
            }
            try
            {
                // check if condition for this group is met:
                // all switches are active and have been activated in the correct order
                if (!currSwitch.getID().Equals(m_orderedSwitchIDs[m_switchesActivatedInOrder]))
                {
                    currSwitch.setTouched(GameConstants.TouchedState.stillTouched);
                    foreach (var curSwitch in m_switches)
                        curSwitch.Value.setInactive();
                    m_switchesActivatedInOrder = 0;
                }
                else
                {
                    m_switchesActivatedInOrder++;
                    if(m_switchesActivatedInOrder == m_orderedSwitchIDs.GetLength(0))
                    {
                        m_conditionMet = true;
                        m_parentEvent.Trigger();
                    }
                }
            }
            catch
            {
            }
        }

        public override void Reset()
        {
            m_conditionMet = false;
            m_switchesActivatedInOrder = 0;
        }
    }
}
