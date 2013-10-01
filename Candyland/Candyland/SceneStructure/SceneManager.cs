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
using System;

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
        Song song1;

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
        Texture2D[] distanceDisplay;
        Texture2D[] skyboxTextures;
        Model skyboxModel;

        // Map arrows
        Texture2D arrowLeft;
        Texture2D arrowRight;
        Texture2D arrowUp;
        Texture2D arrowDown;

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

            player = new CandyGuy(new Vector3(0, 0.4f, 0), new Vector3(0, 0, 1), m_graphics.Viewport.AspectRatio, m_updateInfo, m_bonusTracker);

            sun = new Sun(m_graphics);
            
            //TEST!!!
            m_updateInfo.currentguyLevelID = (GameConstants.startLevelID);           
            
            Vector3 playerStartPos = m_areas[m_updateInfo.currentguyLevelID.Split('.')[0]].GetPlayerStartingPosition(player);
            playerStartPos.Y += 0.6f;
            player.setPosition(playerStartPos);

            // set up shadow map for drop shadows
            m_shadowMap = new ShadowMap(m_graphics, screenManager.Content);
            m_shadowMap.DepthBias = GameConstants.depthBias;

            // set up scene light
            m_globalLight.direction = new Vector3(0.5f, -0.5f, -0.5f);
            m_globalLight.direction.Normalize();
            m_globalLight.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            m_globalLight.rotation = Vector2.Zero;
        }

        public void Load(ContentManager manager, AssetManager assets)
        {
            foreach (var area in m_areas)
                area.Value.Load(manager, assets);

            player.load(manager, assets);

            sun.Load(manager, assets);

            screenFont = assets.mainText;
            keys = assets.hudIcons;
            chocoChip = assets.chocoChip;
            keysFull = assets.hudIconsWithCharChange;
            skyboxModel = LoadSkybox(manager, assets, out skyboxTextures);

            arrowLeft = assets.mapArrowLeft;
            arrowRight = assets.mapArrowRight;
            arrowUp = assets.mapArrowUp;
            arrowDown = assets.mapArrowDown;

            song1 = assets.song1;  // background music from http://longzijun.wordpress.com/2012/12/26/upbeat-background-music-free-instrumentals/

            MediaPlayer.Play(song1);
            MediaPlayer.Volume = (float)m_updateInfo.musicVolume / 10;
            MediaPlayer.IsRepeating = true;


            distanceDisplay = new Texture2D[4];
            distanceDisplay[0] = assets.distanceDisplay0;
            distanceDisplay[1] = assets.distanceDisplay1;
            distanceDisplay[2] = assets.distanceDisplay2;
            distanceDisplay[3] = assets.distanceDisplay3;
        }

        /// <summary>
        /// http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series2/Skybox.php
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="assetName"></param>
        /// <param name="textures"></param>
        /// <returns></returns>
        private Model LoadSkybox(ContentManager manager, AssetManager assets, out Texture2D[] textures)
        {
            Model newModel = assets.skybox;
            textures = assets.skyboxTextures;

            Effect skyboxEffect = assets.skyboxEffect;
            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = skyboxEffect.Clone();

            return newModel;
        }

        // Save Game
        public void SaveGame()
        {
            // Create the data to save.
            SaveGameData data = new SaveGameData();
            // Player and level
                data.guycurrentLevelID = m_updateInfo.currentguyLevelID;
            // Bonus
                data.chocoChipState = m_bonusTracker.chocoChipState;
                data.chocoCount = m_bonusTracker.chocoCount;
            // Actions
                data.actionState = m_actionTracker.actionState;
                data.actionActorVisibility = m_actionTracker.actionActorVisibility;

            string filename = "savegame.sav";

            // Convert the object to XML data
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                IntermediateSerializer.Serialize(writer, data, null);
            }
        }


        /// <summary>
        /// Load savedGame
        /// </summary>
        /// <returns>true, if savegame is available</returns>
        public bool LoadSavegame()
        {
            string filename = "savegame.sav";

            XmlReaderSettings settings = new XmlReaderSettings();
            SaveGameData data;
            XmlReader reader;

            try
            {
                reader = XmlReader.Create(filename, settings);
                using (reader)
                {
                    data = IntermediateSerializer.
                        Deserialize<SaveGameData>
                        (reader, null);
                }

                // Use saved data to put Game into the last saved state

                // Player and level
                    m_updateInfo.currentguyAreaID = data.guycurrentLevelID.Split('.')[0];
                    m_updateInfo.currentguyLevelID = data.guycurrentLevelID;
                    m_updateInfo.reset = true; //everything should be reset, when game is loaded
                // Bonus stuff
                    m_bonusTracker.chocoChipState = data.chocoChipState;
                    m_bonusTracker.chocoCount = data.chocoCount;
                    // set all collected chocoChips in objectWithBillboards list to invisible
                    bool isCollected = false;
                    foreach (GameObject obj in m_updateInfo.objectsWithBillboards)
                    {
                        data.chocoChipState.TryGetValue(obj.getID(), out isCollected);
                        if (isCollected)
                            obj.isVisible = false;
                    }
                // Actions
                    m_actionTracker.actionState = data.actionState;
                    m_actionTracker.actionActorVisibility = data.actionActorVisibility;

                // Update the world to savegame status
                foreach (var area in m_areas)
                    area.Value.Load();
                return true;
            }
            catch
            {
                return false;
            }
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

            currentArea = m_updateInfo.currentguyLevelID.Split('.')[0];

            Area currArea = m_areas[currentArea];
            List<GameObject> currentObjects = currArea.GetObjects();

            m_shadowMap.Begin(m_graphics);

            m_shadowMap.Draw(player.GetModelGroup().model, player.prepareForDrawing(), null);// ((GameObject.ModelGroupAnimated)player.GetModelGroup()).animationPlayer.GetBoneTransforms());

// ANNE fragen

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


        public void wndProc(ref System.Windows.Forms.Message mes)
        {
            if (m_inputManager != null)
                m_inputManager.wndProc(ref mes);
        }
    }
}
