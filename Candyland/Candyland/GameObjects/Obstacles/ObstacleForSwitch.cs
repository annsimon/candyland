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

        public override void load(ContentManager content, AssetManager assets)
        {
            if (!(this.GetType() == typeof(ObstacleForSwitch)))
            {
                base.load(content, assets);
                return;
            }

            switch (size)
            {
                case 0: loadLow(assets); break;
                case 1: loadSmall(assets); break;
                default: loadSmall(assets); break;
            }

            this.m_original_texture = this.m_texture;
            this.effect = assets.commonShader;
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            base.load(content, assets);
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

    }
}
