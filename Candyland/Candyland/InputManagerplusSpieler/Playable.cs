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
    abstract class Playable : GameObject
    {
        protected Camera cam;               //Kamera
        protected BonusTracker m_bonusTracker;
        protected Vector3 direction;        //Laufrichtung des Spielers x-z Ebene
        public Vector3 getDirection() { return direction; }
        protected float currentspeed;       //Momentane geschwindigkeit
        public float getCurrentSpeed() { return currentspeed; }
        protected float gravity;            //beschleinigungsfaktor in y richtung  
        protected float upvelocity;         //beschleinigungsfaktor un y richtung
        protected bool isthirdpersoncam = true;

        public abstract void jump();

        public abstract void moveTo(Vector3 goalpoint);

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

        public abstract void startIntersection();

        /// <summary>
        /// Switches between ThirdPerson- and Top-Down-Perspective
        /// </summary>
        public void switchCameraPerspective() 
        {
            if (isthirdpersoncam) 
            { 
                cam.changeToTopDown();
                isthirdpersoncam = !isthirdpersoncam;
            }
            else 
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
        public void move(float x, float y)
        {
            if (x != 0 && y != 0)
            {
                float length = (float)Math.Sqrt(x * x + y * y);     //Calculate length of MovementVector
                direction = new Vector3(x, 0, y);                   //Movement Vector
                direction.Normalize();                              //Normalize MovementVector
                currentspeed = length * 0.04f;                       //Scale MovementVector for different walking speeds
                m_position += direction * currentspeed;             //Change PLayerPosition
                cam.changeposition(m_position);                     //Change CameraPosition

                m_boundingBox.Min += direction * currentspeed;
                m_boundingBox.Max += direction * currentspeed;
            }
        }



    }
}
