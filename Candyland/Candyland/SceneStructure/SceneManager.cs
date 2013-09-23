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
        Song song1;
        Song song2;

        Boolean boss = false;

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

        float distanceToBoss = 0;

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

        public void Load(ContentManager manager, AssetManager assets)
        {
            foreach (var area in m_areas)
                area.Value.Load(manager, assets);

            player.load(manager, assets);
            player2.load(manager, assets);

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
            song2 = assets.song2;

            MediaPlayer.Play(song1);
            MediaPlayer.Volume = (float)m_updateInfo.musicVolume / 10;
            MediaPlayer.IsRepeating = true;
            boss = false;


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
                data.helperIsAvailable = m_updateInfo.helperavailable;
                data.selectedPlayer = m_updateInfo.candyselected;
                data.guycurrentLevelID = m_updateInfo.currentguyLevelID;
                data.helpercurrentLevelID = m_updateInfo.currenthelperLevelID; 
            // Teleports
                data.activatedTeleports = m_updateInfo.activeTeleports;
            // Bonus
                data.chocoChipState = m_bonusTracker.chocoChipState;
                data.chocoCount = m_bonusTracker.chocoCount;
                data.chocoSpend = m_bonusTracker.chocoChipsSpent;
                data.soldItems = m_bonusTracker.soldItems;
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
                    m_updateInfo.helperavailable = data.helperIsAvailable;
                    m_updateInfo.candyselected = data.selectedPlayer;
                    m_updateInfo.currentguyAreaID = data.guycurrentLevelID.Split('.')[0];
                    m_updateInfo.currentguyLevelID = data.guycurrentLevelID;
                    m_updateInfo.currenthelperAreaID = data.helpercurrentLevelID.Split('.')[0];
                    m_updateInfo.currenthelperLevelID = data.helpercurrentLevelID;
                    m_updateInfo.reset = true; //everything should be reset, when game is loaded
                // Teleport points
                    m_updateInfo.activeTeleports = data.activatedTeleports;
                // Bonus stuff
                    m_bonusTracker.chocoChipState = data.chocoChipState;
                    m_bonusTracker.chocoCount = data.chocoCount;
                    m_bonusTracker.chocoChipsSpent = data.chocoSpend;
                    m_bonusTracker.soldItems = data.soldItems;
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

        /// <summary>
        /// NOT USED
        /// When a new game get's started, while another game is still running, content doesn't need to be reloaded
        /// simply set the necessary data back to their starting values
        /// </summary>
        public bool ResetDataForNewGame()
        {
            // Set everything back to starting values
                m_updateInfo.candyselected = true;
                m_updateInfo.helperavailable = false;
                m_updateInfo.activateHelperNow = false;
                m_updateInfo.loseHelperNow = false;
                m_updateInfo.currentguyAreaID = GameConstants.startAreaID;
                m_updateInfo.currentguyLevelID = GameConstants.startLevelID;
                m_updateInfo.currenthelperAreaID = GameConstants.startAreaID;
                m_updateInfo.currenthelperLevelID = GameConstants.startLevelID;
                m_updateInfo.reset = true; //everything should be reset for a new game
                // set all chocoChips to not collected
                List<string> chocoIDs = new List<string>(m_bonusTracker.chocoChipState.Keys);
                foreach (string id in chocoIDs)
                {
                    m_bonusTracker.chocoChipState[id] = false;
                }
                m_bonusTracker.chocoCount = 0;
                m_bonusTracker.chocoChipsSpent = 0;
                m_bonusTracker.soldItems = new List<string>(30);
                // set all actions to not yet started (false)
                List<string> actionIDs = new List<string>(m_actionTracker.actionState.Keys);
                foreach (string id in actionIDs)
                {
                    m_actionTracker.actionState[id] = false;
                }
                // set all chocoChips in objectWithBillboards list to visible
                foreach (GameObject obj in m_updateInfo.objectsWithBillboards)
                {
                    if (obj.GetType() == typeof(ChocoChip))
                        obj.isVisible = true;
                }
                // no teleports available at game start
                m_updateInfo.activeTeleports = new List<string>(10);

            // Set the world back to start
            foreach (var area in m_areas)
                area.Value.Load();

            return true;
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
                if (!(m_updateInfo.playerWon && (obj is ActionActor && obj.getID().Contains("boss"))))
                    m_shadowMap.Draw(obj.GetModelGroup().model, obj.prepareForDrawing(), null);
            if (m_areas[currentArea].hasPrevious)
            {
                currentObjects = m_areas[currArea.previousID].GetObjects();
                foreach (GameObject obj in currentObjects)
                    if(!(m_updateInfo.playerWon && (obj is ActionActor && obj.getID().Contains("boss"))))
                        m_shadowMap.Draw(obj.GetModelGroup().model, obj.prepareForDrawing(), null);
            }
            if (m_areas[currentArea].hasNext)
            {
                currentObjects = m_areas[currArea.nextID].GetObjects();
                foreach (GameObject obj in currentObjects)
                    if (!(m_updateInfo.playerWon && (obj is ActionActor && obj.getID().Contains("boss"))))
                        m_shadowMap.Draw(obj.GetModelGroup().model, obj.prepareForDrawing(), null);
            }

            m_shadowMap.End();
            m_graphics.BlendState = oldBS;
        }

    }
}
