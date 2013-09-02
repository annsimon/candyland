using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Candyland
{
    class Sun : Billboard
    {
        private Vector3[] positions;

        public Sun(GraphicsDevice graphicsDevice)
        {
            positions = new Vector3[1];
            positions[0] = new Vector3(0, 0, 0);
            Create(graphicsDevice,
                   positions);
            size = new Vector2(6.0f, 6.0f);
            textureCount = 1;
            textures = new Texture2D[textureCount];
        }

        public void Load(ContentManager manager)
        {
            m_effect = manager.Load<Effect>("Shaders/Billboard");
            m_effect.CurrentTechnique = m_effect.Techniques["BillboardingCameraAligned"];
            textures[0] = manager.Load<Texture2D>("Images/Billboards/Sun/Sun1");
            m_mapTexture = textures[0];
        }

        public void Update( GraphicsDevice graphicsDevice, Vector3 position, GameTime gameTime )
        {
            positions[0] = position + new Vector3(30, 30, 30);
            Create(graphicsDevice, positions);
        }
    }
}
