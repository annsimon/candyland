using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System;

namespace Candyland
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        AssetManager assetManager;

        KeyboardState oldState;
        KeyboardState newState;

        IntPtr drawsurface;
        System.Windows.Forms.Form parentForm;
        System.Windows.Forms.PictureBox pictureBox;
        System.Windows.Forms.Control gameForm;


        // Class to hold all data for game settings
        SaveSettingsData settingsData;
        public bool mute;


        public Game1(IntPtr _drawSurface, System.Windows.Forms.Form _parentForm,
            System.Windows.Forms.PictureBox _surfacePictureBox)
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            settingsData = new SaveSettingsData();
            LoadSettings();
            graphics.IsFullScreen = settingsData.isFullscreen;
            if (graphics.IsFullScreen)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                graphics.PreferredBackBufferWidth = 800;
                graphics.PreferredBackBufferHeight = 480;
            }
            // Create the screen manager component.
            assetManager = new AssetManager();
            screenManager = new ScreenManager(this, graphics.IsFullScreen, 800, 480, assetManager, settingsData);
            Components.Add(screenManager);


            //Für WindowsForms
            this.drawsurface = _drawSurface;
            this.parentForm = _parentForm;
            this.pictureBox = _surfacePictureBox;
            Mouse.WindowHandle = drawsurface;
            this.gameForm = System.Windows.Forms.Control.FromHandle(this.Window.Handle);

            gameForm.VisibleChanged += new EventHandler(gameForm_VisibleChanged);
            pictureBox.SizeChanged += new EventHandler(pictureBox_SizeChanged);
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings); 
            // Content.RootDirectory = "CandylandContent"; 
        }

        private void gameForm_VisibleChanged(object sender, EventArgs e) {
            if (gameForm.Visible == true) gameForm.Visible = false;
        }

        private void pictureBox_SizeChanged(object sender, EventArgs e) {
            if (parentForm.WindowState != System.Windows.Forms.FormWindowState.Minimized) {
                graphics.PreferredBackBufferWidth = pictureBox.Width;
                graphics.PreferredBackBufferHeight = pictureBox.Height;
                graphics.ApplyChanges();
            }
        }

        private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = this.drawsurface;

        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = false;

            mute = false;
            // Set start screen
            screenManager.AddScreen(new TitleScreen());


            base.Initialize();
        }

        private void LoadSettings()
        {
            string filename = "settings.sav";

            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader reader;
            
            try
            {
                reader = XmlReader.Create(filename, settings);
                using (reader)
                {
                    settingsData = IntermediateSerializer.
                        Deserialize<SaveSettingsData>
                        (reader, null);
                }                
            }
            catch
            {
                // if no user saved settings, set to default
                settingsData.isFullscreen = false;
                settingsData.musicVolume = 5;
                settingsData.shadowQuality = 2;
                settingsData.showTutorial = true;
                settingsData.soundVolume = 5;
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
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
            newState = Keyboard.GetState();

            // Allows the game to exit, only for debugging
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            //    || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    this.Exit();

            // Controls to Mute background music
            if (newState.IsKeyDown(Keys.L) && newState != oldState)
            {
                if (!mute)
                {
                    MediaPlayer.Volume = 0;
                    mute = true;
                }
                else
                {
                    MediaPlayer.Volume = ((float)screenManager.Settings.musicVolume) / 10;
                    mute = false;
                }
            }

            // Update saved state.
            oldState = newState;

            base.Update(gameTime);
        }


        public void wndProc(ref System.Windows.Forms.Message mes)
        {
            if (this.screenManager != null)
                this.screenManager.wndProc(ref mes);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        //protected override void Draw(GameTime gameTime)
        //{

        //}
    }
}
