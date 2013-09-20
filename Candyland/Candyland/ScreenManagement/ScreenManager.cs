using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Candyland

    // Uses http://xbox.create.msdn.com/en-US/education/catalog/sample/game_state_management
{
    public class ScreenManager : DrawableGameComponent
    {
        List<GameScreen> screens = new List<GameScreen>(); //actually used like a stack

        SpriteBatch spriteBatch;
        SpriteFont mainText;
        SpriteFont mainRegular;
        SpriteFont smallText;
        ContentManager content;
        AssetManager assets;
        SaveSettingsData settings;

        ScreenInputManager screenInput;
        InputState input;

        private SoundEffect buttonSound;

        // the menus will be optimized for the prefered screen size
        // Ingame text will have a bigger font
        bool isFullScreen;
        int preferedScreenWith;
        int preferedScreenHeight;

        // will be set, when first game was started
        public bool gameIsRunning;

        // content manager for main game content
        public ContentManager gameContent;

        // the scene manager, most stuff happens in there
        SceneManager m_sceneManager;

        #region getter

        public int PrefScreenWidth
        {
            get { return preferedScreenWith; }
        }

        public int PrefScreenHeight
        {
            get { return preferedScreenHeight; }
        }

        public bool isFullscreen
        {
            get { return isFullScreen; }
        }

        public SceneManager SceneManager
        {
            get { return m_sceneManager; }
        }

        public SaveSettingsData Settings
        {
            get { return settings; }
        }

        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }


        /// <summary>
        /// A default font shared by all the screens. This saves
        /// each screen having to bother loading their own local copy.
        /// </summary>
        public SpriteFont Font
        {
            get { return mainText; }
        }

        public SpriteFont FontRegular
        {
            get { return mainRegular; }
        }

        public SpriteFont FontSmall
        {
            get { return smallText; }
        }

        public ContentManager Content
        {
            get { return content; }
        }

        public InputState Input
        {
            get { return input; }
        }

        #endregion

        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        public ScreenManager(Game game, bool isFullScreen, int prefWidth, int prefHeight, AssetManager assetManager, SaveSettingsData settings)
            : base(game)
        {
            this.isFullScreen = isFullScreen;
            this.preferedScreenWith = prefWidth;
            this.preferedScreenHeight = prefHeight;
            assets = assetManager;
            this.settings = settings;
        }


        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            content = Game.Content;

            assets.Load(content);

            screenInput = new ScreenInputManager();

            buttonSound = assets.menuButtonSound;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            if (isFullScreen) mainText = assets.mainTextFullscreen;
            else mainText = assets.mainText;
            mainRegular = assets.mainRegular;
            smallText = assets.smallText;

            // Open topmost screen
            screens.Last().ScreenState = ScreenState.Active;
            screens.Last().Open(Game, assets);
        }


        /// <summary>
        /// Tells the currently active screen to update itself.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // when loading thread has finished its work, start the game
            if (readyToStartGame)
            {
                RemoveScreen(screens.Last());
                ActivateNewScreen(new MainGame());
                readyToStartGame = false;               
            }

            input = screenInput.getInput();

            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Active)
                {
                    screen.Update(gameTime);
                    return;
                }
            }
            if (input.Equals(InputState.Continue))
            {
                float volume = 0.5f;
                float pitch = 0.0f;
                float pan = 0.0f;
                buttonSound.Play(volume, pitch, pan);
            }
        }


        /// <summary>
        /// Tells each visible screen to draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Visible
                        || screen.ScreenState == ScreenState.Active)
                    screen.Draw(gameTime);
            }
        }


        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screens.Add(screen);
        }

        /// <summary>
        /// Removes screen from the screen manager.
        /// </summary>
        public void RemoveScreen(GameScreen screen)
        {
            screen.Close();
            screens.Remove(screen);
        }

        /// <summary>
        /// Adds a new screen to the screen manager and makes it the new active screen.
        /// </summary>
        public void ActivateNewScreen(GameScreen newScreen)
        {
            if (screens.Count() > 0)
            {
                // Check if last screen was already loaded
                GameScreen currentScreen = screens.Last();
                if (!currentScreen.ScreenState.Equals(ScreenState.New))
                {
                    // Check if last screen will be hidden by new screen
                    if (newScreen.IsFullscreen)
                    {
                        currentScreen.ScreenState = ScreenState.Hidden;
                    }
                    else if (!currentScreen.ScreenState.Equals(ScreenState.Hidden)) currentScreen.ScreenState = ScreenState.Visible;
                }
            }

            // Add new screen
            AddScreen(newScreen);
            newScreen.ScreenState = ScreenState.Active;
            newScreen.Open(Game, assets);
        }

        /// <summary>
        /// Closes the current screen and returns to the screen, that was active before that one
        /// </summary>
        public void ResumeLast(GameScreen closingScreen)
        {
            // Get rid of currently active screen
            closingScreen.Close();
            screens.Remove(closingScreen);

            // Go back to recent screen
            GameScreen resumedScreen = screens.Last();
            resumedScreen.ScreenState = ScreenState.Active;
            resumedScreen.Resume(); // Maybe used to manage music starts and stops
        }

        /// <summary>
        /// Returns to currently running game (if there is one) or loads the last save game
        /// </summary>
        public void ResumeGame()
        {
            // Search for already running game, should be second last screen in list
            if (screens.Count() > 1)
            {
                if (screens.ElementAt(screens.Count - 2).GetType() == typeof(MainGame))
                {
                    // In case of returning to a running game
                    ResumeLast(screens.Last());

                    return;
                }
            }

            if (!gameIsRunning) LoadLastSavegame();
        }

        private void LoadLastSavegame()
        {
            // Remove main menu
            if (screens.Count() > 0)
            {
                screens.Last().Close();
                RemoveScreen(screens.Last());
            }

            ActivateNewScreen(new LoadingScreen());

            Thread loadingThread = new Thread(LoadingGameContentAndSaveGame);
            loadingThread.Start();
        }

        /// <summary>
        /// Starts a new game
        /// </summary>
        public void StartNewGame()
        {
            // Remove main menu
            if (screens.Count() > 0)
            {
                RemoveScreen(screens.Last());
            }

            // First case: a game is already running and content has been loaded
            if (gameIsRunning)
            {
                // Remove old game
                RemoveScreen(screens.Last());

                // Show loading screen
                ActivateNewScreen(new LoadingScreen());

                Thread loadingThread = new Thread(LoadingGameContent);
                loadingThread.Start();
            }

            // Second case: no running game and content needs to be loaded
            if (!gameIsRunning)
            {
                // Show loading screen
                ActivateNewScreen(new LoadingScreen());

                Thread loadingThread = new Thread(LoadingGameContent);
                loadingThread.Start();
            }
        }

        private void LoadingGameContent()
        {
            m_sceneManager = new SceneManager(this);

            m_sceneManager.Load(content, assets);

            readyToStartGame = true;
        }

        private void LoadingGameContentAndSaveGame()
        {
            m_sceneManager = new SceneManager(this);

            m_sceneManager.Load(content, assets);

            readyToStartGame = m_sceneManager.LoadSavegame();
        }

        /// <summary>
        /// A new game will be started by using the already loaded content and just reseting some values
        /// </summary>
        private void GameReset()
        {
            readyToStartGame = m_sceneManager.ResetDataForNewGame();
        }

        public bool readyToStartGame { get; set; }
    }
}
