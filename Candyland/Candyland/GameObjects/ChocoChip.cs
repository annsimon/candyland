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


        public override void isNotCollidingWith(GameObject obj)
        {
            
        }

        public ChocoChip()
        {
        }

        

        public override void collide(GameObject obj)
        {
            throw new NotImplementedException();
        }

        public ChocoChip(String id, Vector3 pos, UpdateInfo updateInfo, BonusTracker bonusTracker)
        {
            ID = id;
            m_position = pos;
            m_updateInfo = updateInfo;
            m_bonusTracker = bonusTracker;

            this.isActive = true;

            m_bonusTracker.chocoChipState.Add(ID, false);
            m_bonusTracker.chocoTotal++;
        }

        public override void initialize()
        {
            throw new NotImplementedException();
        }

        public void initialize(bool collected)
        {
            isCollected = collected;
            m_bonusTracker.chocoChipState[ID] = collected;
        }


        public override void load(ContentManager content)
        {
            this.m_model = content.Load<Model>("schokolinse");
            this.calculateBoundingBox();
            Console.WriteLine("Min " + this.m_boundingBox.Min + " Max " + this.m_boundingBox.Max);
        }


        public override void update()
        {
        }


        public override void hasCollidedWith(GameObject obj)
        {
            // When the Player steps on a Platform, that functions as a Door to the next Level or Area,
            // UpdateInfo needs to be updated
            if (!isCollected && obj.GetType() == typeof(CandyGuy))
            {
                isCollected = true;
                m_bonusTracker.chocoChipState[ID] = true;
                m_bonusTracker.chocoCount++;
            }
        }

        public override void draw()
        {
            if( !isCollected )
                base.draw();
        }
    }
}
