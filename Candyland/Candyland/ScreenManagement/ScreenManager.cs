using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Candyland

    // Uses http://xbox.create.msdn.com/en-US/education/catalog/sample/game_state_management
{
    public class ScreenManager : DrawableGameComponent
    {
        List<GameScreen> screens = new List<GameScreen>();

        SpriteBatch spriteBatch;
        SpriteFont font;
        ContentManager content;
        Texture2D testImageMap;
        String testMap;

        #region getter

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
            get { return font; }
        }

        public ContentManager Content
        {
            get { return content; }
        }

        public Texture2D TestImageMap
        {
            get { return testImageMap; }
        }

        public String TestMap
        {
            get { return testMap;}
        }

        #endregion

        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        public ScreenManager(Game game)
            : base(game)
        {

        }


        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            content = Game.Content;

            testImageMap = content.Load<Texture2D>("TestSkin/ImageMap");
            testMap = File.OpenText(@"C:/Users/Svenja/Documents/GitHub/candyland/Candyland/Candyland/bin/x86/Debug/TestSkin/Map.txt").ReadToEnd();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>("MainText");

            // Open topmost screen
            screens.Last().Open(Game);
        }


        /// <summary>
        /// Tells the currently active screen to update itself.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Active)
                    screen.Update(gameTime);
            }
        }


        /// <summary>
        /// Tells each screen to draw itself.
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

    }
}
