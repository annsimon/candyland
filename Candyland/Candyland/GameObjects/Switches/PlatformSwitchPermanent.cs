using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Candyland
{
    /// <summary>
    /// Once stepped on this Switch, it stays activated or deactivated.
    /// </summary>
    class PlatformSwitchPermanent : PlatformSwitch
    {
        private SoundEffect sound;

        public PlatformSwitchPermanent(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
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

        public override void load(ContentManager content)
        {
            sound = content.Load<SoundEffect>("Sfx/Switch Activate8bit");
            this.m_activated_texture = content.Load<Texture2D>("Objekte/Plattformen/Schalter/schalterpermanent_aktiv");
            this.m_notActivated_texture = content.Load<Texture2D>("Objekte/Plattformen/Schalter/schalterpermanent_inaktiv");
            this.m_texture = this.m_notActivated_texture;
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Shaders/Shader");
            this.m_model = content.Load<Model>("Objekte/Plattformen/plattform_klein");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();

            base.load(content);
        }

        #endregion

        /// <summary>
        /// Updates the Switch's states.
        /// </summary>
        public override void update()
        {
            if (!isVisible)
                return;
            
            // Activate when first touch occurs
            if (!this.isActivated && this.isTouched == GameConstants.TouchedState.touched)
            {
                this.setActivated(true);
                float volume = 0.5f;
                float pitch = 0.0f;
                float pan = 0.0f;
                sound.Play(volume, pitch, pan);
                this.isTouched = GameConstants.TouchedState.stillTouched;
            }
        }

    }
}
