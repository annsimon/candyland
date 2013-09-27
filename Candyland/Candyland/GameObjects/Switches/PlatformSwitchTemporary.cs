using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkinnedModel;
using Microsoft.Xna.Framework.Audio;

namespace Candyland
{
    /// <summary>
    /// Switch is only activated, when a Player (or an Obstacle) is standing on the Platform.
    /// </summary>
    class PlatformSwitchTemporary : PlatformSwitch
    {
        private SoundEffect sound1;
        private SoundEffect sound2;
        protected bool wasTouched = false;

        public PlatformSwitchTemporary(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            initialize(id, pos, updateInfo, visible);
        }

        #region initialization

        protected void initialize(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            base.init(id, pos, updateInfo, visible);
            this.isActivated = false;
            this.m_switchGroups = new List<SwitchGroup>();
            m_material.specular = new Vector4(0.6f, 0.6f, 0.6f, 1.0f);
            m_material.shiny = 4;
        }

        public override void load(ContentManager content, AssetManager assets)
        {
            sound1 = assets.switchActivateSound;
            sound2 = assets.switchDeactivateSound;
            this.m_activated_texture = assets.switchTemporaryActiveTexture;
            this.m_notActivated_texture = assets.switchTemporaryTexture;
            this.m_texture = this.m_notActivated_texture;
            this.m_original_texture = this.m_texture;
            this.effect = assets.commonShader;
            this.m_model = assets.platformSmall;
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();

            base.load(content, assets);
        }

        #endregion


        /// <summary>
        /// Updates the Switch's states.
        /// </summary>
        public override void update()
        {
            if (!isVisible)
                return;

            if (this.isTouched == GameConstants.TouchedState.touched
                || this.isTouched == GameConstants.TouchedState.stillTouched )
            {
                // Activate when touch occurs and was deactivated before
                if (!this.isActivated)
                {
                    this.setActivated(true);
                    playActivated(true);
                    this.isTouched = GameConstants.TouchedState.stillTouched;
                    this.wasTouched = true;
                }
            }
            // Deactivate when not touched
            else if (this.isActivated && this.isTouched == GameConstants.TouchedState.notTouched
                     && this.wasTouched)
            {
                this.setActivated(false);
                playDeactivated(true);
                this.wasTouched = false;
            }
        }

        public override void playActivated(bool direct)
        {
            if (belongsToOrdered && direct)
                return;
            float pitch = 0.0f;
            float pan = 0.0f;
            sound1.Play((float)m_updateInfo.soundVolume / 10, pitch, pan);
        }

        public override void playDeactivated(bool direct)
        {
            if (belongsToOrdered && direct)
                return;
            float pitch = 0.0f;
            float pan = 0.0f;
            sound2.Play(((float)m_updateInfo.soundVolume) / 10, pitch, pan);
        }
    }
}
