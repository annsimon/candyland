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

namespace Candyland
{
    /// <summary>
    /// This class takes care of the game, this includes visual aspects (Area) and
    /// the game's logic (Communications).
    /// </summary>
    public partial class SceneManager
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

        Sun sun;
        /*************************************************************/
        // graphics device needed for drawing the bounding boxes
        GraphicsDevice m_graphics;
        SpriteBatch m_spriteBatch;
        /*************************************************************/

        // font used for writing tests to screen
        SpriteFont screenFont;

        Texture2D chocoChip;
        Texture2D keys;
        Texture2D keysFull;
        Texture2D[] skyboxTextures;
        Model skyboxModel;

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

            sun = new Sun(m_graphics);
            
            //TEST!!!
            m_updateInfo.currentguyLevelID = (GameConstants.startLevelID);
            m_updateInfo.currenthelperLevelID = (GameConstants.startLevelID);
            
            
            Vector3 playerStartPos = m_areas[m_updateInfo.currentguyLevelID.Split('.')[0]].GetPlayerStartingPosition(player);
            playerStartPos.Y += 0.6f;
            player.setPosition(playerStartPos);
            Vector3 player2StartPos = m_areas[m_updateInfo.currenthelperLevelID.Split('.')[0]].GetCompanionStartingPosition(player2);
            player2StartPos.Y += 0.6f;
            player2.setPosition(player2StartPos);

            // set up shadow map for drop shadows
            m_shadowMap = new ShadowMap(m_graphics, screenManager.Content);
            m_shadowMap.DepthBias = GameConstants.depthBias;

            // set up scene light
            m_globalLight.direction = new Vector3(0.5f, -0.5f, -0.5f);
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

            sun.Load(manager);

            screenFont = manager.Load<SpriteFont>("Fonts/MainText");
            keys = manager.Load<Texture2D>("Images/HUD/HudFull");
            chocoChip = manager.Load<Texture2D>("Images/HUD/Choco");
            keysFull = manager.Load<Texture2D>("Images/HUD/HudFullWithChange");
            skyboxModel = LoadSkybox(manager, "Skybox/skybox2", out skyboxTextures);
        }

        /// <summary>
        /// http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series2/Skybox.php
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="assetName"></param>
        /// <param name="textures"></param>
        /// <returns></returns>
        private Model LoadSkybox(ContentManager manager, string assetName, out Texture2D[] textures)
        {

            Model newModel = manager.Load<Model>(assetName);
            textures = new Texture2D[newModel.Meshes.Count];
            int i = 0;
            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (BasicEffect currentEffect in mesh.Effects)
                    textures[i++] = currentEffect.Texture;

            Effect skyboxEffect = manager.Load<Effect>("Skybox/effects");
            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = skyboxEffect.Clone();

            return newModel;
        }

        // Save Game
        public void Save()
        {
            // Create the data to save.
            SaveGameData data = new SaveGameData();
            data.guycurrentAreaID = m_updateInfo.currentguyLevelID.Split('.')[0];
            data.guycurrentLevelID = m_updateInfo.currentguyLevelID;
            data.helpercurrentAreaID = m_updateInfo.currenthelperLevelID.Split('.')[0];
            data.helpercurrentLevelID = m_updateInfo.currenthelperLevelID; 
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
            m_updateInfo.currentguyLevelID = data.guycurrentLevelID;
            m_updateInfo.currenthelperLevelID = (data.helpercurrentLevelID);
            m_updateInfo.reset = true; //everything should be reset, when game is loaded
            m_bonusTracker.chocoChipState = data.chocoChipState;
            m_bonusTracker.chocoCount = data.chocoCount;
            m_bonusTracker.chocoTotal = data.chocoTotal;

            // Update ChocoChips to savegame
            foreach (var area in m_areas)
                area.Value.Load();
        }

        private void CreateShadowMap()
        {
            // save old state
            BlendState oldBS = m_graphics.BlendState;
            BlendState newBS = new BlendState();
            newBS.ColorWriteChannels = ColorWriteChannels.All;
            newBS.ColorWriteChannels1 = ColorWriteChannels.All;
            newBS.ColorWriteChannels2 = ColorWriteChannels.All;
            newBS.ColorWriteChannels3 = ColorWriteChannels.All;
            newBS.AlphaDestinationBlend = Blend.Zero;
            newBS.AlphaSourceBlend = Blend.One;
            newBS.ColorDestinationBlend = Blend.Zero;
            newBS.ColorSourceBlend = Blend.One;
            m_graphics.BlendState = newBS;

            string currentArea;

            if(m_updateInfo.candyselected)
                currentArea = m_updateInfo.currentguyLevelID.Split('.')[0];
            else
                currentArea = m_updateInfo.currenthelperLevelID.Split('.')[0];

            Area currArea = m_areas[currentArea];
            List<GameObject> currentObjects = currArea.GetObjects();

            m_shadowMap.Begin(m_graphics);

            m_shadowMap.Draw(player.GetModelGroup().model, player.prepareForDrawing(), null);// ((GameObject.ModelGroupAnimated)player.GetModelGroup()).animationPlayer.GetBoneTransforms());
            m_shadowMap.Draw(player2.GetModelGroup().model, player2.prepareForDrawing(), null);

            foreach (GameObject obj in currentObjects)
                m_shadowMap.Draw(obj.GetModelGroup().model, obj.prepareForDrawing(), null);
            if (m_areas[currentArea].hasPrevious)
            {
                currentObjects = m_areas[currArea.previousID].GetObjects();
                foreach (GameObject obj in currentObjects)
                    m_shadowMap.Draw(obj.GetModelGroup().model, obj.prepareForDrawing(), null);
            }
            if (m_areas[currentArea].hasNext)
            {
                currentObjects = m_areas[currArea.nextID].GetObjects();
                foreach (GameObject obj in currentObjects)
                    m_shadowMap.Draw(obj.GetModelGroup().model, obj.prepareForDrawing(), null);
            }

            m_shadowMap.End();
            m_graphics.BlendState = oldBS;
        }

    }
}
