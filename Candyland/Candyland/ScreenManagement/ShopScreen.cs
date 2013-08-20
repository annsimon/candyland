using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Candyland
{
    class ShopScreen : GameScreen
    {
        Texture2D background;

        SpriteFont font;

        int screenWidth;
        int screenHeight;

        int activeID = 0;

        Rectangle shopBox;

        BonusTile testBonus;

        Vector2 pos1, pos2, pos3, pos4, pos5, pos6;

        private string salesmanID;
        private UpdateInfo m_updateInfo;
        private int chocoCollected;

        public ShopScreen(string saleID, UpdateInfo info, int chocoCount)
        {
            salesmanID = saleID;
            m_updateInfo = info;
            chocoCollected = chocoCount;
        }


        public override void Open(Game game)
        {
            background = ScreenManager.Content.Load<Texture2D>("ScreenTextures/shopBackground");

            font = ScreenManager.Font;

            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            shopBox = new Rectangle(screenWidth / 5 + (screenWidth / 10), screenHeight / 10, screenWidth * 2 / 3, screenHeight * 4 / 5);

            int bonusTileWidth = shopBox.Width / 4;

            pos1 = new Vector2(shopBox.X + bonusTileWidth, shopBox.Y + shopBox.Height/20);
            pos2 = pos1 + new Vector2(bonusTileWidth, 0);
            pos3 = pos2 + new Vector2(bonusTileWidth, 0);
            pos4 = pos1 + new Vector2(0, bonusTileWidth);
            pos5 = pos2 + new Vector2(0, bonusTileWidth);
            pos6 = pos3 + new Vector2(0, bonusTileWidth);

            testBonus = new BonusTile("testBonus", "Flower", "Concept Art", 1, "testBonus");
            testBonus.Texture = ScreenManager.Content.Load<Texture2D>("ScreenTextures/"+testBonus.TextureString);
            testBonus.Rectangle = new Rectangle((int)pos1.X, (int)pos1.Y, bonusTileWidth, bonusTileWidth);
        }

        public override void Update(GameTime gameTime)
        {
            // Exit
            if (ScreenManager.Input.Equals(InputState.Enter))
            {
                ScreenManager.ResumeLast(this);
            }

            bool enterPressed = false;

            // look at input and update button selection

        }

        public override void Draw(GameTime gameTime)
        {
            int offset = 20;

            SpriteBatch m_sprite = ScreenManager.SpriteBatch;

            m_sprite.Begin();

            m_sprite.Draw(background, shopBox, Color.White);
            m_sprite.Draw(testBonus.Texture, testBonus.Rectangle, Color.White);
            m_sprite.DrawString(font, "Preis", new Vector2(shopBox.Left + offset, shopBox.Top + offset), Color.White);
            m_sprite.DrawString(font, "Du hast", new Vector2(shopBox.Left + offset, shopBox.Bottom - 90), Color.White);
            m_sprite.DrawString(font, (chocoCollected - m_updateInfo.chocoChipsSpent).ToString(), new Vector2(shopBox.Left + offset, shopBox.Bottom - 60), Color.White);

            m_sprite.End();

            // we need the following as spriteBatch.Begin() sets them to None and AlphaBlend
            // which breaks our model rendering
            GraphicsDevice m_graphics = ScreenManager.Game.GraphicsDevice;
            m_graphics.DepthStencilState = DepthStencilState.Default;
            m_graphics.BlendState = BlendState.Opaque;
        }
    }
}
