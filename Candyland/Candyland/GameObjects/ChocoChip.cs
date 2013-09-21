using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private SoundEffect sound;
        private SoundEffect sound2;

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
            m_position.Y += 0.4f;
            m_original_position = m_position;
            isVisible = visible;
            original_isVisible = isVisible;
            m_bonusTracker = bonusTracker;
            m_bonusTracker.chocoChipState.Add(ID, false);
            m_bonusTracker.chocoTotal++;
            m_hasBillboard = true;
            m_updateInfo.objectsWithBillboards.Add(this);
            m_material.specular = new Vector4(0.7f, 0.7f, 0.7f, 1.0f);
            m_material.shiny = 5;
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


        public override void load(ContentManager content, AssetManager assets)
        {
            sound = assets.chocoSound;  // sound when collecting chocochips
            sound2 = assets.fanfareSound;
            this.m_texture = assets.chocoTexture;
            this.m_original_texture = this.m_texture;
            this.effect = assets.commonShader;
            this.m_model = assets.choco;
            this.m_original_model = this.m_model;
            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            this.m_bb = new ChocoBB(m_updateInfo.graphics, m_position);
            ((ChocoBB)m_bb).Load(content, assets);
            base.load(content, assets);
        }

        #endregion

        public override void update()
        {
            ((ChocoBB)m_bb).Update(m_updateInfo.graphics, m_updateInfo.gameTime);
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
            float pitch = 0.0f;
            float pan = 0.0f;
            if (!isCollected && 
                (obj.GetType() == typeof(CandyGuy) || obj.GetType() == typeof(CandyHelper)))
            {
                isCollected = true;
                isVisible = false;
                m_bonusTracker.chocoChipState[ID] = true;
                m_bonusTracker.chocoCount++;
                // sound options
                if (m_bonusTracker.chocoCount == m_bonusTracker.chocoTotal)
                {

                    sound2.Play(((float)m_updateInfo.soundVolume) / 10, pitch, pan);
                }
                else
                {
                    sound.Play(((float)m_updateInfo.soundVolume) / 10, pitch, pan);
                }
            }
        }

        #endregion

        public override Matrix prepareForDrawing()
        {
            if(!isCollected )
                return base.prepareForDrawing();
            return new Matrix();
        }

        public override void Reset()
        {
        }
    }
}
