﻿using System;
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
                m_dialogImage = "Buddy";
            else
            if (id.Contains("bossActor"))
                m_dialogImage = "Boss";
            else
                m_dialogImage = "AcaHelper";
            base.init(id, position, updateInfo, visible);
        }

        public override void load(ContentManager content, AssetManager assets)
        {
            if (this.ID.Contains("helperActor"))
                this.m_texture = assets.actionBuddyTexture;
            else
            if (this.ID.Contains("bossActor"))
                this.m_texture = assets.bossTexture;
            else
                this.m_texture = assets.tutorialGuyTexture;
            this.m_original_texture = this.m_texture;
            this.effect = assets.commonShader;
            if (this.ID.Contains("helperActor"))
                this.m_model = assets.actionBuddy;
            else
            if (this.ID.Contains("bossActor"))
                this.m_model = assets.boss;
            else
                this.m_model = assets.tutorialGuy;
            this.m_original_model = this.m_model;
            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;

            base.load(content, assets);
        }

        public void SetVisibilityAfterLoadingSavegame()
        {
            if (m_actionTracker.actionActorVisibility.ContainsKey(this.ID))
            {
                this.original_isVisible = m_actionTracker.actionActorVisibility[this.ID];
                this.isVisible = m_actionTracker.actionActorVisibility[this.ID];
            }
        }

        #endregion

        public override void update()
        {
            if (m_updateInfo.playerWon && ID.Contains("boss") && !istargeting)
                return;
            if (!m_updateInfo.tutorialActive && !(ID.Contains("bossActor") || ID.Contains("helperActor")))
                return;
            // subAction of type movement is being performed
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
                if (m_updateInfo.playerWon)
                {
                    m_currentAction = null;
                    istargeting = false;
                    m_updateInfo.locked = false;
                    m_updateInfo.finaledistance = false;
                    return;
                }
            }
            else
            {
                if (m_currentAction == null)
                    return;

                if (m_currentAction.getID().Equals("GetHelper"))
                {
                    this.original_isVisible = false;
                    if(!m_actionTracker.actionActorVisibility.ContainsKey(this.ID))
                        m_actionTracker.actionActorVisibility.Add(this.ID, false);
                }
                if (m_currentAction.getID().Equals("firstTutorial"))
                {
                    this.original_isVisible = false;
                    if (!m_actionTracker.actionActorVisibility.ContainsKey(this.ID))
                        m_actionTracker.actionActorVisibility.Add(this.ID, false);
                }

                SubAction sAction = m_currentAction.getNextSubAction();
                if (sAction == null)
                {
                    m_currentAction = null;
                    m_updateInfo.actionInProgress = false;
                    m_updateInfo.helperActionInProgress = false;
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
                    case GameConstants.SubActionType.dialog: m_updateInfo.m_screenManager.ActivateNewScreen(new DialogListeningScreen(sAction.getText(), m_dialogImage));if(this.ID.Equals("255.2.2.actionActor")) m_updateInfo.playerfirst = true; break;
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

        #endregion

        #region actions

        public override void addAction(Action action)
        {
            m_actions.Add(action.getID(), action);
        }

        public override void Trigger(String actionID)
        {
            // ignore actions if there is one already in progress or if they should not be in progress
            if ((m_updateInfo.actionInProgress && !actionID.Equals("ending")) || (!m_updateInfo.tutorialActive && !(ID.Contains("bossActor") || ID.Contains("helperActor") || actionID.Equals("ending"))))
                return;
            if (actionID.Contains("stage") && m_updateInfo.playerWon)
                return;
            if (actionID.Equals("ending"))
            {
                m_updateInfo.playerWon = true;
                m_updateInfo.helperavailable = true; // this is a todo! -> do it in a different way later
            }
            // action is a one time action
            if (m_actionTracker.actionState.ContainsKey(actionID))
            {
                // and has already been performed
                if (m_actionTracker.actionState[actionID] == true)
                    return;
                // and will now be performed
                else
                {
                    m_actionTracker.actionState[actionID] = true;
                    if (ID.Equals("GetHelper"))
                    {
                        this.original_isVisible = false;
                        if (!m_actionTracker.actionActorVisibility.ContainsKey(this.ID))
                            m_actionTracker.actionActorVisibility.Add(this.ID, false);
                    }
                }
            }

            m_currentAction = m_actions[actionID];
            m_updateInfo.actionInProgress = true;
            if (!(this.ID.Contains("helperActor") || this.ID.Contains("bossActor") || actionID.Equals("ending")))
                m_updateInfo.helperActionInProgress = true;
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
                m_updateInfo.bossPosition = m_position;
                moveTo(sAction.getGoal(), 0.048f);
            }

            else if (this.m_currentAction.getID().Contains("finalstage"))
            {
                m_updateInfo.finaledistance = true;
                m_updateInfo.bossPosition = m_position;
                moveTo(sAction.getGoal(), 0.006f);
                m_updateInfo.bossTarget = sAction.getGoal();
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
            foreach (var v in m_actions)
                v.Value.Reset();
            if (this.ID.Contains("bossActor"))
                m_updateInfo.bossPosition = m_position;
        }

    }
}
