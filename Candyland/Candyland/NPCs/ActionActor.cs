using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class ActionActor : DynamicGameObjects
    {
        protected Action m_currentAction;
        protected Dictionary<String, Action> m_actions;
        protected ActionTracker m_actionTracker;
        protected string m_dialogImage;

        protected ActionActor() { }

        public ActionActor(String id, Vector3 position, ActionTracker actionTracker, 
                            UpdateInfo updateInfo, bool visible)
        {
            Vector3 pos = new Vector3(position.X, position.Y + 0.4f, position.Z);
            if (id.Contains("helperActor"))
                pos = new Vector3(position.X + 0.5f, position.Y + 0.21329f, position.Z + 0.5f);
            if (id.Contains("bossActor"))
                pos = new Vector3(position.X, position.Y + 0.6f, position.Z);
            initialize(id, pos, actionTracker, updateInfo, visible);
        }

        #region initialization

        public virtual void initialize(String id, Vector3 position, ActionTracker actionTracker, UpdateInfo updateInfo, bool visible)
        {
            m_actionTracker = actionTracker;
            m_actions = new Dictionary<String, Action>();
            if (id.Contains("helperActor"))
                m_dialogImage = "Images/DialogImages/Helper";
            else
            if (id.Contains("bossActor"))
                m_dialogImage = "Images/DialogImages/Boss";
            else
                m_dialogImage = "Images/DialogImages/AcaHelper";
            base.init(id, position, updateInfo, visible);
        }

        public override void load(ContentManager content)
        {
            if (this.ID.Contains("helperActor"))
                this.m_texture = content.Load<Texture2D>("NPCs/Helper/buddytextur");
            else
            if (this.ID.Contains("bossActor"))
                this.m_texture = content.Load<Texture2D>("NPCs/Lakritze/bosstextur");
            else
                this.m_texture = content.Load<Texture2D>("NPCs/TutorialGuy/tutorialtexture");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Shaders/Shader"); 
            if (this.ID.Contains("helperActor"))
                this.m_model = content.Load<Model>("NPCs/TutorialGuy/tutorial");
            else
            if (this.ID.Contains("bossActor"))
                this.m_model = content.Load<Model>("NPCs/Lakritze/boss");
            else
                this.m_model = content.Load<Model>("NPCs/TutorialGuy/tutorial");
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
            if (m_actionTracker.actionState.ContainsKey("GetHelper") && m_actionTracker.actionState["GetHelper"] == true)
                this.original_isVisible = false;
            if (!m_updateInfo.actionInProgress)
            {
                if (m_currentAction != null)
                {
                    m_currentAction.Reset();
                    istargeting = false;
                }
                return;
            }
            if (istargeting)
            {
                base.update();
                if (this.ID.Contains("bossActor"))
                    m_updateInfo.bossPosition = m_position;
            }
            else
            {
                if (m_currentAction == null)
                    return;
                SubAction sAction = m_currentAction.getNextSubAction();
                if (sAction == null)
                {
                    m_currentAction = null;
                    m_updateInfo.actionInProgress = false;
                    return;
                }

                // lock the game if we have to
                if (sAction.locksGame())
                    m_updateInfo.locked = true;
                else
                    m_updateInfo.locked = false;

                switch (sAction.getType())
                {
                    case GameConstants.SubActionType.appear: appear(); break;
                    case GameConstants.SubActionType.dialog: m_updateInfo.m_screenManager.ActivateNewScreen(new DialogListeningScreen(sAction.getText(), m_dialogImage)); break;
                    case GameConstants.SubActionType.movement: movement(sAction); break;
                    case GameConstants.SubActionType.disappear: disappear(); break;
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
            // ignore actions if there is one already in progress
            if (m_updateInfo.actionInProgress)
                return;
            // action is a one time action
            if (m_actionTracker.actionState.ContainsKey(actionID))
                // and has already been performed
                if (m_actionTracker.actionState[actionID] == true)
                    return;
                // and will now be performed
                else
                {
                    m_actionTracker.actionState[actionID] = true;
                    if (ID.Equals("GetHelper"))
                        original_isVisible = false;
                }

            m_currentAction = m_actions[actionID];
            m_updateInfo.actionInProgress = true;
        }

        protected virtual void appear()
        {
            if (this.m_currentAction.getID().Contains("LoseHelper"))
                m_updateInfo.loseHelperNow = true;
            isVisible = true;
        }

        protected virtual void disappear()
        {
            if (this.m_currentAction.getID().Contains("GetHelper"))
                m_updateInfo.activateHelperNow = true;
            if (this.m_currentAction.getID().Contains("StartChase"))
                m_updateInfo.alwaysRun = false;
            isVisible = false;
        }

        protected virtual void movement(SubAction sAction)
        {
            if (this.m_currentAction.getID().Contains("StartChase"))
            {
                m_updateInfo.alwaysRun = true;
                moveTo(sAction.getGoal(), 0.048f);
            }
            else
                moveTo(sAction.getGoal());
        }

        #endregion

        public override Matrix prepareForDrawing()
        {
            return base.prepareForDrawing();
        }

        public override void Reset()
        {
            base.Reset();
            if (this.ID.Contains("bossActor"))
                m_updateInfo.bossPosition = m_position;
        }

    }
}
