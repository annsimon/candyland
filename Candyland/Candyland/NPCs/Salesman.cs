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
    /// this NPC greets the player and then gives him four options: talk, shop, travel, leave
    /// hid ID should be equal to the level he's in
    /// </summary>
    class Salesman : GameObject
    {
        // Message the salesman has for the player, when he chooses to talk
        String m_text;
        string m_levelID;

        public Salesman(String id, Vector3 pos, UpdateInfo updateInfo, bool visible, String message)
        {
            base.init(id, pos, updateInfo, visible);
            m_text = message;
            int cutOff = id.LastIndexOf('.');
            m_levelID = id.Substring(0, cutOff);
        }

        public override void load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("Objekte/Schokolinse/schokolinsetextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Shaders/Toon");
            this.m_model = content.Load<Model>("Objekte/Schokolinse/schokolinse");
            this.m_original_model = this.m_model;
            // Bounding box is bigger than the model, so that the player can interact, when standing a bit away
            m_boundingBox = new BoundingBox(this.m_position - new Vector3(0.8f,0.5f,0.8f), this.m_position + new Vector3(0.8f,0.5f,0.8f));
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            base.load(content);
        }

        public override void collide(GameObject obj)
        {
        }

        public override void hasCollidedWith(GameObject obj)
        {
            if(obj.GetType() == typeof(CandyGuy))
            {
                KeyboardState keyState = Keyboard.GetState();

                if (keyState.IsKeyDown(Keys.Space) || keyState.IsKeyDown(Keys.Enter))
                {
                    // set as active teleport point, if not already done
                    if (!m_updateInfo.activeTeleports.Contains(m_levelID))
                        m_updateInfo.activeTeleports.Add(m_levelID);
                    // greet player
                    CandyGuy guy = (CandyGuy)obj;
                    m_updateInfo.m_screenManager.ActivateNewScreen(new SalesmanDialogueScreen(m_text, m_levelID, m_updateInfo, guy.getBonusTracker().chocoCount, "Images/DialogImages/BonbonFairy"));
                }
            }
        }

        public override void isNotCollidingWith(GameObject obj)
        {
        }

        public override void update()
        {
        }
    }
}
