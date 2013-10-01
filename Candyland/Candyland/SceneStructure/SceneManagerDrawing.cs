using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using SkinnedModel;

namespace Candyland
{
    public partial class SceneManager
    {
        public void Draw(GameTime gameTime)
        {
            CreateShadowMap();            

            DrawModel(player.GetModelGroup(), player.prepareForDrawing(), false);
            if (GameConstants.boundingBoxRendering)
                BoundingBoxRenderer.Render(player.getBoundingBox(), m_graphics, m_updateInfo.viewMatrix, m_updateInfo.projectionMatrix, Color.White);

            // draw the area the player currently is in and the two
            // adjacent ones
            string currentArea;

            currentArea = m_updateInfo.currentguyLevelID.Split('.')[0];

// ANNE fragen
            Area currArea = m_areas[currentArea];
            List<GameObject> currentObjects = currArea.GetObjects();
            foreach (GameObject obj in currentObjects)
            {
                DrawModel(obj.GetModelGroup(), obj.prepareForDrawing(), obj.GetInteractable());
                obj.ResetInteractable();
                if (GameConstants.boundingBoxRendering)
                    BoundingBoxRenderer.Render(obj.getBoundingBox(), m_graphics, m_updateInfo.viewMatrix, m_updateInfo.projectionMatrix, Color.White);
            }
            if (m_areas[currentArea].hasPrevious)
            {
                currentObjects = m_areas[currArea.previousID].GetObjects();
                foreach (GameObject obj in currentObjects)
                {
                    DrawModel(obj.GetModelGroup(), obj.prepareForDrawing(), obj.GetInteractable());
                    obj.ResetInteractable();
                }
            }
            if (m_areas[currentArea].hasNext)
            {
                currentObjects = m_areas[currArea.nextID].GetObjects();
                foreach (GameObject obj in currentObjects)
                {
                    DrawModel(obj.GetModelGroup(), obj.prepareForDrawing(), obj.GetInteractable());
                    obj.ResetInteractable();
                }
            }
            DrawSkybox();

            // draw the billboards with their specified effect
            string currID = currArea.id;
            string prevID = currArea.previousID;
            string nextID = currArea.nextID;
            foreach (GameObject obj in m_updateInfo.objectsWithBillboards)
            {
                string objAreaID = obj.getID().Split('.')[0];
                if (obj.isVisible && (objAreaID.Equals(currID) || objAreaID.Equals(prevID) || objAreaID.Equals(nextID)) )
                    DrawBillboard(obj.getBillboard(), true);
            }

            DrawBillboard(sun, false);
        }

        private void DrawBillboard(Billboard bb, bool fog)
        {
            // Save current states.

            RasterizerState prevRasterizerState = m_graphics.RasterizerState;
            if (prevRasterizerState == null)
                prevRasterizerState = RasterizerState.CullCounterClockwise;
            BlendState prevBlendState = m_graphics.BlendState;

            // First pass:
            // Render the non-transparent pixels of the billboards and store
            // their depths in the depth buffer.

            Effect billboardEffect = bb.getEffect();

            billboardEffect.Parameters["world"].SetValue(Matrix.Identity);
            billboardEffect.Parameters["view"].SetValue(m_updateInfo.viewMatrix);
            billboardEffect.Parameters["projection"].SetValue(m_updateInfo.projectionMatrix);
            billboardEffect.Parameters["billboardSize"].SetValue(bb.getSize());
            billboardEffect.Parameters["colorMap"].SetValue(bb.getTexture());
            billboardEffect.Parameters["alphaTestDirection"].SetValue(1.0f);
            billboardEffect.Parameters["withFog"].SetValue(fog);
            billboardEffect.Parameters["fogColor"].SetValue(GameConstants.backgroundColor.ToVector4());
            billboardEffect.Parameters["fogStart"].SetValue(30f);
            billboardEffect.Parameters["fogDensity"].SetValue(0.7f);
            if (!player.getIsThirdPersonCam())
            {
                billboardEffect.Parameters["fogMapMode"].SetValue(true);
                billboardEffect.Parameters["colorMap"].SetValue(bb.getTextureForMap());
            }
            else
                billboardEffect.Parameters["fogMapMode"].SetValue(false);

            m_graphics.BlendState = BlendState.Opaque;
            m_graphics.DepthStencilState = DepthStencilState.Default;
            m_graphics.RasterizerState = RasterizerState.CullNone;

            bb.Draw(m_graphics, billboardEffect);

            // Second pass:
            // Render the transparent pixels of the billboards.
            // Disable depth buffer writes to ensure that the depth values from
            // the first pass are used instead.

            billboardEffect.Parameters["alphaTestDirection"].SetValue(-1.0f);

            m_graphics.BlendState = BlendState.AlphaBlend;
            m_graphics.DepthStencilState = DepthStencilState.DepthRead;

            bb.Draw(m_graphics, billboardEffect);

            // Restore original states.

            m_graphics.BlendState = prevBlendState;
            m_graphics.RasterizerState = prevRasterizerState;
        }

        private void DrawModel(GameObject.ModelGroup modelGroup, Matrix world, bool interactable)
        {
            Model model = modelGroup.model;
            if (model == null) return;

            Dictionary<int, Texture2D> textures = modelGroup.textures;
            GameObject.Material material = modelGroup.material;

            AnimationPlayer animationPlayer = null;
            if (modelGroup is GameObject.ModelGroupAnimated)
                animationPlayer = ((GameObject.ModelGroupAnimated)modelGroup).animationPlayer;

            foreach (ModelMesh m in model.Meshes)
            {
                foreach (Effect e in m.Effects)
                {
                    if (animationPlayer != null)
                    {
                        switch (m_updateInfo.shadowQuality)
                        {
                            case 0: e.CurrentTechnique = e.Techniques["ShadedAndAnimated"]; break;
                            case 1: e.CurrentTechnique = e.Techniques["ShadedWithShadowsAndAnimated"]; break;
                            case 2: e.CurrentTechnique = e.Techniques["ShadedWithShadowsAndAnimated2x2PCF"]; break;
                        }
                        e.Parameters["Bones"].SetValue(animationPlayer.GetSkinTransforms());
                    }
                    else
                        switch (m_updateInfo.shadowQuality)
                        {
                            case 0: e.CurrentTechnique = e.Techniques["Shaded"]; break;
                            case 1: e.CurrentTechnique = e.Techniques["ShadedWithShadows"]; break;
                            case 2: e.CurrentTechnique = e.Techniques["ShadedWithShadows2x2PCF"]; break;
                        }
                    e.Parameters["lightViewProjection"].SetValue(m_shadowMap.LightViewProjectionMatrix);
                    e.Parameters["textureScaleBias"].SetValue(m_shadowMap.TextureScaleBiasMatrix);
                    e.Parameters["depthBias"].SetValue(m_shadowMap.DepthBias);
                    e.Parameters["shadowMap"].SetValue(m_shadowMap.ShadowMapTexture);

                    e.Parameters["world"].SetValue(world * m.ParentBone.Transform);
                    e.Parameters["interactable"].SetValue(interactable);

                    e.Parameters["cameraPos"].SetValue(player.getCameraPos());

                    e.Parameters["view"].SetValue(m_updateInfo.viewMatrix);
                    e.Parameters["projection"].SetValue(m_updateInfo.projectionMatrix);

                    e.Parameters["lightDir"].SetValue(m_globalLight.direction);
                    e.Parameters["lightColor"].SetValue(m_globalLight.color);
                    e.Parameters["materialAmbient"].SetValue(material.ambient);
                    e.Parameters["materialDiffuse"].SetValue(material.diffuse);
                    e.Parameters["materialSpecular"].SetValue(material.specular);
                    e.Parameters["shiny"].SetValue(material.shiny);
                    if (textures.ContainsKey(m.GetHashCode()) && textures[m.GetHashCode()] != null )
                        e.Parameters["colorMap"].SetValue(textures[m.GetHashCode()]);
                    else
                        e.Parameters["colorMap"].SetValue(textures[-1]);

                    e.Parameters["worldInverseTranspose"].SetValue(
                    Matrix.Transpose(Matrix.Invert(world * m.ParentBone.Transform)));

                    e.Parameters["texelSize"].SetValue(m_shadowMap.TexelSize);
                    e.Parameters["withFog"].SetValue(true);
                    e.Parameters["fogColor"].SetValue(GameConstants.backgroundColor.ToVector4());
                    e.Parameters["fogStart"].SetValue(30f);
                    e.Parameters["fogDensity"].SetValue(0.7f);
                    if (!player.getIsThirdPersonCam())
                        e.Parameters["fogMapMode"].SetValue(true);
                    else
                        e.Parameters["fogMapMode"].SetValue(false);
                }
                m.Draw();
            }
        }

        public void Draw2D()
        {
            int screenWidth = m_graphics.Viewport.Width;
            int screenHeight = m_graphics.Viewport.Height;

            m_spriteBatch.Begin();

            if(!player.getIsThirdPersonCam())
            {
                int offset = 5;
                int broadSide = 60;
                int narrowSide = 35;
                m_spriteBatch.Draw(arrowLeft, new Rectangle(offset, screenHeight / 2 - broadSide / 2, narrowSide, broadSide), Color.White);
                m_spriteBatch.Draw(arrowRight, new Rectangle(screenWidth - offset - narrowSide, screenHeight / 2 - broadSide / 2, narrowSide, broadSide), Color.White);
                m_spriteBatch.Draw(arrowUp, new Rectangle(screenWidth/ 2 - broadSide / 2, offset, broadSide, narrowSide), Color.White);
                m_spriteBatch.Draw(arrowDown, new Rectangle(screenWidth / 2 - broadSide / 2, screenHeight - narrowSide - offset, broadSide, narrowSide), Color.White);
            }
            else
            {
                m_spriteBatch.DrawString(screenFont, m_bonusTracker.chocoCount.ToString()
                   + "/" + m_bonusTracker.chocoTotal.ToString(), new Vector2(50f, 5f), Color.White);

                m_spriteBatch.Draw(chocoChip, new Rectangle(13, 5, 32, 40), Color.White);

                if (m_updateInfo.alwaysRun)
                {
                    int index = 0;
                    m_spriteBatch.Draw(distanceDisplay[index], new Rectangle(screenWidth/2-47, screenHeight-70,94, 50), Color.White);
                }

                m_spriteBatch.Draw(keys, new Rectangle(screenWidth - 187, screenHeight - 70, 177, 60), Color.White);
            }
            m_spriteBatch.End();

            //DrawShadowMap();

            // we need the following as spriteBatch.Begin() sets them to None and AlphaBlend
            // which breaks our model rendering
            m_graphics.DepthStencilState = DepthStencilState.Default;
            m_graphics.BlendState = BlendState.Opaque;
        }

        // this draws a 2D image of the shadow map in a corner of the screen - for debugging only
        private void DrawShadowMap()
        {
            Rectangle rect = new Rectangle();

            rect.X = 0;
            rect.Y = m_graphics.Viewport.Height - 128;
            rect.Width = 128;
            rect.Height = 128;

            m_spriteBatch.Begin();
            m_spriteBatch.Draw(m_shadowMap.ShadowMapTexture, rect, Color.White);
            m_spriteBatch.End();

            m_graphics.DepthStencilState = DepthStencilState.Default;
            m_graphics.BlendState = BlendState.Opaque;
        }

        /// <summary>
        /// http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series2/Skybox.php
        /// </summary>
        private void DrawSkybox()
        {
            Matrix[] skyboxTransforms = new Matrix[skyboxModel.Bones.Count];
            skyboxModel.CopyAbsoluteBoneTransformsTo(skyboxTransforms);
            int i = 0;
            foreach (ModelMesh mesh in skyboxModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix;
                    worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(player.getPosition());

                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(m_updateInfo.viewMatrix);
                    currentEffect.Parameters["xProjection"].SetValue(m_updateInfo.projectionMatrix);
                    currentEffect.Parameters["xTexture"].SetValue(skyboxTextures[i++]);
                }
                mesh.Draw();
            }
        }
    }
}
