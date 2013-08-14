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

        // Obstacles will slide over slippery platforms, when being pushed
        protected bool isSlippery;
        public bool getSlippery() { return this.isSlippery; }
        public void setSlippery(bool value) { this.isSlippery = value; }

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

        public Platform(String id, Vector3 pos, bool slippery, string areaDoorID, string levelDoorID, UpdateInfo updateInfo, bool visible, int size)
        {
            initialize(id, pos, slippery, areaDoorID, levelDoorID, updateInfo, visible, size);
        }

        #region initialization

        public void initialize(String id, Vector3 pos, bool slippery, string areaDoorID, string levelDoorID, UpdateInfo updateInfo, bool visible, int size)
        {
            this.size = size;

            base.init(id, pos, updateInfo, visible);

            this.isSlippery = slippery;

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


        public override void load(ContentManager content)
        {
            if (this.GetType() != typeof(Platform))
            {
                base.load(content);
                return;
            }

            switch (size)
            {
                case 1: loadSmall(content); break;
                case 2: loadMedium(content); break;
                case 3: loadLarge(content); break;
            }
            this.m_original_texture = this.m_texture;
            this.m_original_model = this.m_model;

            this.effect = content.Load<Effect>("Shaders/Shader");
            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            base.load(content);
        }

        public void loadSmall(ContentManager content)
        {
            if (isSlippery)
                this.m_texture = content.Load<Texture2D>("Objekte/Plattformen/Slippery/plattformtexturslippery_klein");
            else
                this.m_texture = content.Load<Texture2D>("Objekte/Plattformen/plattformtextur_klein");
            this.m_model = content.Load<Model>("Objekte/Plattformen/plattform_klein");
        }

        public void loadMedium(ContentManager content)
        {
            if (isSlippery)
                this.m_texture = content.Load<Texture2D>("Objekte/Plattformen/Slippery/plattformtexturslippery_mittel");
            else
                this.m_texture = content.Load<Texture2D>("Objekte/Plattformen/plattformtextur_mittel");
            this.m_model = content.Load<Model>("Objekte/Plattformen/plattform_mittel");
        }

        public void loadLarge(ContentManager content)
        {
            if (isSlippery)
                this.m_texture = content.Load<Texture2D>("Objekte/Plattformen/Slippery/plattformtexturslippery_gross");
            else
                this.m_texture = content.Load<Texture2D>("Objekte/Plattformen/plattformtextur_gross");
            this.m_model = content.Load<Model>("Objekte/Plattformen/plattform_gross");
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
            // When the Player steps on a Platform that functions as a Door to the next Level or Area,
            // UpdateInfo needs to be updated
            if (obj is Playable)
            {
                string[] idParts = this.ID.Split('.');
                if (this.isDoorToArea)
                {
                    this.m_updateInfo.playerIsOnAreaExit = true;
                    this.m_updateInfo.areaAfterExitID = this.doorToAreaID;
                    this.m_updateInfo.levelAfterExitID = this.doorToLevelID;

                    this.m_updateInfo.currentAreaID = idParts[0];
                    this.m_updateInfo.currentLevelID = idParts[0]+"."+idParts[1];
                }
                if(this.isDoorToLevel)
                {
                    this.m_updateInfo.playerIsOnLevelExit = true;
                    this.m_updateInfo.levelAfterExitID = this.doorToLevelID;

                    this.m_updateInfo.currentAreaID = idParts[0];
                    this.m_updateInfo.currentLevelID = idParts[0] + "." + idParts[1];
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
