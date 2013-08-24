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
    /// <summary>
    /// This class takes care of the game, this includes visual aspects (Area) and
    /// the game's logic (Communications).
    /// </summary>
    public class SceneManager
    {
        // struct for the light source we use
        private struct DirectionalLight
        {
            public Vector2 rotation;
            public Vector3 direction;
            public Vector4 color;
        }

        DirectionalLight m_globalLight;
        ShadowMap m_shadowMap;

        // areas is a dictionary (works like a map) that saves the area
        // every area can be accessed by its id
        Dictionary<string, Area> m_areas;

        // this object is used to keep track of 
        // the ChocoChip collection and of which extras the player activated
        BonusTracker m_bonusTracker;
        public BonusTracker getBonusTracker() { return m_bonusTracker; }

        // this object is used to keep track of one-time actions
        ActionTracker m_actionTracker;

        // the update info, this object is used for communication
        UpdateInfo m_updateInfo;
        public UpdateInfo getUpdateInfo() { return m_updateInfo; }

        // the player
        CandyGuy player;
        CandyHelper player2;
        /*************************************************************/
        // graphics device needed for drawing the bounding boxes
        GraphicsDevice m_graphics;
        SpriteBatch m_spriteBatch;
        /*************************************************************/

        // font used for writing tests to screen
        SpriteFont screenFont;

        Texture2D chocoChip;
        Texture2D keys;

        InputManager m_inputManager;

        public SceneManager(ScreenManager screenManager)
        {
            m_bonusTracker = new BonusTracker(); // load this one from xml as serialized object?

            m_actionTracker = new ActionTracker();

            m_updateInfo = new UpdateInfo(screenManager.GraphicsDevice, screenManager);


            m_inputManager = new InputManager(screenManager.GraphicsDevice, GameConstants.inputManagerMode, m_updateInfo);
            /****************************************************************/
            m_graphics = screenManager.GraphicsDevice;
            m_spriteBatch = screenManager.SpriteBatch;
            /****************************************************************/
                        
            m_areas = AreaParser.ParseAreas(m_updateInfo, m_bonusTracker, m_actionTracker);

            player2 = new CandyHelper(new Vector3(0, 0.4f, 0.2f), Vector3.Up, m_graphics.Viewport.AspectRatio, m_updateInfo, m_bonusTracker);
            player = new CandyGuy(new Vector3(0, 0.4f, 0), new Vector3(0, 0, 1), m_graphics.Viewport.AspectRatio, m_updateInfo, m_bonusTracker, player2);

            
            //TEST!!!
            player.setCurrentLevelId(GameConstants.startLevelID);
            player2.setCurrentLevelId(GameConstants.startLevelID);
            
            
            Vector3 playerStartPos = m_areas[m_updateInfo.currentAreaID].GetPlayerStartingPosition(player);
            playerStartPos.Y += 0.6f;
            player.setPosition(playerStartPos);
            Vector3 player2StartPos = m_areas[m_updateInfo.currentAreaID].GetCompanionStartingPosition(player2);
            player2StartPos.Y += 0.6f;
            player2.setPosition(player2StartPos);

            // set up shadow map for drop shadows
            m_shadowMap = new ShadowMap(m_graphics, screenManager.Content);
            m_shadowMap.DepthBias = 0.00249f;

            // set up scene light
            m_globalLight.direction = new Vector3(0.0f, -0.5f, -0.5f);
            m_globalLight.direction.Normalize();
            m_globalLight.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            m_globalLight.rotation = Vector2.Zero;
        }

        public void Load(ContentManager manager)
        {
            foreach (var area in m_areas)
                area.Value.Load(manager);

            player.load(manager);
            player2.load(manager);

            screenFont = manager.Load<SpriteFont>("Fonts/MainText");
            keys = manager.Load<Texture2D>("Images/HUD/HudFull");
            chocoChip = manager.Load<Texture2D>("Images/HUD/Choco");
        }

        public void Update(GameTime gameTime)
        {
            /*
            System.Console.Out.WriteLine("currLevel = " + m_updateInfo.currentLevelID);
            if( m_updateInfo.playerIsOnLevelExit)
                System.Console.Out.WriteLine("nextLevel = " + m_updateInfo.levelAfterExitID);
            */

            // Update gameTime in UpdateInfo
            m_updateInfo.gameTime = gameTime;

            if (m_updateInfo.reset)
            {

                
                // reset player to start position of current level
                if (m_updateInfo.candyselected || player.getCurrentLevelId() == player2.getCurrentLevelId())
                {
                    player.Reset();
                    Vector3 resetPos = m_areas[player.getCurrentLevelId().Split('.')[0]].GetPlayerStartingPosition(player);
                    resetPos.Y += 0.6f;
                    player.setPosition(resetPos);
                }

                if (!m_updateInfo.candyselected || player.getCurrentLevelId() == player2.getCurrentLevelId())
                {
                    player2.Reset();
                    Vector3 resetPos2 = m_areas[player2.getCurrentLevelId().Split('.')[0]].GetCompanionStartingPosition(player2);
                    resetPos2.Y += 0.6f;
                    player2.setPosition(resetPos2);
                }


                // reset world *!*| MAYBE NOT NEEDED |*!*
               /* foreach (var area in m_areas)
                    area.Value.Reset();*/
                if (m_updateInfo.candyselected)
                    m_areas[player.getCurrentLevelId().Split('.')[0]].Reset();

                if (!m_updateInfo.candyselected)
                    m_areas[player2.getCurrentLevelId().Split('.')[0]].Reset();

                m_updateInfo.reset = false;
            }

            m_inputManager.update(player,player2);
            player.update();
            player2.update();

            player.startIntersection();
            player2.startIntersection();

            // check for Collision between the Player and all Game Objects in the current Level
            m_areas[player.getCurrentLevelId().Split('.')[0]].Collide(player);
            if (m_updateInfo.playerIsOnAreaExit && player.getNextLevelId() != null)
                m_areas[player.getNextLevelId().Split('.')[0]].Collide(player);
            // check for Collision between the Player2 and all Game Objects in the current Level
            m_areas[player2.getCurrentLevelId().Split('.')[0]].Collide(player2);
            if (m_updateInfo.playerIsOnAreaExit && player2.getNextLevelId() != null)
                m_areas[player2.getNextLevelId().Split('.')[0]].Collide(player2);

            // update the area the player currently is in
            // and the next area if the player is about to leave the current area
            m_areas[player.getCurrentLevelId().Split('.')[0]].Update(gameTime, player,player2);
            if (m_updateInfo.playerIsOnAreaExit && player.getNextLevelId() != null)
                m_areas[player.getNextLevelId().Split('.')[0]].Update(gameTime, player,player2);

            if(player.getCurrentLevelId() != player2.getCurrentLevelId())
            m_areas[player2.getCurrentLevelId().Split('.')[0]].Update(gameTime,player, player2);
            if (m_updateInfo.playerIsOnAreaExit && player2.getNextLevelId() != null)
                m_areas[player2.getNextLevelId().Split('.')[0]].Update(gameTime,player, player2);
           
            player.endIntersection();
            player2.endIntersection();

            m_areas[m_updateInfo.currentAreaID].endIntersection();



            KeyboardState keystate = Keyboard.GetState();
            if (keystate.IsKeyDown(Keys.D1))
                m_shadowMap.DepthBias += 0.0001f;

            if (keystate.IsKeyDown(Keys.D2))
                m_shadowMap.DepthBias -= 0.0001f;

            UpdateShadowMap();
        }

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
            string currentArea = m_updateInfo.currentAreaID;
            Area currArea = m_areas[currentArea];
            List<GameObject> currentObjects = currArea.GetObjects();
            foreach (GameObject obj in currentObjects)
            {
                DrawModel(obj.GetModelGroup(), obj.prepareForDrawing());
                if(GameConstants.boundingBoxRendering)
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
                        e.CurrentTechnique = e.Techniques["ShadedAndAnimated"];
                        e.Parameters["Bones"].SetValue(animationPlayer.GetSkinTransforms());
                    }
                    else
                        e.CurrentTechnique = e.Techniques["Shaded"];
                    e.Parameters["lightViewProjection"].SetValue(m_shadowMap.LightViewProjectionMatrix);
                    e.Parameters["textureScaleBias"].SetValue(m_shadowMap.TextureScaleBiasMatrix);
                    e.Parameters["depthBias"].SetValue(m_shadowMap.DepthBias);
                    e.Parameters["shadowMap"].SetValue(m_shadowMap.ShadowMapTexture);

                    e.Parameters["world"].SetValue(world * m.ParentBone.Transform);

                    e.Parameters["view"].SetValue(m_updateInfo.viewMatrix);
                    e.Parameters["projection"].SetValue(m_updateInfo.projectionMatrix);

                    e.Parameters["lightDir"].SetValue(m_globalLight.direction);
                    e.Parameters["lightColor"].SetValue(m_globalLight.color);
                    e.Parameters["materialAmbient"].SetValue(material.ambient);
                    e.Parameters["materialDiffuse"].SetValue(material.diffuse);
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

            m_spriteBatch.Draw(chocoChip, new Rectangle(5, 5, 40, 40), Color.White);

            m_spriteBatch.Draw(keys, new Rectangle(screenWidth - 186, screenHeight-70, 176, 60), Color.White);

            m_spriteBatch.End();

            //DrawShadowMap();

            // we need the following as spriteBatch.Begin() sets them to None and AlphaBlend
            // which breaks our model rendering
            m_graphics.DepthStencilState = DepthStencilState.Default;
            m_graphics.BlendState = BlendState.Opaque;
        }


        // Save Game
        public void Save()
        {
            // Create the data to save.
            SaveGameData data = new SaveGameData();
            data.currentAreaID = m_updateInfo.currentAreaID;
            data.currentLevelID = m_updateInfo.currentLevelID;
            data.chocoChipState = m_bonusTracker.chocoChipState;
            data.chocoCount = m_bonusTracker.chocoCount;
            data.chocoTotal = m_bonusTracker.chocoTotal;

            string filename = "savegame.sav";

            // Convert the object to XML data
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                IntermediateSerializer.Serialize(writer, data, null);
            }
        }

        // Load savedGame
        public void Load()
        {
            string filename = "savegame.sav";

            XmlReaderSettings settings = new XmlReaderSettings();
            SaveGameData data;
            XmlReader reader;
            //TODO Needs to be changed to testing if the savefile exists
            if (true)
                reader = XmlReader.Create(filename, settings);
            using (reader)
            {
                data = IntermediateSerializer.
                    Deserialize<SaveGameData>
                    (reader, null);
            }

            // Use saved data to put Game into the last saved state
            m_updateInfo.currentAreaID = data.currentAreaID;
            m_updateInfo.currentLevelID = data.currentLevelID;
            m_updateInfo.reset = true; //everything should be reset, when game is loaded
            m_bonusTracker.chocoChipState = data.chocoChipState;
            m_bonusTracker.chocoCount = data.chocoCount;
            m_bonusTracker.chocoTotal = data.chocoTotal;

            // Update ChocoChips to savegame
            foreach (var area in m_areas)
                area.Value.Load();
        }

        #region ShadowMap

        private void CreateShadowMap()
        {
            string currentArea = m_updateInfo.currentAreaID;
            Area currArea = m_areas[currentArea];
            List<GameObject> currentObjects = currArea.GetObjects();

            m_shadowMap.Begin(m_graphics);

            m_shadowMap.Draw(player.GetModelGroup().model, player.prepareForDrawing());
            m_shadowMap.Draw(player2.GetModelGroup().model, player2.prepareForDrawing());

            foreach (GameObject obj in currentObjects)
                m_shadowMap.Draw(obj.GetModelGroup().model, obj.prepareForDrawing());
            if (m_areas[currentArea].hasPrevious)
            {
                currentObjects = m_areas[currArea.previousID].GetObjects();
                foreach (GameObject obj in currentObjects)
                    m_shadowMap.Draw(obj.GetModelGroup().model, obj.prepareForDrawing());
            }
            if (m_areas[currentArea].hasNext)
            {
                currentObjects = m_areas[currArea.nextID].GetObjects();
                foreach (GameObject obj in currentObjects)
                    m_shadowMap.Draw(obj.GetModelGroup().model, obj.prepareForDrawing());
            }

            m_shadowMap.End();
        }

        private void UpdateShadowMap()
        {
            m_shadowMap.Update(m_globalLight.direction, 
                m_updateInfo.viewMatrix*m_updateInfo.projectionMatrix);
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

        #endregion

    }
}
