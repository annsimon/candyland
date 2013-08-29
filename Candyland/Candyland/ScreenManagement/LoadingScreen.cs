using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    /// <summary>
    /// shows an animation while a parallel tread loads the game content
    /// </summary>
    class LoadingScreen : GameScreen
    {
        Texture2D background;
        int screenWidth;
        int screenHeight;

        private SpriteFont font;

        private int timePast;
        private string currentMessage = "Candyland wird vorbereitet";

        List<string> sentences = new List<string>();

        public override void Open(Game game)
        {
            this.isFullscreen = true;

            font = ScreenManager.Font;

            FillWithText();

            ContentManager content = ScreenManager.Content;

            screenWidth = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            screenHeight = ScreenManager.Game.GraphicsDevice.Viewport.Height;

            background = content.Load<Texture2D>("ScreenTextures/optionsScreen");
        }


        public override void Update(GameTime gameTime)
        {
            timePast += gameTime.ElapsedGameTime.Milliseconds;

            // change displayed message after a few seconds
            if (timePast % 250 == 0)
            {
                Random rand = new Random();
                int i = rand.Next(19);
                currentMessage = sentences[i];
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.Black);

            ScreenManager.SpriteBatch.Begin();

            ScreenManager.SpriteBatch.DrawString(font, currentMessage, new Vector2 ((screenWidth - font.MeasureString(currentMessage).X) / 2, screenHeight/2), Color.White);

            ScreenManager.SpriteBatch.End();
        }


        private void FillWithText()
        {
 	        sentences.Add("Plätzchen werden gebacken");
            sentences.Add("Zuckerwattewolken werden aufgeklopft");
            sentences.Add("Kekse werden abgestaubt");
            sentences.Add("Pralinen mit Nugat füllen");
            sentences.Add("Zuckerstangen werden gedreht");
            sentences.Add("Bonbons werden eingepackt");
            sentences.Add("Schokolade wird geschmolzen");
            sentences.Add("Schokolade wird in Formen gegossen");
            sentences.Add("Eiscreme wird gerührt");
            sentences.Add("Noch etwas Zimt");
            sentences.Add("Mit Puderzucker bestäuben");
            sentences.Add("Teig kneten");
            sentences.Add("Plätzchen werden ausgestochen");
            sentences.Add("Teig probieren");
            sentences.Add("Geschirr spülen und aufräumen");
            sentences.Add("Nüsse werden gemahlen, Pistazien gehackt");
            sentences.Add("Marzipanschweinchen werden geformt");
            sentences.Add("Cupcakes verzieren");
            sentences.Add("Noch etwas mehr Zucker");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
            //sentences.Add("");
        }
    }
}
