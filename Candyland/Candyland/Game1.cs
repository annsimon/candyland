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

        KeyboardState oldState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Create the screen manager component.
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            screenManager.AddScreen(new MainGame());
            //screenManager.AddScreen(new MainMenu());

            GameConstants.screenWidth = graphics.PreferredBackBufferWidth;
            GameConstants.screenHeight = graphics.PreferredBackBufferHeight;

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
            KeyboardState newState = Keyboard.GetState();

            //if (this.IsActive && (gameTime.TotalGameTime.Milliseconds % GameConstants.framerate == 0 )
            //    && ((newState.IsKeyDown(Keys.Enter)&& newState != oldState) || GameConstants.singlestepperOFF))
            //{
                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                    || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    this.Exit();

            //    m_sceneManager.Update(gameTime);
            //}
            // Controls to Mute background music
            if (newState.IsKeyDown(Keys.L) && newState != oldState)
            {
                if (MediaPlayer.Volume == 0) MediaPlayer.Volume = 1;
                else MediaPlayer.Volume = 0;
            }


           newState = Keyboard.GetState();

            //// Save, when F5 was pressed and now released
            //if (oldState.IsKeyDown(Keys.F5) && newState.IsKeyUp(Keys.F5))
            //{
            //    System.Diagnostics.Debug.WriteLine("Saving");
            //    m_sceneManager.Save();
            //}

            //// Load last savegame, when F6 was pressed and now released
            //if (oldState.IsKeyDown(Keys.F6) && newState.IsKeyUp(Keys.F6))
            //{
            //    m_sceneManager.Load();
            //}

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
