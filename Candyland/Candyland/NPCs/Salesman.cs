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
            pos = pos - new Vector3(0.0f, 0.47f, 0.0f);
            base.init(id, pos, updateInfo, visible);
            m_text = message;
            int cutOff = id.LastIndexOf('.');
            m_levelID = id.Substring(0, cutOff);
        }

        public override void load(Microsoft.Xna.Framework.Content.ContentManager content, AssetManager assets)
        {
            this.m_texture = assets.salesmanTexture;
            this.m_original_texture = this.m_texture;
            this.effect = assets.commonShader;
            this.m_model = assets.salesman;
            this.m_original_model = this.m_model;
            // Bounding box is bigger than the model, so that the player can interact, when standing a bit away
            m_boundingBox = new BoundingBox(this.m_position - new Vector3(0.5f,0.5f,0.5f), this.m_position + new Vector3(0.5f,0.5f,0.5f));
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            base.load(content, assets);
        }

        public override void collide(GameObject obj)
        {
        }

        public override void hasCollidedWith(GameObject obj)
        {
            if(obj.GetType() == typeof(CandyGuy))
            {
                candyIsClose = true;
                ((CandyGuy)obj).setCloseEnoughToInteract();
                if (m_updateInfo.m_screenManager.Input.Equals(InputState.Continue))
                {
                    // greet player
                    CandyGuy guy = (CandyGuy)obj;
                    m_updateInfo.m_screenManager.ActivateNewScreen(new SalesmanDialogueScreen(m_text, m_levelID, m_updateInfo, guy.getBonusTracker().chocoCount, "Salesman"));                
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
