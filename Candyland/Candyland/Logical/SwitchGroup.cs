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
    public class SwitchGroup
    {
        // switches is a dictionary that keeps all switches belonging to the group
        Dictionary<string, PlatformSwitch> m_switches;

        // event ths group belongs to
        Event m_parentEvent;

        protected bool m_conditionMet;

        public SwitchGroup(Dictionary<string, GameObject> objects, Event parentEvent)
        {
            m_switches = new Dictionary<string, PlatformSwitch>();
            m_parentEvent = parentEvent;
            m_conditionMet = false;

            string switchID = "0.1.0.switchPermanent";

            PlatformSwitch currSwitch = (PlatformSwitch)objects[switchID];
            currSwitch.setGroup(this);
            m_switches.Add(switchID, currSwitch);
        }

        public void Changed()
        {
            if(!m_conditionMet)
            {
                // check if condition for this group is met:
                // all switches are active
                foreach( var curSwitch in m_switches )
                {
                    if (curSwitch.Value.getActivated())
                    {
                        m_conditionMet = true;
                    }
                    else
                    {
                        m_conditionMet = false;
                        return;
                    }    
                }
                m_parentEvent.Trigger();
            }
        }

        public void Reset()
        {
            m_conditionMet = false;
        }
    }
}
