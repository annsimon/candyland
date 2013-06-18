using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    /// <summary>
    /// ChocoChip are Objects in the Game World which the player can collect.
    /// </summary>
    class ChocoChip : GameObject
    {
        // ChocoChips can be collected by the player
        public bool isCollected { get; set; }

        private BonusTracker m_bonusTracker;

        public ChocoChip()
        {
        }

        public ChocoChip(String id, Vector3 pos, UpdateInfo updateInfo, bool visible, BonusTracker bonusTracker)
        {
            initialize(id, pos, updateInfo, visible, bonusTracker);
        }

        #region initialization

        public void initialize(String id, Vector3 pos, UpdateInfo updateInfo, bool visible, BonusTracker bonusTracker)
        {
            base.init(id, pos, updateInfo, visible);
            m_position.Y += 0.25f;
            m_original_position = m_position;

            m_bonusTracker = bonusTracker;
            m_bonusTracker.chocoChipState.Add(ID, false);
            m_bonusTracker.chocoTotal++;
        }

        public override void initialize()
        {
            isCollected = m_bonusTracker.chocoChipState[ID];
        }

        public void initialize(bool collected)
        {
            isCollected = collected;
            m_bonusTracker.chocoChipState[ID] = collected;
        }


        public override void load(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("schokolinsetextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Toon");
            this.m_model = content.Load<Model>("schokolinse");
            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
        }

        #endregion

        public override void update()
        {
        }

        #region collision

        public override void collide(GameObject obj)
        {
        }

        #endregion


        #region collision related

        public override void isNotCollidingWith(GameObject obj)
        {

        }

        public override void hasCollidedWith(GameObject obj)
        {
            // When the Player steps on a Platform, that functions as a Door to the next Level or Area,
            // UpdateInfo needs to be updated
            if (!isCollected && 
                (obj.GetType() == typeof(CandyGuy) || obj.GetType() == typeof(CandyHelper)))
            {
                isCollected = true;
                m_bonusTracker.chocoChipState[ID] = true;
                m_bonusTracker.chocoCount++;
            }
        }

        #endregion

        public override void draw()
        {
            if( isVisible && !isCollected )
                base.draw();
        }

        public override void Reset()
        {
        }
    }
}
