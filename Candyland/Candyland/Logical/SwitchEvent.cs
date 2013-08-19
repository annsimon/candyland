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
        
        public SwitchEvent( string triggerableID, string switchGroupType, List<String> switchIDs,
                            Dictionary<string, GameObject> objects, Dictionary<string, GameObject> switches)
        {
            m_triggerable = objects[triggerableID];
            if( switchGroupType.Equals("ordered") )
                m_switchGroup = new OrderedSwitchGroup(switchIDs, switches, this);
            else
                m_switchGroup = new SwitchGroup(switchIDs, switches, this);
        }

        public void Trigger()
        {
            m_triggerable.isVisible = !m_triggerable.getOriginalVisibility();
        }

        public void ResetTrigger()
        {
            m_triggerable.isVisible = m_triggerable.getOriginalVisibility();
        }

        public void Reset()
        {
            m_switchGroup.Reset();
        }
    }
}
