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
            this.m_position.Y += 0.23f;
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
            // let the Object fall, if no collision with lower Objects
            fall();
            isonground = false;

            // Obstacle is sliding
            if (currentspeed != 0 && isOnSlipperyGround)
            {
                move();
            }
        }

        #region collision

        public override void collide(GameObject obj)
        {
            if (obj.GetType() == typeof(Platform)) collideWithPlatform(obj);
            //if (obj.GetType() == typeof(Obstacle)) collideWithObstacle(obj); // may not be called for itself!!!
            if (obj.GetType() == typeof(ObstacleBreakable)) collideWithBreakable(obj);
            if (obj.GetType() == typeof(ObstacleMoveable)) collideWithMovable(obj);
            if (obj.GetType() == typeof(PlatformSwitchPermanent)) collideWithSwitchPermanent(obj);
            if (obj.GetType() == typeof(PlatformSwitchTemporary)) collideWithSwitchTemporary(obj);
            if (obj.GetType() == typeof(ChocoChip)) collideWithChocoChip(obj); 
        }

             private void collideWithPlatform(GameObject obj)
            {
                // Obstacle sits on a Platform
                if (obj.getBoundingBox().Intersects(m_boundingBox))
                {
                    preventIntersection(obj);
                    Platform platform = (Platform) obj;
                    isOnSlipperyGround = platform.getSlippery();
                } 
            }
            private void collideWithObstacle(GameObject obj) {
                if (obj.getBoundingBox().Intersects(m_boundingBox))
                {
                    preventIntersection(obj);
                }
            }
            private void collideWithSwitchPermanent(GameObject obj) {
                if (obj.getBoundingBox().Intersects(m_boundingBox))
                {
                    preventIntersection(obj);
                    obj.hasCollidedWith(this);
                }
                else
                {
                    obj.isNotCollidingWith(this);
                }
            }
            private void collideWithSwitchTemporary(GameObject obj) {
                if (obj.getBoundingBox().Intersects(m_boundingBox))
                {
                    preventIntersection(obj);
                    obj.hasCollidedWith(this);
                }
                else
                {
                    obj.isNotCollidingWith(this);
                }
            }
            private void collideWithBreakable(GameObject obj) {
                if (obj.getBoundingBox().Intersects(m_boundingBox) && !obj.isdestroyed)
                {
                    preventIntersection(obj);
                }
            }
            private void collideWithMovable(GameObject obj) {
                if (obj.getBoundingBox().Intersects(m_boundingBox)) {
                    preventIntersection(obj);
                    obj.hasCollidedWith(this);
                }
                else
                {
                    obj.isNotCollidingWith(this);
                }
            }

            private void collideWithChocoChip(GameObject obj) {
                if (obj.getBoundingBox().Intersects(m_boundingBox))
                {
                    preventIntersection(obj);
                }
            }

        #endregion

        public override void hasCollidedWith(GameObject obj)
        {
            // getting pushed by the player
            if (obj.GetType() == typeof(CandyGuy))
            {
                this.isActive = true;

                // Find out on which boundingbox side the collision occurs

                    BoundingBox bbSwitch = m_boundingBox;
                    float playerRight = obj.getPosition().X + (m_boundingBox.Max.X - m_boundingBox.Min.X) / 2;
                    float playerLeft = obj.getPosition().X - (m_boundingBox.Max.X - m_boundingBox.Min.X) / 2;
                    float playerFront = obj.getPosition().Z + (m_boundingBox.Max.Z - m_boundingBox.Min.Z) / 2;
                    float playerBack = obj.getPosition().Z - (m_boundingBox.Max.Z - m_boundingBox.Min.Z) / 2;
                    float playerTop = obj.getPosition().Y + (m_boundingBox.Max.Y - m_boundingBox.Min.Y) / 2;
                    float playerBottom = obj.getPosition().Y - (m_boundingBox.Max.Y - m_boundingBox.Min.Y) / 2;

                    // Obstacle should only be moved, if collided from the side

                    // Test if Player is beside the Obstacle and not on top
                    if (playerBottom < bbSwitch.Max.Y && playerTop > bbSwitch.Min.Y)
                    {
                        //Test if collison in X direction
                        if ((playerLeft < bbSwitch.Min.X || playerRight > bbSwitch.Max.X)
                            && playerBack < bbSwitch.Max.Z && playerFront > bbSwitch.Min.Z)
                        {
                            this.direction = new Vector3(obj.getDirection().X, 0, 0);
                            move();
                        }
                        // Test if collision in Z direction
                        if ((playerBack < bbSwitch.Min.Z || playerFront > bbSwitch.Max.Z)
                            && playerLeft < bbSwitch.Max.X && playerRight > bbSwitch.Min.X)
                        {
                            this.direction = new Vector3(0, 0, obj.getDirection().Z);
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


    }
}
