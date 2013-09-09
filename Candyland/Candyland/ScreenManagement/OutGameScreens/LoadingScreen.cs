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

        AnimatingSprite loading;

        // Border
        protected Rectangle MenuBoxTL;
        protected Rectangle MenuBoxTR;
        protected Rectangle MenuBoxBL;
        protected Rectangle MenuBoxBR;
        protected Rectangle MenuBoxL;
        protected Rectangle MenuBoxR;
        protected Rectangle MenuBoxT;
        protected Rectangle MenuBoxB;
        protected Rectangle MenuBoxM;

        protected Texture2D BorderTopLeft;
        protected Texture2D BorderTopRight;
        protected Texture2D BorderBottomLeft;
        protected Texture2D BorderBottomRight;
        protected Texture2D BorderLeft;
        protected Texture2D BorderRight;
        protected Texture2D BorderTop;
        protected Texture2D BorderBottom;
        protected Texture2D BorderMiddle;

        public override void Open(Game game, AssetManager assets)
        {
            this.isFullscreen = true;

            font = ScreenManager.Font;

            FillWithText();

            ContentManager content = ScreenManager.Content;

            screenWidth = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            screenHeight = ScreenManager.Game.GraphicsDevice.Viewport.Height;

            background = assets.optionsScreen;

            loading = new AnimatingSprite();
            loading.Texture = assets.loadingTexture;
            loading.Animations.Add("load", new Animation(600, 100, 6, 0, 0));
            loading.CurrentAnimation = "load";
            loading.isRepeatable = true;
            loading.Animations["load"].FramesPerSecond = 24;
            loading.Position = new Vector2(screenWidth / 2 - 50, screenHeight / 2 - 80);

            loading.StartAnimation();

            BorderTopLeft = assets.dialogTL;
            BorderTopRight = assets.dialogTR;
            BorderBottomLeft = assets.dialogBL;
            BorderBottomRight = assets.dialogBR;
            BorderLeft = assets.dialogL;
            BorderRight = assets.dialogR;
            BorderTop = assets.dialogT;
            BorderBottom = assets.dialogB;
            BorderMiddle = assets.dialogC;

            int width = 500;
            int height = 200;

            MakeBorderBox(new Rectangle((screenWidth) / 2 - width/2, screenHeight/2 - height/2, width, height),
                out MenuBoxTL, out MenuBoxT, out MenuBoxTR, out MenuBoxR,
                out MenuBoxBR, out MenuBoxB, out MenuBoxBL, out MenuBoxL, out MenuBoxM);
        }


        public override void Update(GameTime gameTime)
        {
            timePast += gameTime.ElapsedGameTime.Milliseconds;

            // change displayed message after a few seconds
            if (timePast % 2000 == 0)
            {
                Random rand = new Random();
                int i = rand.Next(19);
                currentMessage = sentences[i];
            }

            loading.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(GameConstants.BackgroundColorLoading);

            ScreenManager.SpriteBatch.Begin();


            // Draw Boarder
            ScreenManager.SpriteBatch.Draw(BorderTopLeft, MenuBoxTL, Color.White);
            ScreenManager.SpriteBatch.Draw(BorderTopRight, MenuBoxTR, Color.White);
            ScreenManager.SpriteBatch.Draw(BorderBottomLeft, MenuBoxBL, Color.White);
            ScreenManager.SpriteBatch.Draw(BorderBottomRight, MenuBoxBR, Color.White);
            ScreenManager.SpriteBatch.Draw(BorderLeft, MenuBoxL, Color.White);
            ScreenManager.SpriteBatch.Draw(BorderRight, MenuBoxR, Color.White);
            ScreenManager.SpriteBatch.Draw(BorderTop, MenuBoxT, Color.White);
            ScreenManager.SpriteBatch.Draw(BorderBottom, MenuBoxB, Color.White);
            ScreenManager.SpriteBatch.Draw(BorderMiddle, MenuBoxM, Color.White);

            // Draw rest

            ScreenManager.SpriteBatch.DrawString(font, currentMessage, new Vector2 ((screenWidth - font.MeasureString(currentMessage).X) / 2, screenHeight/2+25), Color.Black);

            loading.Draw(ScreenManager.SpriteBatch);

            ScreenManager.SpriteBatch.End();
        }


        private void FillWithText()
        {
 	        sentences.Add("Plätzchen werden gebacken");
            sentences.Add("Zuckerwattewolken werden aufgeklopft");
            sentences.Add("Kekse werden abgestaubt");
            sentences.Add("Pralinen werden mit Nugat gefüllt");
            sentences.Add("Zuckerstangen werden gedreht");
            sentences.Add("Bonbons werden eingepackt");
            sentences.Add("Schokolade wird geschmolzen");
            sentences.Add("Schokolade wird in Formen gegossen");
            sentences.Add("Eiscreme wird gerührt");
            sentences.Add("Prise Zimt wird hinzugefügt");
            sentences.Add("Gebäck wird mit Puderzucker bestäubt");
            sentences.Add("Teig wird geknetet");
            sentences.Add("Plätzchen werden ausgestochen");
            sentences.Add("Teig wird probiert");
            sentences.Add("Geschirr wird gespült");
            sentences.Add("Nüsse werden gemahlen, Pistazien gehackt");
            sentences.Add("Marzipanschweinchen werden geformt");
            sentences.Add("Cupcakes werden verziert");
            sentences.Add("Zuckergehalt wird erhöht");
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
