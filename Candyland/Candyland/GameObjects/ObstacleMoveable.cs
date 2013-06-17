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
            this.m_position.Y += 2.25f;
            this.m_original_position = this.m_position;
            this.isActive = false;
            this.original_isActive = false;
            this.m_updateInfo = updateInfo;
            this.isOnSlipperyGround = false;
            this.currentspeed = 0;
            this.upvelocity = 0;
        }


        public override void load(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("blockmovabletextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Toon");
            this.m_model = content.Load<Model>("blockmovable");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
        }


        public override void update()
        {
            // let the Object fall, if no collision with lower Objects
            fall();
            isonground = false;

            // Obstacle is sliding
            if (currentspeed != 0 && isOnSlipperyGround)
            {
                slide();
            }
        }

        #region collision

             protected override void collideWithPlatform(GameObject obj)
            {
                // Obstacle sits on a Platform, that can be slippery
                if (obj.getBoundingBox().Intersects(m_boundingBox))
                {
                    preventIntersection(obj);
                    Platform platform = (Platform) obj;
                    isOnSlipperyGround = platform.getSlippery();
                } 
            }

        #endregion

        public override void hasCollidedWith(GameObject obj)
        {
            // getting pushed by the player
            if (obj.GetType() == typeof(CandyGuy))
            {
                // Find out on which boundingbox side the collision occurs

                    BoundingBox bbSwitch = m_boundingBox;
                    float playerRight = obj.getPosition().X + (m_boundingBox.Max.X - m_boundingBox.Min.X) / 4;
                    float playerLeft = obj.getPosition().X - (m_boundingBox.Max.X - m_boundingBox.Min.X) / 4;
                    float playerFront = obj.getPosition().Z + (m_boundingBox.Max.Z - m_boundingBox.Min.Z) / 4;
                    float playerBack = obj.getPosition().Z - (m_boundingBox.Max.Z - m_boundingBox.Min.Z) / 4;
                    float playerTop = obj.getPosition().Y + (m_boundingBox.Max.Y - m_boundingBox.Min.Y) / 4;
                    float playerBottom = obj.getPosition().Y - (m_boundingBox.Max.Y - m_boundingBox.Min.Y) / 4;

                    // Obstacle should only be moved, if collided from the side

                    // Test if Player is beside the Obstacle and not on top
                    if (playerBottom < bbSwitch.Max.Y && playerTop > bbSwitch.Min.Y)
                    {
                        //Test if collison in X direction
                        if ((playerLeft < bbSwitch.Min.X || playerRight > bbSwitch.Max.X)
                            && playerBack < bbSwitch.Max.Z && playerFront > bbSwitch.Min.Z)
                        {
                            this.direction = new Vector3(obj.getDirection().X, 0, 0);
                            this.direction.Normalize();
                            if (isOnSlipperyGround)
                            {
                                currentspeed = GameConstants.obstacleSpeed;
                            }
                            move();
                        }
                        // Test if collision in Z direction
                        if ((playerBack < bbSwitch.Min.Z || playerFront > bbSwitch.Max.Z)
                            && playerLeft < bbSwitch.Max.X && playerRight > bbSwitch.Min.X)
                        {
                            this.direction = new Vector3(0, 0, obj.getDirection().Z);
                            this.direction.Normalize();
                            if (isOnSlipperyGround)
                            {
                                currentspeed = GameConstants.obstacleSpeed;
                            }
                            move();
                        }
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
                translate = GameConstants.obstacleSpeed * direction;
                newPosition = this.getPosition() + translate;
                this.setPosition(newPosition);
        }

        public void slide()
        {
            Vector3 newPosition;
            Vector3 translate;

            // move Obstacle
            translate = currentspeed * direction;
            newPosition = this.getPosition() + translate;
            this.setPosition(newPosition);
        }


    }
}
