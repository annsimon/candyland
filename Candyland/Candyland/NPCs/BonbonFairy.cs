﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkinnedModel;

namespace Candyland
{
    /// <summary>
    /// this NPC can give advice when addressed by the player or get the candyhelper
    /// </summary>
    class BonbonFairy : GameObject
    {
        // Message the fairy has for the player
        String m_text;

        bool isTeleportFairy = false;

        public BonbonFairy(String id, Vector3 pos, UpdateInfo updateInfo, bool visible, String message)
        {
            pos = pos + new Vector3(0.0f, 1.0f, 0.0f);
            base.init(id, pos, updateInfo, visible);
            if (message.Equals("teleport"))
                isTeleportFairy = true;
            else
                m_text = message;
        }

        public override void load(Microsoft.Xna.Framework.Content.ContentManager content, AssetManager assets)
        {
            if (isTeleportFairy)
                this.m_texture = assets.fairyRedTexture;
            else
                this.m_texture = assets.fairyBlueTexture;
            this.m_original_texture = this.m_texture;
            this.effect = assets.commonShader;
            this.m_model = assets.fairy;
            this.m_original_model = this.m_model;
            // Bounding box is bigger than the model, so that the player can interact, when standing a bit away
            m_boundingBox = new BoundingBox(this.m_position - new Vector3(0.5f, 1, 0.5f), this.m_position + new Vector3(0.5f, 0.2f, 0.5f));
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            base.load(content, assets);

            //AnimationClip clip = m_skinningData.AnimationClips["ArmatureAction"];

            //animationPlayer.StartClip(clip);
        }

        public override void collide(GameObject obj)
        {
        }
        public override void hasCollidedWith(GameObject obj)
        {
            if (obj.GetType() == typeof(CandyHelper))
                helperIsClose = true;
            else
            if(obj.GetType() == typeof(CandyGuy))
            {        
                candyIsClose = true;
                ((CandyGuy)obj).setCloseEnoughToInteract();
                if (m_updateInfo.m_screenManager.Input.Equals(InputState.Continue))
                {
                    // ask to teleport the helper
                    if (isTeleportFairy)
                    {
                        CandyGuy guy = (CandyGuy)obj;
                        CandyHelper helper = guy.getCandyHelper();
                        Vector3 teleportPosition = this.getPosition();
                        teleportPosition.Y -= 1;
                        m_updateInfo.m_screenManager.ActivateNewScreen(new TeleportFairyDialog(helper, m_updateInfo, teleportPosition, "BonbonRed"));
                        
                    }
                    // show fairy message
                    else
                        m_updateInfo.m_screenManager.ActivateNewScreen(new DialogListeningScreen(m_text, "BonbonBlue"));
                }
            }
            if (obj.GetType() == typeof(CandyHelper) && !m_updateInfo.candyselected)
            {
                if (m_updateInfo.m_screenManager.Input.Equals(InputState.Continue))
                {
                    // greet helper
                    if (isTeleportFairy)
                    {
                        m_updateInfo.m_screenManager.ActivateNewScreen(new DialogListeningScreen("Hallo, Süßer!", "BonbonRed"));
                    }
                    // show fairy message
                    else
                        m_updateInfo.m_screenManager.ActivateNewScreen(new DialogListeningScreen(m_text, "BonbonBlue"));
                }
            }
        }

        public override void update()
        {
        //animationPlayer.Update(m_updateInfo.gameTime.ElapsedGameTime, true, Matrix.Identity);
        }
    }
}
