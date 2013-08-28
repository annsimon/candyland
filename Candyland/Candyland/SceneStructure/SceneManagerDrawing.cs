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

            DrawModel(player.GetModelGroup(), player.prepareForDrawing());
            if (GameConstants.boundingBoxRendering)
                BoundingBoxRenderer.Render(player.getBoundingBox(), m_graphics, m_updateInfo.viewMatrix, m_updateInfo.projectionMatrix, Color.White);
            DrawModel(player2.GetModelGroup(), player2.prepareForDrawing());
            if (GameConstants.boundingBoxRendering)
                BoundingBoxRenderer.Render(player2.getBoundingBox(), m_graphics, m_updateInfo.viewMatrix, m_updateInfo.projectionMatrix, Color.White);

            // draw the area the player currently is in and the two
            // adjacent ones
            string currentArea;

            if (m_updateInfo.candyselected)
                currentArea = m_updateInfo.currentguyLevelID.Split('.')[0];
            else
                currentArea = m_updateInfo.currenthelperLevelID.Split('.')[0];

            Area currArea = m_areas[currentArea];
            List<GameObject> currentObjects = currArea.GetObjects();
            foreach (GameObject obj in currentObjects)
            {
                DrawModel(obj.GetModelGroup(), obj.prepareForDrawing());
                if (GameConstants.boundingBoxRendering)
                    BoundingBoxRenderer.Render(obj.getBoundingBox(), m_graphics, m_updateInfo.viewMatrix, m_updateInfo.projectionMatrix, Color.White);
            }
            if (m_areas[currentArea].hasPrevious)
            {
                currentObjects = m_areas[currArea.previousID].GetObjects();
                foreach (GameObject obj in currentObjects)
                    DrawModel(obj.GetModelGroup(), obj.prepareForDrawing());
            }
            if (m_areas[currentArea].hasNext)
            {
                currentObjects = m_areas[currArea.nextID].GetObjects();
                foreach (GameObject obj in currentObjects)
                    DrawModel(obj.GetModelGroup(), obj.prepareForDrawing());
            }
            DrawSkybox();
        }

        private void DrawModel(GameObject.ModelGroup modelGroup, Matrix world)
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
                        e.CurrentTechnique = e.Techniques["ShadedWithShadowsAndAnimated"];
                        e.Parameters["Bones"].SetValue(animationPlayer.GetSkinTransforms());
                    }
                    else
                        e.CurrentTechnique = e.Techniques["ShadedWithShadows"];
                    e.Parameters["lightViewProjection"].SetValue(m_shadowMap.LightViewProjectionMatrix);
                    e.Parameters["textureScaleBias"].SetValue(m_shadowMap.TextureScaleBiasMatrix);
                    e.Parameters["depthBias"].SetValue(m_shadowMap.DepthBias);
                    e.Parameters["shadowMap"].SetValue(m_shadowMap.ShadowMapTexture);

                    e.Parameters["world"].SetValue(world * m.ParentBone.Transform);

                    if (m_updateInfo.candyselected)
                        e.Parameters["cameraPos"].SetValue(player.getCameraPos());
                    else
                        e.Parameters["cameraPos"].SetValue(player2.getCameraPos());
                    e.Parameters["view"].SetValue(m_updateInfo.viewMatrix);
                    e.Parameters["projection"].SetValue(m_updateInfo.projectionMatrix);

                    e.Parameters["lightDir"].SetValue(m_globalLight.direction);
                    e.Parameters["lightColor"].SetValue(m_globalLight.color);
                    e.Parameters["materialAmbient"].SetValue(material.ambient);
                    e.Parameters["materialDiffuse"].SetValue(material.diffuse);
                    e.Parameters["materialSpecular"].SetValue(material.specular);
                    e.Parameters["shiny"].SetValue(material.shiny);
                    if (textures.ContainsKey(m.GetHashCode()))
                        e.Parameters["colorMap"].SetValue(textures[-1]);
                    else
                        e.Parameters["colorMap"].SetValue(textures[-1]);

                    e.Parameters["worldInverseTranspose"].SetValue(
                    Matrix.Transpose(Matrix.Invert(world * m.ParentBone.Transform)));

                    e.Parameters["texelSize"].SetValue(m_shadowMap.TexelSize);
                    e.Parameters["withFog"].SetValue(true);
                    e.Parameters["fogColor"].SetValue(GameConstants.backgroundColor.ToVector4());
                    e.Parameters["fogStart"].SetValue(30f);
                    e.Parameters["fogDensity"].SetValue(0.7f);
                    if (!player.getIsThirdPersonCam() || !player2.getIsThirdPersonCam())
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

            m_spriteBatch.DrawString(screenFont, m_bonusTracker.chocoCount.ToString()
               + "/" + m_bonusTracker.chocoTotal.ToString(), new Vector2(50f, 5f), Color.White);

            m_spriteBatch.Draw(chocoChip, new Rectangle(13, 5, 32, 40), Color.White);      

            if (m_updateInfo.helperavailable)
                m_spriteBatch.Draw(keysFull, new Rectangle(screenWidth - 252, screenHeight - 70, 242, 60), Color.White);
            else
                m_spriteBatch.Draw(keys, new Rectangle(screenWidth - 187, screenHeight - 70, 177, 60), Color.White);

            m_spriteBatch.End();

            //DrawShadowMap();

            // we need the following as spriteBatch.Begin() sets them to None and AlphaBlend
            // which breaks our model rendering
            m_graphics.DepthStencilState = DepthStencilState.Default;
            m_graphics.BlendState = BlendState.Opaque;
        }

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
                    Matrix worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(player.getPosition());
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
