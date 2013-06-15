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
    /// Obstacle, that can be moved by the Player.
    /// </summary>
    class ObstacleMoveable : Obstacle
    {
        protected bool isOnSlipperyGround;

        public ObstacleMoveable(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            this.ID = id;
            this.m_position = pos;
            this.m_position.Y += 0.2f;
            this.m_original_position = this.m_position;
            this.isActive = false;
            this.original_isActive = false;
            this.m_updateInfo = updateInfo;
            this.isOnSlipperyGround = false;
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            this.currentspeed = 0;
            this.upvelocity = 0;
        }


        public override void load(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("wunderkugeltextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Toon");
            this.m_model = content.Load<Model>("wunderkugelmovable");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
        }


        public override void update()
        {
            fall();

            // Obstacle is sliding
            if (currentspeed != 0 && isOnSlipperyGround)
            {
                move();
            }
        }

        public override void collide(GameObject obj)
        {
            // TODO Test for Collison

            if (obj.GetType() == typeof(Platform))
            {
                // Obstacle sits on a Platform
                if (obj.getBoundingBox().Intersects(m_boundingBox))
                {
                    preventIntersection(obj);
                    Platform platform = (Platform) obj;
                    isOnSlipperyGround = platform.getSlippery();
                }
                else
                {
                    isonground = isonground || false;
                } 
            }
        }

        public override void hasCollidedWith(GameObject obj)
        {
            // getting pushed by the player
            if (obj.GetType() == typeof(CandyGuy))
            {
                this.isActive = true;
                this.currentspeed = obj.getCurrentSpeed();

                // Find out on which boundingbox side the collision occurs

                    BoundingBox bbSwitch = m_boundingBox;
                    float playerX = obj.getPosition().X;
                    float playerZ = obj.getPosition().Z;

                    // Obstacle should only be moved, if collided from the side

                        //Test if collison in X direction
                        if ( (playerX < bbSwitch.Min.X || playerX > bbSwitch.Max.X)
                            && playerZ < bbSwitch.Max.Z && playerZ > bbSwitch.Min.Z )
                        {
                            this.direction = new Vector3(obj.getDirection().X,0,0);
                            move();
                        }
                        // Test if collision in Z direction
                        if ((playerZ < bbSwitch.Min.Z || playerZ > bbSwitch.Max.Z)
                            && playerX < bbSwitch.Max.X && playerX > bbSwitch.Min.X)
                        {
                            this.direction = new Vector3(0, 0, obj.getDirection().Z);
                            move();
                        }


            }
        }


        /// <summary>
        /// Obstacle starts moving, when pushed by a Player
        /// </summary>
        /// <param name="direction">normalised Vector3 indicating the direction of the movement</param>
        /// <param name="slipperyGround">bool cointaining information about the platform, the object is currently on</param>
        /// <param name="speed">how fast the object ought to move</param>
        public void move()
        {
            Vector3 newPosition;
            Vector3 translate;

            // move Obstacle
                translate = currentspeed * direction;
                newPosition = this.getPosition() + translate;
                this.setPosition(newPosition);

            // move Bounding Box at the same time
            this.m_boundingBox.Min += translate;
            this.m_boundingBox.Max += translate;
        }


    }
}
