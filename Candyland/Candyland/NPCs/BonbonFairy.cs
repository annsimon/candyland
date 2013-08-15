using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Candyland
{
    /// <summary>
    /// this NPC can give advice when addressed by the player
    /// </summary>
    class BonbonFairy : GameObject
    {
        // Message the fairy has for the player
        String m_text;

        public BonbonFairy(String id, Vector3 pos, UpdateInfo updateInfo, bool visible)
        {
            base.init(id, pos, updateInfo, visible);
        }

        public override void load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            m_text = GameConstants.getFairyMessage(m_updateInfo.currentLevelID);
            this.m_texture = content.Load<Texture2D>("Objekte/Schokolinse/schokolinsetextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Shaders/Toon");
            this.m_model = content.Load<Model>("Objekte/Schokolinse/schokolinse");
            this.m_original_model = this.m_model;
            //TODO Bounding box is bigger than the model, so that the player can interact, when standing a bit away
            m_boundingBox = new BoundingBox(this.m_position - new Vector3(1,1,1), this.m_position + new Vector3(1,1,1));
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            base.load(content);
        }

        public override void collide(GameObject obj)
        {
            ;
        }

        public override void hasCollidedWith(GameObject obj)
        {
            if(obj.GetType() == typeof(CandyGuy))
            {
                KeyboardState keyState = Keyboard.GetState();

                if(keyState.IsKeyDown(Keys.B))
                    m_updateInfo.m_screenManager.ActivateNewScreen(new DialogListeningScreen(m_text, "Images/DialogImages/BonbonFairy"));
            }
        }

        public override void isNotCollidingWith(GameObject obj)
        {
            ;
        }

        public override void update()
        {
            ;
        }
    }
}
