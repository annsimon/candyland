using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Candyland
{
    class ChocoBB : Billboard
    {
        private Vector3[] positions;

        public ChocoBB(GraphicsDevice graphicsDevice, Vector3 position)
        {
            positions = new Vector3[1];
            positions[0] = position;
            Create(graphicsDevice,
                   positions);
            size = new Vector2(0.5f, 0.5f);
            textureCount = 1;
            textures = new Texture2D[textureCount];
        }

        public void Load(ContentManager manager)
        {
            m_effect = manager.Load<Effect>("Shaders/Billboard");
            m_effect.CurrentTechnique = m_effect.Techniques["BillboardingCameraAligned"];
            textures[0] = manager.Load<Texture2D>("Images/Billboards/Choco1");
            m_mapTexture = manager.Load<Texture2D>("Images/Billboards/ChocoMap");
        }

        public void Update(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
        }
    }
}
