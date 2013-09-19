using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.IsFullScreen = false; 
            Content.RootDirectory = "Content";

            // Create the screen manager component.
            assetManager = new AssetManager();
            screenManager = new ScreenManager(this, graphics.IsFullScreen, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, assetManager);
            Components.Add(screenManager);

           // Content.RootDirectory = "CandylandContent"; 
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
           // IsMouseVisible = true;

            screenManager.AddScreen(new TitleScreen());

            BalanceBoard.initialize(this.Window.Handle);

            base.Initialize();
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
                if (MediaPlayer.Volume == 0) MediaPlayer.Volume = 1;
                else MediaPlayer.Volume = 0;
            }

            // Update saved state.
            oldState = newState;

            base.Update(gameTime);
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
