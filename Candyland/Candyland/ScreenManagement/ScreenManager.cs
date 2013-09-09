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
        ContentManager content;
        AssetManager assets;

        ScreenInputManager screenInput;
        InputState input;

        private SoundEffect sound;

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
        public ScreenManager(Game game, bool isFullScreen, int prefWidth, int prefHeight, AssetManager assetManager)
            : base(game)
        {
            this.isFullScreen = isFullScreen;
            this.preferedScreenWith = prefWidth;
            this.preferedScreenHeight = prefHeight;
            assets = assetManager;
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

            sound = assets.menuButtonSound;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            if (isFullScreen) mainText = assets.mainTextFullscreen;
            else mainText = assets.mainText;

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
                sound.Play(volume, pitch, pan);
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
            resumedScreen.Resume();
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

            if (!gameIsRunning) StartNewGame();
            // TODO Load last save game
        }

        /// <summary>
        /// Starts a new game
        /// </summary>
        public void StartNewGame()
        {
            if (gameIsRunning)
            {
                while (screens.Count > 0)
                {
                    RemoveScreen(screens.Last());
                }
            }

            // Remove main menu
            if (screens.Count() > 0)
            {
                screens.Last().Close();
                RemoveScreen(screens.Last());
            }

            ActivateNewScreen(new LoadingScreen());

            Thread loadingThread = new Thread(LoadingGameContent);
            loadingThread.Start();
        }

        private void LoadingGameContent()
        {
            m_sceneManager = new SceneManager(this);

            // Load all content required by the scene
            if (gameContent == null)
                gameContent = new ContentManager(Game.Services, "Content");

            m_sceneManager.Load(gameContent, assets);
            readyToStartGame = true;
        }

        public bool readyToStartGame { get; set; }
    }
}
