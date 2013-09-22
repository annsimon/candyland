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
    /// Obstacles are Objects in the Game World, placed on the Platforms and block the Players movement.
    /// The basic Obstacle cannot be moved or destroyed by the Player.
    /// </summary>
    class Obstacle : DynamicGameObjects
    {
        public Obstacle()
        {
        }

        public Obstacle(String id, Vector3 pos, UpdateInfo updateInfo, bool visible, int size)
        {
            initialize(id, pos, updateInfo, visible, size);
        }

        #region initialization

        protected virtual void initialize(String id, Vector3 pos, UpdateInfo updateInfo, bool visible, int size = 1)
        {
            base.init(id, pos, updateInfo, visible);
            if (size == 1)
                this.m_position.Y += 0.56f;
            else if (size > 1)
                this.m_position.Y += 1.12f;
            else
                this.m_position.Y += 0.31f;
            this.m_original_position = this.m_position;
            this.size = size;
        }

        public override void load(ContentManager content, AssetManager assets)
        {
            if (!(this.GetType() == typeof(Obstacle)))
            {
                base.load(content, assets);
                return;
            }

            switch (size)
            {
                case 0: loadLow(assets); break;
                case 1: loadSmall(assets); break;
                case 2: loadLarge(assets); break;
                default: loadSmall(assets); break;
            }
            this.m_original_texture = this.m_texture;
            this.m_original_model = this.m_model;

            this.effect = assets.commonShader;

            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            base.load(content, assets);
        }

        public void loadLow(AssetManager assets)
        {
            this.m_texture = assets.obstacleTexture;
            this.m_model = assets.obstacleHalf;
        }

        public void loadSmall(AssetManager assets)
        {
            this.m_texture = assets.obstacleTexture;
            this.m_model = assets.obstacle;
        }

        public void loadLarge(AssetManager assets)
        {
            this.m_texture = assets.obstacleTexture;
            this.m_model = assets.obstacleLarge;
        }

        #endregion

        public override void update()
        {
            base.update();
        }

        #region collision related

        public override void isNotCollidingWith(GameObject obj)
        {
        }

        public override void hasCollidedWith(GameObject obj)
        {
        }

        #endregion

        public override Matrix prepareForDrawing()
        {
            return base.prepareForDrawing();
        }

        

    }
}
