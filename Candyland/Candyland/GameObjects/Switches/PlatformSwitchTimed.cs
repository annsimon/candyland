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
    class PlatformSwitchTimed : PlatformSwitch
    {
        private SoundEffect sound1;
        private SoundEffect sound2;
        protected double m_activeTime;

        public PlatformSwitchTimed(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            initialize(id, pos, updateInfo, visible);
        }

        #region initialization

        public void initialize(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            base.init(id, pos, updateInfo, visible);

            this.isActivated = false;
            this.m_switchGroups = new List<SwitchGroup>();
            m_activeTime = 0;
            m_material.specular = new Vector4(0.6f, 0.6f, 0.6f, 1.0f);
            m_material.shiny = 4;
        }

        public override void load(ContentManager content)
        {
            sound1 = content.Load<SoundEffect>("Sfx/Ticking8bit");
            sound2 = content.Load<SoundEffect>("Sfx/Switch Deactivate8bit");
            this.m_activated_texture = content.Load<Texture2D>("Objekte/Plattformen/Schalter/zeitschalteraktivtextur");
            this.m_notActivated_texture = content.Load<Texture2D>("Objekte/Plattformen/Schalter/zeitschaltertextur");
            this.m_texture = this.m_notActivated_texture;
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Shaders/Shader");
            this.m_model = content.Load<Model>("Objekte/Plattformen/plattform_klein");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
            // Needed here?
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;

            base.load(content);
        }

        #endregion


        /// <summary>
        /// Updates the Switch's states.
        /// </summary>
        public override void update()
        {
            if (this.isActivated)
                m_activeTime += m_updateInfo.gameTime.ElapsedGameTime.TotalSeconds;

            // Activate when first touch occurs
            if (this.isTouched == GameConstants.TouchedState.touched)
            {
                if (!this.isActivated)
                {
                    float volume = 0.7f;
                    float pitch = 0.0f;
                    float pan = 0.0f;
                    sound1.Play(volume, pitch, pan);
                    this.setActivated(true);
                    m_activeTime = 0;
                    this.isTouched = GameConstants.TouchedState.stillTouched;
                }
            }
            // Deactivate when timeout
            if ((m_activeTime > GameConstants.switchActiveTime))
            {
                float volume = 0.5f;
                float pitch = 0.0f;
                float pan = 0.0f;
                sound2.Play(volume, pitch, pan);
                this.setActivated(false);
            }
        }

    }
}
