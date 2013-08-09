using Microsoft.Xna.Framework;

namespace Candyland
{
    public enum ScreenState
    {
        New,
        Hidden,
        Visible,
        Active,
    }

    /// <summary>
    /// Each game state has its one screen
    /// </summary>
    public abstract class GameScreen
    {
        /// <summary>
        /// Tells if screen is fullscreen
        /// </summary>
        public bool IsFullscreen
        {
            get { return isFullscreen; }
        }
        protected bool isFullscreen = false;

        /// <summary>
        /// Gets or sets the current screen state.
        /// </summary>
        public ScreenState ScreenState
        {
            get { return screenState; }
            set { screenState = value; }
        }
        ScreenState screenState = ScreenState.New;


        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }
        ScreenManager screenManager;


        /// <summary>
        /// Initialize Screen and Load its Content
        /// </summary>
        public abstract void Open(Game game);

        /// <summary>
        /// Reactivate screen
        /// </summary>
        public virtual void Resume()
        {
        }

        /// <summary>
        /// Update screen
        /// </summary>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draw screen
        /// </summary>
        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Deactivate screen
        /// </summary>
        public virtual void Leave()
        {
        }

        /// <summary>
        /// Clean up screen before closing (Stop music, Unload content...)
        /// </summary>
        public virtual void Close()
        {
        }


    }
}
