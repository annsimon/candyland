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

namespace Candyland
{
    class MovingPlatform : Platform
    {

        Vector3 start;
        Vector3 end;
        int sign = 1;
        int m_originalsign = 1;
        bool nowchangingdirection = false;

        public MovingPlatform(String id, Vector3 start, Vector3 end, UpdateInfo info) {
            this.ID = id;
            this.m_position = start;
            this.start = start;
            this.end = end;
            this.m_original_position = start;
            m_updateInfo = info;
            currentspeed = 0.01f;
            direction =  start - end;
            direction.Normalize();
            direction *= currentspeed;
            original_currentspeed = currentspeed;
            original_direction = direction;

        }

        public override void collide(GameObject obj) { }

        public override void draw()
        {
            base.draw();
        }

        public override void hasCollidedWith(GameObject obj) {
        }

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
            this.m_texture = content.Load<Texture2D>("plattformtextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Toon");
            this.m_model = content.Load<Model>("plattform");
            this.m_original_model = this.m_model;
            this.calculateBoundingBox();
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void update()
        {
            nowchangingdirection = false;
            if (Math.Round(m_position.X, 2) == Math.Round(start.X, 2)
               && Math.Round(m_position.Y, 2) == Math.Round(start.Y, 2)
               && Math.Round(m_position.Z, 2) == Math.Round(start.Z, 2)) {
                   nowchangingdirection = true;
                   direction *= -1;
            }

            else if (Math.Round(m_position.X, 2) == Math.Round(end.X, 2)
               && Math.Round(m_position.Y, 2) == Math.Round(end.Y, 2)
               && Math.Round(m_position.Z, 2) == Math.Round(end.Z, 2))
            {
                nowchangingdirection = true;
                direction *= -1;
            }
            m_position += direction;
            m_boundingBox.Min+= direction;
            m_boundingBox.Max += direction;

            
        }


    }
}
