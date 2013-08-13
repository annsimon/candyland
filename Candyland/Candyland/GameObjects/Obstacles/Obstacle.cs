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
            this.m_position.Y += 0.68f;
            this.m_original_position = this.m_position;
            this.size = size;
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

            this.effect = content.Load<Effect>("Shaders/Toon");

            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
        }

        public void loadSmall(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("Objekte/Obstacles/lakritztextur_klein");
            this.m_model = content.Load<Model>("Objekte/Obstacles/lakritzblock_klein");
        }

        public void loadMedium(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("Objekte/Obstacles/lakritztextur_mittel");
            this.m_model = content.Load<Model>("Objekte/Obstacles/lakritzblock_mittel");
        }

        public void loadLarge(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("Objekte/Obstacles/lakritztextur_gross");
            this.m_model = content.Load<Model>("Objekte/Obstacles/lakritzblock_gross");
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

        public override void draw()
        {
            base.draw();
        }

        

    }
}
