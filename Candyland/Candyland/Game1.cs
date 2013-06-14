using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Xml;

namespace Candyland
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState oldState;
        
        // the scene manager, most stuff happens in there
        SceneManager m_sceneManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            m_sceneManager = new SceneManager( GraphicsDevice, this.graphics);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load all content required by the scene
            m_sceneManager.Load(this.Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (this.IsActive && gameTime.TotalGameTime.Milliseconds % 1 ==0 )
            {
                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                    || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    this.Exit();

                m_sceneManager.Update(gameTime);
            }

            KeyboardState newState = Keyboard.GetState();

            // Save, when F5 was pressed and now released
            if (oldState.IsKeyDown(Keys.F5) && newState.IsKeyUp(Keys.F5))
            {
                System.Diagnostics.Debug.WriteLine("Saving");
                Save();
            }

            // Load last savegame, when F6 was pressed and now released
            if (oldState.IsKeyDown(Keys.F6) && newState.IsKeyUp(Keys.F6))
            {
                Load();
            }

            // Update saved state.
            oldState = newState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            m_sceneManager.Draw(gameTime);
            m_sceneManager.Draw2D(spriteBatch);

            base.Draw(gameTime);
        }


        // Save Game
        protected void Save()
        {
            // Create the data to save.
            SaveGameData data = new SaveGameData();
            data.currentAreaID = m_sceneManager.getUpdateInfo().currentAreaID;
            data.currentLevelID = m_sceneManager.getUpdateInfo().currentLevelID;
            data.chocoChipState = m_sceneManager.getBonusTracker().chocoChipState;
            data.chocoCount = m_sceneManager.getBonusTracker().chocoCount;
            data.chocoTotal = m_sceneManager.getBonusTracker().chocoTotal;

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
        protected void Load()
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
                m_sceneManager.getUpdateInfo().currentAreaID = data.currentAreaID;
                m_sceneManager.getUpdateInfo().currentLevelID = data.currentLevelID;
                m_sceneManager.getUpdateInfo().reset = true; //everything should be reset, when game is loaded
                m_sceneManager.getBonusTracker().chocoChipState = data.chocoChipState;
                m_sceneManager.getBonusTracker().chocoCount = data.chocoCount;
                m_sceneManager.getBonusTracker().chocoTotal = data.chocoTotal;
            }
    }
}
