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
    /// Actions describe some sort of behaviour an object can show that is triggered by the player
    /// (i.e. movement/interaction)
    /// An Action consists of several SubActions.
    /// The last SubAction of each Action MUST unlock the game (set locks to false)
    /// </summary>
    public class Action
    {
        String m_ID;
        public String getID() { return m_ID; }
        // these SubActions will be executed in the order indicated by their position in this list
        List<SubAction> m_subActions;
        // these SubActions still have to be executed
        List<SubAction> m_subActionsLeft;

        public Action() { }

        /// <summary>
        /// Creates a new Action
        /// </summary>
        /// <param name="id">This action's ID</param>
        /// <param name="subActions">This action's subActions (in order of execution)</param>
        public Action(String id, List<SubAction> subActions)
        {
            m_ID = id;
            m_subActions = subActions;
            // create a shallow copy
            m_subActionsLeft = new List<SubAction>(m_subActions);
        }

        public SubAction getNextSubAction()
        {
            if (m_subActionsLeft.Count == 0)
                return null;
            SubAction retAction = m_subActionsLeft[0];
            m_subActionsLeft.RemoveAt(0);
            return retAction;
        }

        public void Reset()
        {
            // create a shallow copy
            m_subActionsLeft = new List<SubAction>(m_subActions);
        }
    }
}
