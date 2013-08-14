using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class HelperTest : DynamicGameObjects
    {
        Action m_currentAction;
        Dictionary<String, Action> m_actions;
        ActionTracker m_actionTracker;

        public HelperTest(String id, Vector3 position, ActionTracker actionTracker, 
                            UpdateInfo updateInfo, bool visible)
        {
            initialize(id, position, actionTracker, updateInfo, visible);
        }

        #region initialization

        public void initialize(String id, Vector3 position, ActionTracker actionTracker, UpdateInfo updateInfo, bool visible)
        {
            m_actionTracker = actionTracker;
            m_actions = new Dictionary<String, Action>();
            base.init(id, position, updateInfo, visible);
        }

        public override void load(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("Objekte/Wunderkugel/wunderkugeltextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Shaders/Shader");
            this.m_model = content.Load<Model>("Objekte/Wunderkugel/wunderkugelmovable");
            this.m_original_model = this.m_model;
            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;

            base.load(content);
        }

        #endregion

        public override void update()
        {
            // subAction of type movement is being performed
            if (istargeting)
                base.update();
            else
            {
                if (m_currentAction == null)
                    return;
                SubAction sAction = m_currentAction.getNextSubAction();
                if (sAction == null)
                {
                    m_currentAction = null;
                    return;
                }

                // lock the game if we have to
                if (sAction.locksGame())
                    m_updateInfo.locked = true;
                else
                    m_updateInfo.locked = false;

                switch (sAction.getType())
                {
                    case GameConstants.SubActionType.appear: isVisible = true; break;
                    case GameConstants.SubActionType.dialog: break; // not yet supported
                    case GameConstants.SubActionType.movement: moveTo(sAction.getGoal()); break;
                    case GameConstants.SubActionType.disappear: isVisible = false; break;
                }
            }
        }

        #region collision

        public override void collide(GameObject obj) { }

        #endregion

        #region collision related

        public override void hasCollidedWith(GameObject obj)
        {
        }

        public override void isNotCollidingWith(GameObject obj)
        {
        }

        #endregion

        #region actions

        public override void addAction(Action action)
        {
            m_actions.Add(action.getID(), action);
        }

        public override void Trigger(String actionID)
        {
            // action is a one time action
            if (m_actionTracker.actionState.ContainsKey(actionID))
                // and has already been performed
                if (m_actionTracker.actionState[actionID] == true)
                    return;
                // and will now be performed
                else
                    m_actionTracker.actionState[actionID] = true;

            m_currentAction = m_actions[actionID];
        }

        #endregion

        public override Matrix prepareForDrawing()
        {
            return base.prepareForDrawing();
        }

        public override void Reset()
        {
            base.Reset();
        }

    }
}
