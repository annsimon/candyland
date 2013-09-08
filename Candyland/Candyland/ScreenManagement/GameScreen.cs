using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public abstract void Open(Game game, AssetManager assets);

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

        /// <summary>
        /// Calculates the size and position of nine rectangles, which then fit into the target rectangle
        /// </summary>
        /// <param name="TargetBox"></param>
        /// <param name="tl"></param>
        /// <param name="bl"></param>
        /// <param name="tr"></param>
        /// <param name="br"></param>
        /// <param name="t"></param>
        /// <param name="l"></param>
        /// <param name="b"></param>
        /// <param name="r"></param>
        /// <param name="m"></param>
        public void MakeBorderBox(Rectangle TargetBox,
            out Rectangle tl, out Rectangle t, out Rectangle tr, out Rectangle r,
            out Rectangle br, out Rectangle b, out Rectangle bl, out Rectangle l, out Rectangle m)
        {
            int left = TargetBox.Left;
            int right = TargetBox.Right;
            int top = TargetBox.Top;
            int bottom = TargetBox.Bottom;
            int width = TargetBox.Width;
            int height = TargetBox.Height;

            tl = new Rectangle(left, top, 42, 49);
            tr = new Rectangle(right - 42, top, 42, 49);
            bl = new Rectangle(left, bottom - 49, 42, 49);
            br = new Rectangle(right - 42, bottom - 49, 42, 49);
            l = new Rectangle(left, top + 49, 42, height - 96);
            r = new Rectangle(right - 42, top + 49, 42, height -96);
            t = new Rectangle(left + 42, top, width - 84, 49);
            b = new Rectangle(left + 42,bottom - 49, width - 84 , 49);
            m = new Rectangle(left + 42, top + 49, width - 84, height - 96);
        }

    }
}
