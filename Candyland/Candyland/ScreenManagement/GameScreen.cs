using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Candyland
{
    public enum ScreenState
    {
        Hidden,
        Visible,
        Active,
    }

    public abstract class GameScreen
    {
        /// <summary>
        /// Gets the current screen state.
        /// </summary>
        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        ScreenState screenState = ScreenState.Hidden;


        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        ScreenManager screenManager;



        public abstract void Open(Game game); // Initialize Screen and Load its Content
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
        public abstract void Close(); // Stop music, Unload content...


    }
}
