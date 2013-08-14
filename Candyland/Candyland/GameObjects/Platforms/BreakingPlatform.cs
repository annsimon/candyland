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
    /// Platform that starts breaking apart when stepped on
    /// </summary>
    class BreakingPlatform : Platform
    {
        protected bool isBreaking;
        protected double timeSincedSteppedOn;

        public BreakingPlatform(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            initialize(id, pos, updateInfo, visible);
        }

        public void initialize(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            base.init(id, pos, updateInfo, visible);

            this.isBreaking = false;
            timeSincedSteppedOn = 0;
        }

        public override void load(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("Objekte/Plattformen/plattformtextur_klein");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Shaders/Shader");
            this.m_model = content.Load<Model>("Objekte/Plattformen/plattform_klein");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;

            base.load(content);
        }

        public override void hasCollidedWith(GameObject obj)
        {
            if (isBreaking) return;

            // Position of the collided object (and therefore it's middle) is on the platform
            if (obj.getPosition().X < m_boundingBox.Max.X
                && obj.getPosition().X > m_boundingBox.Min.X
                && obj.getPosition().Z < m_boundingBox.Max.Z
                && obj.getPosition().Z > m_boundingBox.Min.Z)
            {
                isBreaking = true;
            }
        }

        public override void update()
        {
            if (isBreaking)
                timeSincedSteppedOn += m_updateInfo.gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSincedSteppedOn >= GameConstants.breakTime)
                this.isVisible = false;
        }
    }
}
