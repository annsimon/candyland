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
    abstract class Playable
    {
        protected Camera cam;               //Kamera
        protected Vector3 position;         //Position des Spielers
        protected Vector3 direction;        //Laufrichtung des Spielers x-z Ebene
        protected float currentspeed;       //Momentane geschwindigkeit
        protected float gravity;            //beschleinigungsfaktor in y richtung  
        protected float upvelocity;         //beschleinigungsfaktor un y richtung
        protected Model model;              //CharacterModel
        protected BoundingBox bbox;         //HitBox

        abstract public void loadContent(ContentManager content);

        abstract public void Draw();

        public abstract void jump();

        public abstract void moveTo(Vector3 goalpoint);

        public abstract void movementInput(float movex, float movey, float camx, float camy);




        public  Vector3 getPosition(){ return position; }

        // todo: FIX THIS!
        public float getCameraDir() { return cam.getDirection(); }

        public  Matrix getProjectionM(){return cam.getProjectionMatrix();}

        public Matrix getViewM() { return cam.getviewMatrix(); }

        /// <summary>
        /// Moves the Player, input should be between [-1,1]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void move(float x, float y)
        {
            if (x != 0 && y != 0)
            {
                float length = (float)Math.Sqrt(x * x + y * y);
                direction = new Vector3(x / length, 0, y / length);
                //TODO Länge normalisieren
                currentspeed = length * 0.1f;
                position += direction * currentspeed;
                cam.changeposition(position);
            }
        }


        public  BoundingBox getBoundingBox() { return bbox; }





    }
}
