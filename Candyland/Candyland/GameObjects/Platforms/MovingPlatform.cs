using System;
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
        #region properties

        Vector3 start;
        Vector3 end;
        int sign = 1;
        int m_originalsign = 1;
        bool nowchangingdirection = false;

        #endregion

        public MovingPlatform(String id, Vector3 start, Vector3 end, UpdateInfo updateInfo, bool visible, int size) 
        {
            initialize(id, start, end, updateInfo, visible, size);
        }

        #region initialization

        public void initialize(String id, Vector3 start, Vector3 end, UpdateInfo updateInfo, bool visible, int size)
        {
            this.size = size;

            base.init(id, start, updateInfo, visible);

            this.start = start;
            this.end = end;
            currentspeed = 0.01f;
            direction = start - end;
            direction.Normalize();
            direction *= currentspeed;
            original_currentspeed = currentspeed;
            original_direction = direction;
        }

        public override void load(ContentManager content)
        {
            switch (size)
            {
                case 1: loadSmall(content); break;
                case 2: loadMedium(content); break;
                case 3: loadLarge(content); break;
            }
            this.m_original_texture = this.m_texture;
            this.m_original_model = this.m_model;

            this.effect = content.Load<Effect>("Shaders/Shader");
            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;

            base.load(content);
        }

        public void loadSmall(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("Objekte/Plattformen/plattformtextur_klein");
            this.m_model = content.Load<Model>("Objekte/Plattformen/plattform_klein");
        }

        public void loadMedium(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("Objekte/Plattformen/plattformtextur_mittel");
            this.m_model = content.Load<Model>("Objekte/Plattformen/plattform_mittel");
        }

        public void loadLarge(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("Objekte/Plattformen/plattformtextur_gross");
            this.m_model = content.Load<Model>("Objekte/Plattformen/plattform_gross");
        }

        #endregion

        public override void update()
        {
            if (!isVisible)
                return;

            nowchangingdirection = false;
            if (Math.Round(m_position.X, 2) == Math.Round(start.X, 2)
               && Math.Round(m_position.Y, 2) == Math.Round(start.Y, 2)
               && Math.Round(m_position.Z, 2) == Math.Round(start.Z, 2))
            {
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
            m_boundingBox.Min += direction;
            m_boundingBox.Max += direction;
        }

        #region collision

        public override void collide(GameObject obj) { }

        #endregion

        #region collision related

        public override void hasCollidedWith(GameObject obj) {
            base.hasCollidedWith(obj);
        }

        public override void isNotCollidingWith(GameObject obj)
        {
        }

        #endregion

        public override Matrix prepareForDrawing()
        {
            return base.prepareForDrawing();
        }

        public override void Reset()
        {
            base.Reset();
        }

    }
}
