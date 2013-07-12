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
    /// A SubAction is a part of an action (i.e. movement/interaction triggered by the player by interacting with a certain object)
    /// </summary>
    public class SubAction
    {
        GameConstants.SubActionType m_type;
        public GameConstants.SubActionType getType() { return m_type; }
        Vector3 m_goal;
        public Vector3 getGoal() { return m_goal; }
        String m_text;
        public String getText() { return m_text; }
        bool m_locks;
        public bool locksGame() { return m_locks; }

        public SubAction() { }

        /// <summary>
        /// Creates a new SubAction that can be processed by any object supporting actions.
        /// </summary>
        /// <param name="type">The SubActionType</param>
        /// <param name="goal">The position the object will move to (only used if SubActionType is movement)</param>
        /// <param name="text">The text the object will display in a conversation (only used if SubActionTime is dialog)</param>
        /// <param name="locks">This SubAction will lock the game (player can't move; "movie mode")</param>
        public SubAction(GameConstants.SubActionType type, Vector3 goal, String text, bool locks)
        {
            m_type = type;
            m_goal = goal;
            m_text = text;
            m_locks = locks;
        }
    }
}
