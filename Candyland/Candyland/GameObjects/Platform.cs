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


        public Platform()
        {
        }

        public Platform(String id, Vector3 pos, string areaDoorID, string levelDoorID)
        {
            this.ID = id;
            this.m_position = pos;
            this.isActive = false;
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

            //Add Platforms with door function to currentObjectsToBeCollided List in UpdateInfo

        }

        public override void initialize()
        {
        }

        public override void load(ContentManager content)
        {
            this.m_model = content.Load<Model>("plattform");

            this.calculateBoundingBox();
            Console.WriteLine("Min " + this.m_boundingBox.Min + " Max " + this.m_boundingBox.Max);
        }

        public override void collide(GameObject obj)
        {
            throw new NotImplementedException();
        }

        public override void update()
        {
        }
    }
}
