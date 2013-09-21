using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Candyland
{
    class PlatformTeleporter : Platform
    {
        protected Vector3 teleportTarget;
        private SoundEffect sound;

        public PlatformTeleporter(String id, Vector3 pos, UpdateInfo updateInfo, bool visible, Vector3 target)
        {
            initialize(id, pos, updateInfo, visible, target);
        }

        #region initialization

        public void initialize(String id, Vector3 pos, UpdateInfo updateInfo, bool visible, Vector3 target)
        {
            base.init(id, pos, updateInfo, visible);

            this.teleportTarget = target;
        }
        public override void load(ContentManager content, AssetManager assets)
        {
            this.m_texture = assets.platformTextureTeleport;
            this.m_model = assets.platformSmall;
            sound = assets.teleportSound;

            this.m_original_texture = this.m_texture;
            this.m_original_model = this.m_model;

            this.effect = assets.commonShader;
            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            base.load(content, assets);
        }

        #endregion

        #region collision related

        public override void hasCollidedWith(GameObject obj)
        {
            // Position of the collided object (and therefore it's middle) is on the switch
            if (obj.getPosition().X < m_boundingBox.Max.X
                && obj.getPosition().X > m_boundingBox.Min.X
                && obj.getPosition().Z < m_boundingBox.Max.Z
                && obj.getPosition().Z > m_boundingBox.Min.Z)
            {
                float pitch = 0.0f;
                float pan = 0.0f;
                sound.Play(((float)m_updateInfo.soundVolume) / 10, pitch, pan);
                obj.setPosition(teleportTarget);
            }
        }

        #endregion
    }
}
