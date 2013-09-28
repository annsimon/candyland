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
    /// Platforms are Objects in the Game World on which the Player can walk.
    /// </summary>
    class Platform : GameObject
    {
        #region properties

        // 0 means not slippery
        // 1 means slippery for obstacles. Obstacles will slide over slippery platforms, when being pushed.
        // 2 means slippery for all. Obstacles and Players will slide over these platforms.
        protected int slipperyInfo;
        public int getSlippery() { return slipperyInfo; }


        protected bool isDoorToArea;
        public bool getIsDoorToArea() { return this.isDoorToArea; }
        protected string doorToAreaID;
        public string getDoorToAreaID() { return this.doorToAreaID; }

        protected bool isDoorToLevel;
        public bool getIsDoorToLevel() { return this.isDoorToLevel; }
        protected string doorToLevelID;
        public string getDoorToLevelID() { return this.doorToLevelID; }

        #endregion

        public Platform()
        {
        }

        public Platform(String id, Vector3 pos, int slippery, string areaDoorID, string levelDoorID, UpdateInfo updateInfo, bool visible, int size)
        {
            initialize(id, pos, slippery, areaDoorID, levelDoorID, updateInfo, visible, size);
        }

        #region initialization

        public void initialize(String id, Vector3 pos, int slippery, string areaDoorID, string levelDoorID, UpdateInfo updateInfo, bool visible, int size)
        {
            this.size = size;

            base.init(id, pos, updateInfo, visible);

            this.slipperyInfo = slippery;

            switch (slipperyInfo)
            {
                case 1: m_material.specular = new Vector4(0.6f, 0.6f, 0.6f, 1.0f);
                        m_material.shiny = 4;
                        break;
                case 2: m_material.specular = new Vector4(0.6f, 0.6f, 0.6f, 1.0f);
                        m_material.shiny = 4;
                        break;
            }

            if (areaDoorID == "x")
                this.isDoorToArea = false;
            else
            {
                this.isDoorToArea = true;
                this.doorToAreaID = areaDoorID;
            }
            if (levelDoorID == "x")
                this.isDoorToLevel = false;
            else
            {
                this.isDoorToLevel = true;
                this.doorToLevelID = levelDoorID;
            }
        }


        public override void load(ContentManager content, AssetManager assets)
        {
            if (this.GetType() != typeof(Platform))
            {
                base.load(content, assets);
                return;
            }

            switch (size)
            {
                case 1: loadSmall(assets); break;
                case 2: loadMedium(assets); break;
                case 3: loadLarge(assets); break;
                default: loadSmall(assets); break;
            }
            this.m_original_texture = this.m_texture;
            this.m_original_model = this.m_model;

            this.effect = assets.commonShader;
            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            base.load(content, assets);
        }

        public virtual void loadSmall(AssetManager assets)
        {
            switch (slipperyInfo)
            {
                case 0: this.m_texture = assets.platformTexture; break;
                case 1: this.m_texture = assets.platformTextureSlippery; break;
                case 2: this.m_texture = assets.platformTextureVerySlippery; break;
            }
            this.m_model = assets.platformSmall;
        }

        public virtual void loadMedium(AssetManager assets)
        {
            switch (slipperyInfo)
            {
                case 0: this.m_texture = assets.platformTexture; break;
                case 1: this.m_texture = assets.platformTextureSlippery; break;
                case 2: this.m_texture = assets.platformTextureVerySlippery; break;
            }
            this.m_model = assets.platformMedium;
        }

        public virtual void loadLarge(AssetManager assets)
        {
            switch (slipperyInfo)
            {
                case 0: this.m_texture = assets.platformTexture; break;
                case 1: this.m_texture = assets.platformTextureSlippery; break;
                case 2: this.m_texture = assets.platformTextureVerySlippery; break;
            }
            this.m_model = assets.platformLarge;
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

        public override void hasCollidedWith(GameObject obj)
        {
            obj.getBoundingBox();
            // When the Player steps on a Platform that functions as a Door to the next Level or Area,
            // UpdateInfo needs to be updated
            if (obj is Playable)
            {
                string[] idParts = this.ID.Split('.');
                if (this.isDoorToArea)
                {
                    this.m_updateInfo.playerIsOnAreaExit = true;
                }

                if (this.isDoorToLevel)
                {
                    this.m_updateInfo.playerIsOnLevelExit = true;

                    if (obj is CandyGuy && !m_updateInfo.guyHasTouchedDoorInThisUpdate)
                    {
                        // update door touched by candyguy
                        m_updateInfo.guyHasTouchedDoorInThisUpdate = true;

                        if (this.m_updateInfo.currentguyLevelID != (idParts[0] + "." + idParts[1]))
                        {
                            this.m_updateInfo.currentguyAreaID = idParts[0];
                            this.m_updateInfo.currentguyLevelID = idParts[0] + "." + idParts[1];

                            // set as active teleport point, if not already done
                            if (m_updateInfo.allTeleports.Contains(idParts[0] + "." + idParts[1])
                                && !m_updateInfo.activeTeleports.Contains(idParts[0] + "." + idParts[1]))
                                m_updateInfo.activeTeleports.Add(idParts[0] + "." + idParts[1]);

                            // automatically save the game, when entering a new level
                            m_updateInfo.m_screenManager.SceneManager.SaveGame();
                        }
                        this.m_updateInfo.nextguyLevelID = doorToLevelID;
                    }
                    else if (obj is CandyHelper && !m_updateInfo.helperHasTouchedDoorInThisUpdate)
                    {
                        // update door touched by helper
                        m_updateInfo.helperHasTouchedDoorInThisUpdate = true;

                        if (this.m_updateInfo.currenthelperLevelID != (idParts[0] + "." + idParts[1]))
                        {
                            this.m_updateInfo.currenthelperAreaID = idParts[0];
                            this.m_updateInfo.currenthelperLevelID = idParts[0] + "." + idParts[1];

                            // automatically save the game, when entering a new level
                            m_updateInfo.m_screenManager.SceneManager.SaveGame();
                        }
                        this.m_updateInfo.nexthelperLevelID = doorToLevelID;
                    }
                }

                if (m_triggersActionWithID != null)
                {
                    m_triggersActionOfObject.Trigger(m_triggersActionWithID);
                }
            }
        }

        #endregion
    }
}
