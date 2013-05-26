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

namespace Controller_test
{

    class Camera
    {

        private  Matrix viewMatrix;
        private  Matrix projectionMatrix;
        private float upangle;
        private float rotation;
        private Vector3 centerposition;
        private float offset = 3;
        private float upspeed = 0.01f;
        private float sidespeed = 0.1f;


        /// <summary>
        /// Creates a third person camera. standard viewdirection along the z axis
        /// </summary>
        /// <param name="pos">position of the point the camera is looking at</param>
        /// <param name="fov">field of view: recommended PiOverFour</param>
        /// <param name="aspectRatio">the aspectratio, may be: GraphicsDevice.Viewport.AspectRatio</param>
        /// <param name="nearPlane">distance of the nearPlane</param>
        /// <param name="farPlane">distance of the farPlane</param>

        public Camera(Vector3 pos, float fov, float aspectRatio, float nearPlane, float farPlane ) 
        {
            upangle = -0.5f;
            rotation = 0.0f;
            centerposition = pos;
            updatevMatrix();
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlane, farPlane);
        }
       
        /// <summary>
        /// Returns a Vector indicating the direction of the cam on the x-z plane
        /// </summary>
        /// <returns></returns>
        public float getDirection()
        {
            return rotation;//new Vector3((float)Math.Sin(rotation),0, (float) Math.Cos(rotation));
        }

        /// <summary>
        /// Used to update the Camera direction and position
        /// </summary>
        /// <param name="pos">position of the point the camera is looking at</param>
        /// <param name="x">rotation around the y axis</param>
        /// <param name="y">height rotation</param>
        public  void update(Vector3 pos, float x, float y) 
        {
            changeposition(pos);
            changeAngle(x, y);
        }
        
        
        public void changeposition(Vector3 pos) 
        {
            centerposition = pos;
            updatevMatrix();
        }

        /// <summary>
        /// Changes the ViewAngle of the Camera towards the Player
        /// </summary>
        /// <param name="x">Rotate around the y Axis.</param>
        /// <param name="y">Rotate around center up and down.</param>
        /// 
        public void changeAngle(float x, float y) 
        {
            upangle -= upspeed * y;
            rotation += sidespeed * x;

            if (upangle < -Math.PI * 0.35f) upangle = (float)-Math.PI * 0.35f;
            if (upangle > Math.PI * 0.01f) upangle = (float)Math.PI * 0.01f;

            updatevMatrix();
        }

        private void updatevMatrix() 
        {
            Vector3 posdiff = offset * new Vector3((float)-Math.Sin(rotation) * (float) Math.Cos(upangle),
                                                    (float)Math.Sin(upangle),
                                                    (float)Math.Cos(rotation)* (float)Math.Cos(upangle));

            viewMatrix = Matrix.CreateLookAt(centerposition - posdiff, centerposition, Vector3.Up);

        }

        public Matrix getviewMatrix() { return viewMatrix; }
        public Matrix getProjectionMatrix() { return projectionMatrix; }

    }
}
