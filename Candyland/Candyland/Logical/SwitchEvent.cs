using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Candyland
{
    /// <summary>
    /// Class that takes care of what has to happen after a switch group was activated.
    /// </summary>
    public class SwitchEvent
    {
        private GameObject m_triggerable;

        private SwitchGroup m_switchGroup;

        private bool m_triggered;
        
        public SwitchEvent( string triggerableID, string switchGroupType, List<String> switchIDs, Dictionary<string,GameObject> objects )
        {
            m_triggerable = objects[triggerableID];
            if( switchGroupType.Equals("ordered") )
                m_switchGroup = new OrderedSwitchGroup(switchIDs, objects, this);
            else
                m_switchGroup = new SwitchGroup(switchIDs, objects, this);
        }

        public void Trigger()
        {
            m_triggered = true;
            m_triggerable.isVisible = !m_triggerable.getOriginalVisibility();
        }

        public void ResetTrigger()
        {
            m_triggered = false;
            m_triggerable.isVisible = m_triggerable.getOriginalVisibility();
        }

        public void Reset()
        {
            m_switchGroup.Reset();
            m_triggered = false;
        }
    }
}
