﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SkinnedModel;

namespace Candyland
{
    public abstract class Playable : DynamicGameObjects
    {
        protected Camera cam;                   //Kamera
        protected BonusTracker m_bonusTracker;
        public BonusTracker getBonusTracker() { return m_bonusTracker; }
        protected bool isthirdpersoncam = true;
        public bool getIsThirdPersonCam() { return isthirdpersoncam; }
        protected bool isOnSlipperyGround;
        public bool getIsOnSlippery() { return isOnSlipperyGround; }
        protected bool onNonSlipperyObject;

        protected bool isCloseEnoughToInteract = false;
        public void setCloseEnoughToInteract() { isCloseEnoughToInteract = true; }
        public void resetCloseEnoughToInteract() { isCloseEnoughToInteract = false; }

        public abstract void uniqueskill();

        public Model[] modelArray;
        public SkinningData[] skinningArray;
        public AnimationClip[] clipArray;

        /// <summary>
        /// Moves the Player and the Camera
        /// </summary>
        /// <param name="movex">movement in Camera - X Coordinates</param>
        /// <param name="movey">movement in Camera - Y Coordinates</param>
        /// <param name="camx">Sideways rotation of the Camera</param>
        /// <param name="camy">Up / Down Rotation of the Camera</param>
        public abstract void movementInput(float movex, float movey, float camx, float camy);



        /// <summary>
        /// Returns a float representing the CameraAngle
        /// </summary>
        /// <returns></returns>
        public float getCameraDir() { return cam.getDirection(); }

        public Vector3 getCameraPos() { return cam.getPosition(); }

        public  Matrix getProjectionM(){return cam.getProjectionMatrix();}

        public Matrix getViewM() { return cam.getviewMatrix(); }


        public override void update()
        {
            onNonSlipperyObject = false;
            // Reset if Player has fallen down
            if (m_position.Y < GameConstants.endOfWorld_Y) {
                m_updateInfo.reset = true;
            }
                
        }

        /// <summary>
        /// Switches between ThirdPerson- and Top-Down-Perspective
        /// </summary>
        public void switchCameraPerspective() 
        {
            if (isthirdpersoncam) 
            { 
                cam.changeToTopDown();
                isthirdpersoncam = !isthirdpersoncam;
            }else 
            {
                cam.changeToThirdPP();
                isthirdpersoncam = !isthirdpersoncam;
            }
        }


        /// <summary>
        /// Moves the Player, input should be between [-1,1]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected override void move(float x, float y, float z)
        {
            if (cam.isInThirdP())
            {
        
                if ((x != 0 || z!=0))
                {
                    float length = (float)Math.Sqrt(x * x + z * z);     //Calculate length of MovementVector
                    direction = new Vector3(x, 0, z);                   //Movement Vector
                    direction.Normalize();                              //Normalize MovementVector
                    currentspeed = length * 0.04f;                       //Scale MovementVector for different walking speeds
                    m_position += direction * currentspeed;             //Change ObjectPosition

                    m_boundingBox.Min += direction * currentspeed;
                    m_boundingBox.Max += direction * currentspeed;
                }
    
                cam.changeposition(m_position);                     //Change CameraPosition
            }
        }

        // resets player position to level start and triggers level reset
        // ToDo: use Fade-Out so level reset has time to happen
        
        public override void Reset()
        {
            upvelocity = 0;
            cam.changeToThirdPP();
            cam.resetangle();
        }

        public bool isInThirdP() { return cam.isInThirdP(); }

        public void startIntersection()
        {
            this.isonground = false;
            cam.startCollision();
        }

        public override void endIntersection()
        {
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
        }


        #region collision

        public override void collide(GameObject obj)
        {
            cam.collideWith(obj);
            if (obj.GetType() == typeof(BonbonFairy)) collideWithBonbonFairy(obj);

            base.collide(obj);
        }

        // Needs to be able to collect the chips
        protected override void collideWithChocoChip(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                obj.hasCollidedWith(this);
            }
        }

        // Needs to find out if the ground is slippery for players
        protected override void collideWithPlatform(GameObject obj)
        {
            // Obstacle sits on a Platform, that can be slippery
            if (obj.isVisible && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                preventIntersection(obj);
                Platform platform = (Platform)obj;
                if (!onNonSlipperyObject)
                {
                    switch (platform.getSlippery())
                    {
                        case 0: isOnSlipperyGround = false; onNonSlipperyObject = true; break;
                        case 1: isOnSlipperyGround = false; onNonSlipperyObject = true; break;
                        case 2: isOnSlipperyGround = true; break;
                    }
                }
                obj.hasCollidedWith(this);
            }
            
        }

        protected void collideWithBonbonFairy(GameObject obj)
        {
            if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
            {
                obj.hasCollidedWith(this);
            }
        }

        protected override void collideWithBreakingPlatform(GameObject obj)
        {
            collideWithPlatform(obj);
        }

        protected override void collideWithObstacle(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                currentspeed = 0;
                // player stands on the object
                if ((this.getBoundingBox().Min.Y - obj.getBoundingBox().Max.Y) < 0.01f)
                {
                    isOnSlipperyGround = false;
                    onNonSlipperyObject = true;
                }
                base.collideWithObstacle(obj);
            }
        }

        protected override void collideWithMovable(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                currentspeed = 0;
                // player stands on the object
                if ((this.getBoundingBox().Min.Y - obj.getBoundingBox().Max.Y) < 0.01f)
                {
                    isOnSlipperyGround = false;
                    onNonSlipperyObject = true;
                }
                base.collideWithMovable(obj);
            }
        }

        protected override void collideWithObstacleForSwitch(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                currentspeed = 0;
                // player stands on the object
                if ((this.getBoundingBox().Min.Y - obj.getBoundingBox().Max.Y) < 0.01f)
                {
                    isOnSlipperyGround = false;
                    onNonSlipperyObject = true;
                }
                base.collideWithObstacle(obj);
            }
        }
        #endregion

    }
}
