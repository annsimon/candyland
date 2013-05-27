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
        public ObstacleMoveable(Vector3 pos)
        {
            this.Position = pos;
            this.isActive = false;
        }


        public override void Load(ContentManager content)
        {
            this.Model = content.Load<Model>("wunderkugelmovable"); ;
        }


        public override void Update()
        {
            // TODO Decide when to call move and with what parameters or maybe make different methodes like push and slide
            // this.move(...);
            // this.setActive(true); // when obstacle is moving
        }


        /// <summary>
        /// Obstacle starts moving, when pushed by a Player
        /// </summary>
        /// <param name="direction">normalised Vector3 indicating the direction of the movement</param>
        /// <param name="slipperyGround">bool cointaining information about the platform, the object is currently on</param>
        /// <param name="speed">how fast the object ought to move</param>
        public void move(bool slipperyGround, float playerSpeed, Vector3 direction)
        {
            //TODO This is only a first try and should be changed, when we are clear on how this might work

            Vector3 newPosition;

            // Obstacle moves with constant speed, while on slippery Platforms and not colliding
            if (slipperyGround)
            {
                newPosition = this.getPosition() + 0.3f * direction; // TODO add speed constant to Game Constants Class
                this.setPosition(newPosition);
            }
            // Obstacle moves with the same speed as the Player, who is pushing it
            else
            {
                newPosition = this.getPosition() + playerSpeed * direction;
                this.setPosition(newPosition);
            }

            // move Bounding Box at the same time
            Matrix translateMatrix = Matrix.CreateTranslation(newPosition);

            Vector3[] boxCorners = this.getBoundingBox().GetCorners();
            foreach (Vector3 element in this.getBoundingBox().GetCorners())
            {
                Vector3.Transform(element, translateMatrix);
            }
        }


    }
}
