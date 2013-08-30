using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class HelperActor : ActionActor
    {
        public HelperActor(String id, Vector3 position, ActionTracker actionTracker, 
                            UpdateInfo updateInfo, bool visible)
        {
            Vector3 pos = new Vector3(position.X, position.Y + 0.4f, position.Z);
            initialize(id, pos, actionTracker, updateInfo, visible);
        }

        public void initialize(String id, Vector3 position, ActionTracker actionTracker, UpdateInfo updateInfo, bool visible)
        {
            m_actionTracker = actionTracker;
            m_actions = new Dictionary<String, Action>();
            m_dialogImage = "Images/DialogImages/Helper";
            base.init(id, position, updateInfo, visible);
        }

        public override void load(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("NPCs/TutorialGuy/buddytextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Shaders/Shader");
            this.m_model = content.Load<Model>("NPCs/TutorialGuy/buddy");
            this.m_original_model = this.m_model;
            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;

            base.load(content);
        }
    }
}
