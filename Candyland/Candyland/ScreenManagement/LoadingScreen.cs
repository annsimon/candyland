using System;
using Microsoft.Xna.Framework;
using System.Threading;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    /// <summary>
    /// http://xboxforums.create.msdn.com/forums/p/45017/468346.aspx
    /// </summary>
    class LoadingScreen : GameScreen
    {

        #region Fields  
 
        Thread backgroundThread;  
        EventWaitHandle backgroundThreadExit;  
        bool loadingIsSlow;  
        bool otherScreensAreGone;  
        int lettermove = 0;

        GameScreen screenToLoad;  
        GameTime loadStartTime;  
 
        string[] letters = { "L", "o", "a", "d", "i", "n", "g", ".", ".", "." };  
        int[] bounce = new int[10];  
        bool[] bounceBool = new bool[10];  
        int MaxBounce = 25;
      
        #endregion  
 
        #region Initialization  
 
 
        /// <summary>  
        /// The constructor is private: loading screens should  
        /// be activated via the static Load method instead.  
        /// </summary>  
        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow,  
                              GameScreen screenToLoad)  
        {  
            this.loadingIsSlow = loadingIsSlow;  
            this.screenToLoad = screenToLoad;  
 
            //TransitionOnTime = TimeSpan.FromSeconds(0.5);  
             
            if (loadingIsSlow)  
            {  
                backgroundThread = new Thread(BackgroundWorkerThread);  
                backgroundThreadExit = new ManualResetEvent(false);  
            }  
 
        }  
 
 
        /// <summary>  
        /// Activates the loading screen.  
        /// </summary>  
        public static void Load(ScreenManager screenManager, bool loadingIsSlow,    
                                 GameScreen screenToLoad)  
        {  
            // Create and activate the loading screen.  
            LoadingScreen loadingScreen = new LoadingScreen(screenManager,  
                                                            loadingIsSlow,  
                                                            screenToLoad);  
            loadingScreen.ScreenState = ScreenState.Active;
            screenManager.AddScreen(loadingScreen);  
        }
        #endregion  
 
        #region Update and Draw  
 
 
        /// <summary>  
        /// Updates the loading screen.  
        /// </summary>  
        public override void Update(GameTime gameTime)  
        {  
    
            // If all the previous screens have finished transitioning  
            // off, it is time to actually perform the load.  
            //if (otherScreensAreGone)  
            //{  
                if (backgroundThread != null)  
                {  
                    loadStartTime = gameTime;  
                    backgroundThread.Start();  
                }

                if (screenToLoad != null)
                {
                    ScreenManager.AddScreen(screenToLoad);
                    screenToLoad.ScreenState = ScreenState.New;
                    screenToLoad.Open(ScreenManager.Game);
                    //ScreenManager.ActivateNewScreen(screenToLoad);
                } 
 
                // Signal the background thread to exit, then wait for it to do so.  
                if (backgroundThread != null)  
                {
                    backgroundThreadExit.Set();  
                    backgroundThread.Join();  
                }
                ScreenManager.RemoveScreen(this);
                // Once the load has finished, we use ResetElapsedTime to tell  
                // the  game timing mechanism that we have just finished a very  
                // long frame, and that it should not try to catch up.  
                ScreenManager.Game.ResetElapsedTime();  
            //}  
        }  
 
        /// <summary>  
        /// Worker thread draws the loading animation and updates the network  
        /// session while the load is taking place.  
        /// </summary>  
        void BackgroundWorkerThread()  
        {  
            long lastTime = Stopwatch.GetTimestamp();  
 
            // EventWaitHandle.WaitOne will return true if the exit signal has  
            // been triggered, or false if the timeout has expired. We use the  
            // timeout to update at regular intervals, then break out of the  
            // loop when we are signalled to exit.  
            while (!backgroundThreadExit.WaitOne(1000 / 30))  
            {  
                GameTime gameTime = GetGameTime(ref lastTime);  
 
                DrawLoadAnimation(gameTime);  
            }  
        }  
 
 
        /// <summary>  
        /// Works out how long it has been since the last background thread update.  
        /// </summary>  
        GameTime GetGameTime(ref long lastTime)  
        {  
            long currentTime = Stopwatch.GetTimestamp();  
            long elapsedTicks = currentTime - lastTime;  
            lastTime = currentTime;  
 
            TimeSpan elapsedTime = TimeSpan.FromTicks(elapsedTicks *  
                                                      TimeSpan.TicksPerSecond /  
                                                      Stopwatch.Frequency);  
 
            return new GameTime(loadStartTime.TotalGameTime + elapsedTime, elapsedTime);  
        }  
 
 
        /// <summary>  
        /// Calls directly into our Draw method from the background worker thread,  
        /// so as to update the load animation in parallel with the actual loading.  
        /// </summary>  
        void DrawLoadAnimation(GameTime gameTime)  
        {  
            if ((ScreenManager.GraphicsDevice == null) || ScreenManager.GraphicsDevice.IsDisposed)  
                return;  
 
            try 
            {  
                ScreenManager.GraphicsDevice.Clear(Color.Black);  
 
                // Draw the loading screen.  
                Draw(gameTime);  
 
                 //If we have a message display component, we want to display  
                 //that over the top of the loading screen, too.  
                //if (messageDisplay != null)  
                //{  
                //    messageDisplay.Update(gameTime);  
                //    messageDisplay.Draw(gameTime);  
                //}  
 
                //ScreenManager.GraphicsDevice.Present();  
            }  
            catch 
            {  
                // If anything went wrong (for instance the graphics device was lost  
                // or reset) we don't have any good way to recover while running on a  
                // background thread. Setting the device to null will stop us from  
                // rendering, so the main game can deal with the problem later on.  
                //ScreenManager.GraphicsDevice = null;  
            }  
        }  
 
        /// <summary>  
        /// Draws the loading screen.  
        /// </summary>  
        public override void Draw(GameTime gameTime)  
        {  
            // If we are the only active screen, that means all the previous screens  
            // must have finished transitioning off. We check for this in the Draw  
            // method, rather than in Update, because it isn't enough just for the  
            // screens to be gone: in order for the transition to look good we must  
            // have actually drawn a frame without them before we perform the load.  
            //if ((ScreenState == ScreenState.Active) &&  
            //    (ScreenManager.GetScreens().Length == 1))  
            //{  
            //    otherScreensAreGone = true;  
            //}  
 
            // The gameplay screen takes a while to load, so we display a loading  
            // message while that is going on, but the menus load very quickly, and  
            // it would look silly if we flashed this up for just a fraction of a  
            // second while returning from the game to the menus. This parameter  
            // tells us how long the loading is going to take, so we know whether  
            // to bother drawing the message.  
            if (loadingIsSlow)  
            {  
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;  
                SpriteFont font = ScreenManager.Font;  
 
                const string message = "Loading...";  
   
               
 
                // Center the text in the viewport.  
                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;  
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);  
                Vector2 textSize = font.MeasureString(message);  
                Vector2 textPosition = (viewportSize - textSize) / 2;  
 
                Color color = Color.White;  
 
                // Draw the text.  
                spriteBatch.Begin();  
 
                
 
                for(int i=0; i< letters.Length ;i++)  
                {  
                    if (i > 0)  
                    {  
                        if (bounceBool[i] == false)  
                        {  
                            if (bounce[i - 1] > 10)  
                            {  
                                if (bounce[i] < 20) bounce[i]++;  
                                else 
                                    bounceBool[i] = true;  
                            }  
                        }  
                        else 
                        {  
                            if (bounce[i - 1] < 10)  
                            {  
                                if (bounce[i] > 0) bounce[i]--;  
                                else bounceBool[i] = false;  
                            }  
                        }  
                    }  
                    else 
                    {  
                        if (bounceBool[i] == false && bounceBool[i+1] ==false)  
                        {  
                            if (bounce[i] < 20) bounce[i]++;  
                            else if(bounce[9]==20)  
                                bounceBool[i] = true;  
                        }  
                        else 
                        {  
                            if (bounce[i] > 0) bounce[i]--;  
                            else bounceBool[i] = false;  
                        }  
                    }  
 
                    spriteBatch.DrawString(font, letters[i], (textPosition + new Vector2(32 * i, -bounce[i]*2)), color);  
                }  
                spriteBatch.End();  
            }  
        }
        #endregion  
 


        public override void Open(Game game)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

    }
}
