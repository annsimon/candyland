using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class ObstacleForSwitch : Obstacle
    {
        public ObstacleForSwitch(String id, Vector3 pos, UpdateInfo updateInfo, bool visible, int size)
        {
            initialize(id, pos, updateInfo, visible, size);
        }

        #region initialization

        protected override void initialize(string id, Vector3 pos, UpdateInfo updateInfo, bool visible, int size = 1)
        {
            base.initialize(id, pos, updateInfo, visible, size);
        }

        public override void load(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("Objekte/Obstacles/obstacletextur_switch");
            if (!(this.GetType() == typeof(ObstacleForSwitch)))
            {
                base.load(content);
                return;
            }

            switch (size)
            {
                case 0: loadLow(content); break;
                case 1: loadSmall(content); break;
                default: loadSmall(content); break;
            }

            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Shaders/Shader");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            base.load(content);
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
        }

        #region collision

        // nothing to do here so far

        #endregion

        #region collision related

        public override void isNotCollidingWith(GameObject obj)
        {
        }

        public override void hasCollidedWith(GameObject obj)
        {
        }

        #endregion
    }
}
