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

        public void Load(ContentManager manager, AssetManager assets)
        {
            m_effect = assets.billboardEffect;
            m_effect.CurrentTechnique = m_effect.Techniques["BillboardingCameraAligned"];
            textures[0] = assets.chocoBillboard;
            m_mapTexture = assets.chocoBillboardForMap;
        }

        public void Update(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
        }
    }
}
