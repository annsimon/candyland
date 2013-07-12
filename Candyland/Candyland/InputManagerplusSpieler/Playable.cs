using System;
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

namespace Candyland
{
    abstract class Playable : DynamicGameObjects
    {
        protected Camera cam;                   //Kamera
        protected BonusTracker m_bonusTracker;
        protected bool isthirdpersoncam = true;

        public abstract void uniqueskill();

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

        public  Matrix getProjectionM(){return cam.getProjectionMatrix();}

        public Matrix getViewM() { return cam.getviewMatrix(); }


        public override void update()
        {
            // Reset if Player has fallen down
            if(m_position.Y < GameConstants.endOfWorld_Y)
                m_updateInfo.reset = true;
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
        protected override void move(float x, float y)
        {
            if (cam.isInThirdP())
            {
                base.move(x, y);
                cam.changeposition(m_position);                     //Change CameraPosition
            }
        }

        // resets player position to level start and triggers level reset
        // ToDo: use Fade-Out so level reset has time to happen
        
        public override void Reset()
        {
            upvelocity = 0;
            cam.changeToThirdPP();
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

            base.collide(obj);
        }

        // Needs to be able to collect the chips
        protected override void collideWithChocoChip(GameObject obj)
        {
            if (obj.getBoundingBox().Intersects(m_boundingBox))
            {
                obj.hasCollidedWith(this);
            }
            else
            {
                obj.isNotCollidingWith(this);
            }
        }

        #endregion

    }
}
