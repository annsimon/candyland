using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland.GameObjects
{
    class MovingPlatform : Platform
    {

        Vector3 start;
        Vector3 end;
        int sign = 1;
        int m_originalsign = 1;

        public MovingPlatform(String id, Vector3 start, Vector3 end) {
            this.ID = id;
            this.m_position = start;
            this.start = start;
            this.end = end;
            this.m_original_position = start;
        }

        private const float speed = 0.1f;

        public override void collide(GameObject obj) { }

        public override void draw()
        {
            base.draw();
        }

        public override void hasCollidedWith(GameObject obj) { }

        public override void initialize()
        {
            throw new NotImplementedException();
        }

        public override void isNotCollidingWith(GameObject obj)
        {
            throw new NotImplementedException();
        }

        public override void load(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void update()
        {
            Vector3 direction = end - start;
            direction.Normalize();
            direction *= speed;
            if (Math.Round(m_position.X, 1) == Math.Round(start.X, 1)
               && Math.Round(m_position.Y, 1) == Math.Round(start.Y, 1)
               && Math.Round(m_position.Z, 1) == Math.Round(start.Z, 1)) {
                   sign *= -1;
            }

            if (Math.Round(m_position.X, 1) == Math.Round(end.X, 1)
               && Math.Round(m_position.Y, 1) == Math.Round(end.Y, 1)
               && Math.Round(m_position.Z, 1) == Math.Round(end.Z, 1))
            {
                sign *= -1;
            }
            m_position += direction;
            
        }


    }
}
