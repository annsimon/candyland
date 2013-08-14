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
        protected bool isPushing;
        public bool getPushing() { return isPushing; }
        public void setPushing(bool value) { this.isPushing = value; }
        protected float movedDistance = 0;

        public ObstacleMoveable(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            initialize(id, pos, updateInfo, visible);
        }

        #region initialization

        protected void initialize(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            base.initialize(id, pos, updateInfo, visible);

            this.isPushing = false;
            this.isOnSlipperyGround = false;
            this.currentspeed = 0;
            this.upvelocity = 0;
        }
        
        public override void load(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("Objekte/Obstacles/Movable/blockmovabletextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Shaders/Toon");
            this.m_model = content.Load<Model>("Objekte/Obstacles/Movable/blockmovable");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
        }
        #endregion

        public override void update()
        {
            if (!isVisible)
                return;
            base.update();
            // let the Object fall, if no collision with lower Objects
            fall();
            isonground = false;

            if (!isOnSlipperyGround)
            {
                currentspeed = 0;
                //direction = Vector3.Zero; // Maybe not needed
            }

            // Obstacle is sliding
            if (currentspeed != 0 && isOnSlipperyGround)
            {
                slide();
            }

            isPushing = false;
            // Obstacle was pushed and moves one step
            //TODO make sure there won't be rounding errors
            //if (isPushed && movedDistance < GameConstants.obstacleMoveDistance)
            //{
            //    move();
            //}
            //else
            //{
            //    movedDistance = 0;
            //    isPushed = false;
            //}

        }

        #region collision

             protected override void collideWithPlatform(GameObject obj)
            {
                // Obstacle sits on a Platform, that can be slippery
                if (obj.isVisible && obj.getBoundingBox().Intersects(m_boundingBox))
                {
                    preventIntersection(obj);
                    Platform platform = (Platform) obj;
                    switch (platform.getSlippery())
                    {
                        case 0: isOnSlipperyGround = false; break;
                        case 1: isOnSlipperyGround = true; break;
                        case 2: isOnSlipperyGround = true; break;
                    }
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
                     //does this even make sense? Isn't obj of type ObstacleMovable?
                     if (!(obj is Playable))
                         System.Console.WriteLine("collideMovable");
                     if (this.isOnSlipperyGround)
                     {
                         obj.hasCollidedWith(this);
                         return;
                     }
                     currentspeed = 0;
                     preventIntersection(obj);

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

                BoundingBox bbObstacle = m_boundingBox;
                BoundingBox bbPlayer = obj.getBoundingBox();

                // Obstacle should only be moved, if collided from the side

                    //Test if collison in X direction
                    if ( ( (bbObstacle.Max.X -bbPlayer.Min.X) < 0.01f ) || ( (bbPlayer.Max.X -bbObstacle.Min.X) < 0.01f ) )
                    {
                        this.direction = new Vector3(obj.getDirection().X, 0, 0);
                        this.direction.Normalize();
                        if (isOnSlipperyGround)
                        {
                            currentspeed = GameConstants.slippingSpeed;
                        }
                        //isPushed = true;
                        move();
                    }
                    // Test if collision in Z direction
                    if (((bbObstacle.Max.Z - bbPlayer.Min.Z) < 0.01f) || ((bbPlayer.Max.Z - bbObstacle.Min.Z) < 0.01f))
                    {
                        this.direction = new Vector3(0, 0, obj.getDirection().Z);
                        this.direction.Normalize();
                        if (isOnSlipperyGround)
                        {
                            currentspeed = GameConstants.slippingSpeed;
                        }
                        //isPushed = true;
                        move();
                    }
            }

            // getting pushed by other obstacle
            if (obj.GetType() == typeof(ObstacleMoveable))
            {
                // Check from which direction obj is coming
                    Vector3 vecBetweenObstacles = (this.getPosition() - obj.getPosition());
                    Vector3 objVec = obj.getDirection();
                    // look if both obj.direction equals vecBetweenObstacles
                    bool rightPushDirection = false;
                    if (vecBetweenObstacles.X > vecBetweenObstacles.Z && objVec.X > objVec.Z
                        || vecBetweenObstacles.X < vecBetweenObstacles.Z && objVec.X < objVec.Z)
                        rightPushDirection = true;
                    
                // pushing obstacle is sliding and pushed obstacle is on slippery ground
                if (obj.getCurrentSpeed() != 0 && this.isOnSlipperyGround && rightPushDirection)
                {
                    float speed = obj.getCurrentSpeed();
                    this.currentspeed = obj.getCurrentSpeed();
                    this.direction = obj.getDirection();
                    obj.setCurrentSpeed(0);
                }
                //else
                //{
                //    currentspeed = 0;
                //    preventIntersection(obj);
                //}
            }
        }

        public override void isNotCollidingWith(GameObject obj)
        {
        }

        #endregion

        #region actions

        /// <summary>
        /// Obstacle starts moving when pushed by a Player
        /// </summary>
        /// <param name="direction">normalised Vector3 indicating the direction of the movement</param>
        /// <param name="slipperyGround">bool cointaining information about the platform, the object is currently on</param>
        /// <param name="speed">how fast the object ought to move</param>
        public void move()
        {
            Vector3 newPosition;
            Vector3 translate;

            // move Obstacle
                translate = GameConstants.slippingSpeed * direction;
                //movedDistance += GameConstants.slippingSpeed;
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
