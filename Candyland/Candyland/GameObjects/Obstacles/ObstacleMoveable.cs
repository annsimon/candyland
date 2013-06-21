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

        public ObstacleMoveable(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            initialize(id, pos, updateInfo, visible);
        }

        #region initialization
        protected override void initialize(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            base.initialize(id, pos, updateInfo, visible);

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
        #endregion

        public override void update()
        {
            base.update();
            // let the Object fall, if no collision with lower Objects
            fall();
            isonground = false;

            if (!isOnSlipperyGround)
            {
                currentspeed = 0;
            }

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
                if (obj.isVisible && obj.getBoundingBox().Intersects(m_boundingBox))
                {
                    preventIntersection(obj);
                    Platform platform = (Platform) obj;
                    isOnSlipperyGround = platform.getSlippery();
                } 
            }

             protected override void collideWithMovingPlatform(GameObject obj)
             {
                 // Object sits on a Platform
                 if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
                 {
                     currentspeed = 0;
                     preventIntersection(obj);
                     Vector3 diff = obj.getDirection();

                     if (diff.Y > 0) diff.Y = 0;

                     m_boundingBox.Min += diff;
                     m_boundingBox.Max += diff;
                     m_position += diff;

                     obj.hasCollidedWith(this);
                 }
             }

             protected override void collideWithObstacle(GameObject obj)
             {
                 if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
                 {
                     currentspeed = 0;
                     preventIntersection(obj);
                 }
             }

             protected override void collideWithBreakable(GameObject obj)
             {
                 if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
                 {
                     currentspeed = 0;
                     preventIntersection(obj);
                 }
             }
             protected override void collideWithMovable(GameObject obj)
             {
                 if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
                 {
                     if (!(obj is Playable))
                         System.Console.WriteLine("collideMovable");
                     currentspeed = 0;
                     preventIntersection(obj);
                     obj.hasCollidedWith(this);
                 }
                 else
                 {
                     obj.isNotCollidingWith(this);
                 }
             }

             protected override void collideWithObstacleForSwitch(GameObject obj)
             {
                 if (obj.isVisible && !obj.getID().Equals(this.ID) && obj.getBoundingBox().Intersects(m_boundingBox))
                 {
                     currentspeed = 0;
                     preventIntersection(obj);
                 }
             }

        #endregion

        #region collision related

        public override void hasCollidedWith(GameObject obj)
        {
            // getting pushed by the player
            if (obj.GetType() == typeof(CandyGuy))
            {
                // Find out on which boundingbox side the collision occurs

                BoundingBox bbSwitch = m_boundingBox;
                BoundingBox bbPlayer = obj.getBoundingBox();

                // Obstacle should only be moved, if collided from the side

                    //Test if collison in X direction
                    if ( ( (bbSwitch.Max.X -bbPlayer.Min.X) < 0.01f ) || ( (bbPlayer.Max.X -bbSwitch.Min.X) < 0.01f ) )
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
                    if (((bbSwitch.Max.Z - bbPlayer.Min.Z) < 0.01f) || ((bbPlayer.Max.Z - bbSwitch.Min.Z) < 0.01f))
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

        public override void isNotCollidingWith(GameObject obj)
        {
        }

        #endregion

        #region actions

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

        #endregion

    }
}
