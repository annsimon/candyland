﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Candyland
{
    class ShopScreen : GameScreen
    {
        #region fields

        SpriteFont font;
        AssetManager m_assets;

        int screenWidth;
        int screenHeight;

        int activeID = 1;
        int numberOfItems = 0;

        protected Rectangle bigBox;
        protected Rectangle shopBox;
        protected Rectangle tileBox1, tileBox2, tileBox3, tileBox4, tileBox5, tileBox6,
            tileBox7, tileBox8, tileBox9, tileBox10, tileBox11, tileBox12; 

        protected Rectangle MenuBoxTL;
        protected Rectangle MenuBoxTR;
        protected Rectangle MenuBoxBL;
        protected Rectangle MenuBoxBR;
        protected Rectangle MenuBoxL;
        protected Rectangle MenuBoxR;
        protected Rectangle MenuBoxT;
        protected Rectangle MenuBoxB;
        protected Rectangle MenuBoxM;

        protected Rectangle ShopBoxTL;
        protected Rectangle ShopBoxTR;
        protected Rectangle ShopBoxBL;
        protected Rectangle ShopBoxBR;
        protected Rectangle ShopBoxL;
        protected Rectangle ShopBoxR;
        protected Rectangle ShopBoxT;
        protected Rectangle ShopBoxB;
        protected Rectangle ShopBoxM;

        protected Rectangle TileBoxTL1, TileBoxTL2, TileBoxTL3, TileBoxTL4, TileBoxTL5, TileBoxTL6,
            TileBoxTL7, TileBoxTL8, TileBoxTL9, TileBoxTL10, TileBoxTL11, TileBoxTL12;
        protected Rectangle TileBoxTR1, TileBoxTR2, TileBoxTR3, TileBoxTR4, TileBoxTR5, TileBoxTR6,
            TileBoxTR7, TileBoxTR8, TileBoxTR9, TileBoxTR10, TileBoxTR11, TileBoxTR12;
        protected Rectangle TileBoxBL1, TileBoxBL2, TileBoxBL3, TileBoxBL4, TileBoxBL5, TileBoxBL6,
            TileBoxBL7, TileBoxBL8, TileBoxBL9, TileBoxBL10, TileBoxBL11, TileBoxBL12;
        protected Rectangle TileBoxBR1, TileBoxBR2, TileBoxBR3, TileBoxBR4, TileBoxBR5, TileBoxBR6,
            TileBoxBR7, TileBoxBR8, TileBoxBR9, TileBoxBR10, TileBoxBR11, TileBoxBR12;
        protected Rectangle TileBoxB1, TileBoxB2, TileBoxB3, TileBoxB4, TileBoxB5, TileBoxB6,
            TileBoxB7, TileBoxB8, TileBoxB9, TileBoxB10, TileBoxB11, TileBoxB12;
        protected Rectangle TileBoxL1, TileBoxL2, TileBoxL3, TileBoxL4, TileBoxL5, TileBoxL6,
            TileBoxL7, TileBoxL8, TileBoxL9, TileBoxL10, TileBoxL11, TileBoxL12;
        protected Rectangle TileBoxR1, TileBoxR2, TileBoxR3, TileBoxR4, TileBoxR5, TileBoxR6,
            TileBoxR7, TileBoxR8, TileBoxR9, TileBoxR10, TileBoxR11, TileBoxR12;
        protected Rectangle TileBoxT1, TileBoxT2, TileBoxT3, TileBoxT4, TileBoxT5, TileBoxT6,
            TileBoxT7, TileBoxT8, TileBoxT9, TileBoxT10, TileBoxT11, TileBoxT12;
        protected Rectangle TileBoxM1, TileBoxM2, TileBoxM3, TileBoxM4, TileBoxM5, TileBoxM6,
            TileBoxM7, TileBoxM8, TileBoxM9, TileBoxM10, TileBoxM11, TileBoxM12;

        protected Texture2D BorderTopLeft;
        protected Texture2D BorderTopRight;
        protected Texture2D BorderBottomLeft;
        protected Texture2D BorderBottomRight;
        protected Texture2D BorderLeft;
        protected Texture2D BorderRight;
        protected Texture2D BorderTop;
        protected Texture2D BorderBottom;
        protected Texture2D BorderMiddle;

        protected Texture2D chocoChip;

        List<BonusTile> forSale;
        BonusTracker m_bonusTracker;

        int bonusTileWidth;

        Vector2 pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, pos10, pos11, pos12;

        Color color1, color2, color3, color4, color5, color6, color7, color8, color9, color10, color11, color12;

        private string salesmanID;
        private UpdateInfo m_updateInfo;
        private int chocoCollected;

        private SoundEffect errorSound;

        Button backButton;

        #endregion fields

        public ShopScreen(string saleID, UpdateInfo info, int chocoCount)
        {
            salesmanID = saleID;
            m_updateInfo = info;
            chocoCollected = chocoCount;
        }


        public override void Open(Game game, AssetManager assets)
        {
            font = assets.mainText;
            m_assets = assets;

            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            BorderTopLeft = assets.dialogTL;
            BorderTopRight = assets.dialogTR;
            BorderBottomLeft = assets.dialogBL;
            BorderBottomRight = assets.dialogBR;
            BorderLeft = assets.dialogL;
            BorderRight = assets.dialogR;
            BorderTop = assets.dialogT;
            BorderBottom = assets.dialogB;
            BorderMiddle = assets.dialogC;

            chocoChip = assets.chocoChip;

            errorSound = assets.menuButtonError;

            // What's there to sell?
            m_bonusTracker = ScreenManager.SceneManager.getBonusTracker();
            forSale = new List<BonusTile>();
            foreach (BonusTile bonus in m_bonusTracker.conceptArts)
            {
                if(!m_bonusTracker.soldItems.Contains(bonus.ID)) // not yet sold
                {
                    forSale.Add(bonus);
                    switch (bonus.TextureString)
                    {
                        case "player": bonus.Texture = assets.CAplayer; break;
                        case "helper": bonus.Texture = assets.CAhelper; break;
                        case "aca": bonus.Texture = assets.CAacaguy; break;
                        case "lakritz": bonus.Texture = assets.CAlakritz; break;
                        case "platform": bonus.Texture = assets.CAplatform; break;
                        case "switch": bonus.Texture = assets.CAswitch; break;
                        case "bonbon": bonus.Texture = assets.CAbonbon; break;
                        case "salesman": bonus.Texture = assets.CAsalesman; break;
                        case "cupcake1": bonus.Texture = assets.CAcupcake1; break;
                        case "cupcake2": bonus.Texture = assets.CAcupcake2; break;
                        case "cupcake3": bonus.Texture = assets.CAcupcake3; break;
                        case "cupcake4": bonus.Texture = assets.CAcupcake4; break;
                    }
                }
            }
            
            // layout stuff
            int offset = 5;

            // Big Box
            int MenuBoxWidth = ScreenManager.PrefScreenWidth - 2 * offset;
            int MenuBoxHeight = ScreenManager.PrefScreenHeight - 2 * offset;
            bigBox = new Rectangle((screenWidth - MenuBoxWidth) / 2, (screenHeight - MenuBoxHeight) / 2, MenuBoxWidth, MenuBoxHeight);
            MakeBorderBox(bigBox,
                out MenuBoxTL, out MenuBoxT, out MenuBoxTR, out MenuBoxR,
                out MenuBoxBR, out MenuBoxB, out MenuBoxBL, out MenuBoxL, out MenuBoxM);
            // Shop Box
            shopBox = new Rectangle(bigBox.X + bigBox.Width / 5, bigBox.Y + 2 * offset, bigBox.Width * 4 / 5 - 2*offset, bigBox.Height - 4 * offset);
            MakeBorderBox(shopBox,
                out ShopBoxTL, out ShopBoxT, out ShopBoxTR, out ShopBoxR,
                out ShopBoxBR, out ShopBoxB, out ShopBoxBL, out ShopBoxL, out ShopBoxM);

            // Bonus Tiles
            bonusTileWidth = 120;

            int tileDistX = (shopBox.Width - 4 * bonusTileWidth) / 5;
            int tileDistY = (shopBox.Height - 3 * bonusTileWidth) / 6;
            int tileDistTotalX = bonusTileWidth + (shopBox.Width - 4 * bonusTileWidth) / 5;
            int tileDistTotalY = bonusTileWidth + (shopBox.Height - 3 * bonusTileWidth) / 6;

            pos1 = new Vector2(shopBox.X + tileDistX, shopBox.Y + 2 * tileDistY);
            pos2 = pos1 + new Vector2(tileDistTotalX, 0);
            pos3 = pos2 + new Vector2(tileDistTotalX, 0);
            pos4 = pos3 + new Vector2(tileDistTotalX, 0);

            pos5 = pos1 + new Vector2(0, tileDistTotalY);
            pos6 = pos2 + new Vector2(0, tileDistTotalY);
            pos7 = pos3 + new Vector2(0, tileDistTotalY);
            pos8 = pos4 + new Vector2(0, tileDistTotalY);

            pos9 = pos5 + new Vector2(0, tileDistTotalY);
            pos10 = pos6 + new Vector2(0, tileDistTotalY);
            pos11 = pos7 + new Vector2(0, tileDistTotalY);
            pos12 = pos8 + new Vector2(0, tileDistTotalY);

            // Tile Box
            MakeTileBoxes();

            numberOfItems = forSale.Count;

            backButton = new Button("Zurück", new Vector2(MenuBoxL.Right - 25, MenuBoxB.Top - 20), assets, this);
        }


        public override void Update(GameTime gameTime)
        {
            // Exit
            if (ScreenManager.Input.Equals(InputState.Back))
            {
                ScreenManager.ResumeLast(this);
                return;
            }

            bool enterPressed = false;         

            // look at input and update shop selection
            switch (ScreenManager.Input)
            {
                case InputState.Continue: enterPressed = true; break;
                case InputState.Left: if(activeID % 4 == 1) activeID = 0; else activeID--; break;
                case InputState.Right: activeID++; break;
                case InputState.Up: if(activeID - 4 >= 1)activeID -= 4; break;
                case InputState.Down: if (activeID + 4 <= numberOfItems) activeID += 4; break;
            }
            if (activeID >= numberOfItems) activeID = numberOfItems;
            if (activeID < 0) activeID = 0;

            // higlight back button
            if (activeID == 0) backButton.selected = true;

            if (enterPressed)
            {
                // Exit with back button
                if (activeID == 0)
                {
                    ScreenManager.ResumeLast(this);
                    return;
                }
                if (forSale.Count() == 0 || (m_bonusTracker.chocoCount - m_bonusTracker.chocoChipsSpent) < forSale[activeID - 1].Price)
                {
                    float pitch = 0.0f;
                    float pan = 0.0f;
                    errorSound.Play(((float)m_updateInfo.soundVolume) / 10, pitch, pan);
                }
                else
                    ScreenManager.ActivateNewScreen(new BuyQuestion(forSale[activeID - 1], forSale));
            }

            // count again after buying
            numberOfItems = forSale.Count;
        }

        public override void Draw(GameTime gameTime)
        {
            // count again after buying
            numberOfItems = forSale.Count;

            int offset = 20;

            SpriteBatch m_sprite = ScreenManager.SpriteBatch;

            color1 = Color.White;
            color2 = Color.White;
            color3 = Color.White;
            color4 = Color.White;
            color5 = Color.White;
            color6 = Color.White;
            color7 = Color.White;
            color8 = Color.White;
            color9 = Color.White;
            color10 = Color.White;
            color11 = Color.White;
            color12 = Color.White;
            Color selectColor = Color.GreenYellow;

            switch (activeID)
            {
                case 1: color1 = selectColor; break;
                case 2: color2 = selectColor; break;
                case 3: color3 = selectColor; break;
                case 4: color4 = selectColor; break;
                case 5: color5 = selectColor; break;
                case 6: color6 = selectColor; break;
                case 7: color7 = selectColor; break;
                case 8: color8 = selectColor; break;
                case 9: color9 = selectColor; break;
                case 10: color10 = selectColor; break;
                case 11: color11 = selectColor; break;
                case 12: color12 = selectColor; break;
            }

            m_sprite.Begin();

            // Draw Border
            DrawBigBox(m_sprite);

            backButton.Draw(m_sprite);

            DrawShopBox(m_sprite);

            DrawTiles(m_sprite);

            DrawBonusPictures(m_sprite);

            Color textColor = Color.Black;

            Vector2 pricePos = new Vector2(bigBox.Left + offset + 10, bigBox.Top + offset + font.LineSpacing);
            m_sprite.DrawString(font, "Preis", new Vector2(bigBox.Left + offset, bigBox.Top + offset + 5), textColor);
            if (forSale.Count != 0 && activeID <= forSale.Count && activeID > 0)
            {
                m_sprite.DrawString(font, forSale[activeID - 1].Price.ToString(), pricePos, textColor);
            }
            else m_sprite.DrawString(font, "0", pricePos, textColor);
            m_sprite.Draw(chocoChip, new Rectangle((int)pricePos.X + 40, (int)pricePos.Y, 24, 30), Color.White);
            m_sprite.DrawString(font, "Du hast", new Vector2(bigBox.Left + offset, bigBox.Top + 200), textColor);

            Vector2 availableChocosPos = new Vector2(bigBox.Left + offset + 10, bigBox.Top + 235);
            m_sprite.DrawString(font, (chocoCollected - m_bonusTracker.chocoChipsSpent).ToString(), availableChocosPos, textColor);
            m_sprite.Draw(chocoChip, new Rectangle((int)availableChocosPos.X + 40, (int)availableChocosPos.Y, 24, 30), Color.White);

            m_sprite.End();

            // we need the following as spriteBatch.Begin() sets them to None and AlphaBlend
            // which breaks our model rendering
            GraphicsDevice m_graphics = ScreenManager.Game.GraphicsDevice;
            m_graphics.DepthStencilState = DepthStencilState.Default;
            m_graphics.BlendState = BlendState.Opaque;
        }

        private void DrawBonusPictures(SpriteBatch m_sprite)
        {
            int offset = 10;
            int bonusPictureWidth = bonusTileWidth - 2 * offset;
            Color white = Color.White;
            int i = 0;

            if (i >= forSale.Count) return;
            forSale[0].Draw(m_sprite, (int)pos1.X + offset, (int)pos1.Y + offset, bonusPictureWidth, bonusPictureWidth, white, m_assets);
            i++;
            if (i >= forSale.Count) return;
            forSale[1].Draw(m_sprite, (int)pos2.X + offset, (int)pos2.Y + offset, bonusPictureWidth, bonusPictureWidth, white, m_assets);
            i++;
            if (i >= forSale.Count) return;
            forSale[2].Draw(m_sprite, (int)pos3.X + offset, (int)pos3.Y + offset, bonusPictureWidth, bonusPictureWidth, white, m_assets);
            i++;
            if (i >= forSale.Count) return;
            forSale[3].Draw(m_sprite, (int)pos4.X + offset, (int)pos4.Y + offset, bonusPictureWidth, bonusPictureWidth, white, m_assets);
            i++;
            if (i >= forSale.Count) return;
            forSale[4].Draw(m_sprite, (int)pos5.X + offset, (int)pos5.Y + offset, bonusPictureWidth, bonusPictureWidth, white, m_assets);
            i++;
            if (i >= forSale.Count) return;
            forSale[5].Draw(m_sprite, (int)pos6.X + offset, (int)pos6.Y + offset, bonusPictureWidth, bonusPictureWidth, white, m_assets);
            i++;
            if (i >= forSale.Count) return;
            forSale[6].Draw(m_sprite, (int)pos7.X + offset, (int)pos7.Y + offset, bonusPictureWidth, bonusPictureWidth, white, m_assets);
            i++;
            if (i >= forSale.Count) return;
            forSale[7].Draw(m_sprite, (int)pos8.X + offset, (int)pos8.Y + offset, bonusPictureWidth, bonusPictureWidth, white, m_assets);
            i++;
            if (i >= forSale.Count) return;
            forSale[8].Draw(m_sprite, (int)pos9.X + offset, (int)pos9.Y + offset, bonusPictureWidth, bonusPictureWidth, white, m_assets);
            i++;
            if (i >= forSale.Count) return;
            forSale[9].Draw(m_sprite, (int)pos10.X + offset, (int)pos10.Y + offset, bonusPictureWidth, bonusPictureWidth, white, m_assets);
            i++;
            if (i >= forSale.Count) return;
            forSale[10].Draw(m_sprite, (int)pos11.X + offset, (int)pos11.Y + offset, bonusPictureWidth, bonusPictureWidth, white, m_assets);
            i++;
            if (i >= forSale.Count) return;
            forSale[11].Draw(m_sprite, (int)pos12.X + offset, (int)pos12.Y + offset, bonusPictureWidth, bonusPictureWidth, white, m_assets);
            i++;
        }

                private void MakeTileBoxes()
        {
            tileBox1 = new Rectangle((int)pos1.X, (int)pos1.Y, bonusTileWidth, bonusTileWidth);
            MakeBorderBox(tileBox1,
                out TileBoxTL1, out TileBoxT1, out TileBoxTR1, out TileBoxR1,
                out TileBoxBR1, out TileBoxB1, out TileBoxBL1, out TileBoxL1, out TileBoxM1);
            tileBox2 = new Rectangle((int)pos2.X, (int)pos2.Y, bonusTileWidth, bonusTileWidth);
            MakeBorderBox(tileBox2,
                out TileBoxTL2, out TileBoxT2, out TileBoxTR2, out TileBoxR2,
                out TileBoxBR2, out TileBoxB2, out TileBoxBL2, out TileBoxL2, out TileBoxM2);
            tileBox3 = new Rectangle((int)pos3.X, (int)pos3.Y, bonusTileWidth, bonusTileWidth);
            MakeBorderBox(tileBox3,
                out TileBoxTL3, out TileBoxT3, out TileBoxTR3, out TileBoxR3,
                out TileBoxBR3, out TileBoxB3, out TileBoxBL3, out TileBoxL3, out TileBoxM3);
            tileBox4 = new Rectangle((int)pos4.X, (int)pos4.Y, bonusTileWidth, bonusTileWidth);
            MakeBorderBox(tileBox4,
                out TileBoxTL4, out TileBoxT4, out TileBoxTR4, out TileBoxR4,
                out TileBoxBR4, out TileBoxB4, out TileBoxBL4, out TileBoxL4, out TileBoxM4);
            tileBox5 = new Rectangle((int)pos5.X, (int)pos5.Y, bonusTileWidth, bonusTileWidth);
            MakeBorderBox(tileBox5,
                out TileBoxTL5, out TileBoxT5, out TileBoxTR5, out TileBoxR5,
                out TileBoxBR5, out TileBoxB5, out TileBoxBL5, out TileBoxL5, out TileBoxM5);
            tileBox6 = new Rectangle((int)pos6.X, (int)pos6.Y, bonusTileWidth, bonusTileWidth);
            MakeBorderBox(tileBox6,
                out TileBoxTL6, out TileBoxT6, out TileBoxTR6, out TileBoxR6,
                out TileBoxBR6, out TileBoxB6, out TileBoxBL6, out TileBoxL6, out TileBoxM6);
            tileBox7 = new Rectangle((int)pos7.X, (int)pos7.Y, bonusTileWidth, bonusTileWidth);
            MakeBorderBox(tileBox7,
                out TileBoxTL7, out TileBoxT7, out TileBoxTR7, out TileBoxR7,
                out TileBoxBR7, out TileBoxB7, out TileBoxBL7, out TileBoxL7, out TileBoxM7);
            tileBox8 = new Rectangle((int)pos8.X, (int)pos8.Y, bonusTileWidth, bonusTileWidth);
            MakeBorderBox(tileBox8,
                out TileBoxTL8, out TileBoxT8, out TileBoxTR8, out TileBoxR8,
                out TileBoxBR8, out TileBoxB8, out TileBoxBL8, out TileBoxL8, out TileBoxM8);
            tileBox9 = new Rectangle((int)pos9.X, (int)pos9.Y, bonusTileWidth, bonusTileWidth);
            MakeBorderBox(tileBox9,
                out TileBoxTL9, out TileBoxT9, out TileBoxTR9, out TileBoxR9,
                out TileBoxBR9, out TileBoxB9, out TileBoxBL9, out TileBoxL9, out TileBoxM9);
            tileBox10 = new Rectangle((int)pos10.X, (int)pos10.Y, bonusTileWidth, bonusTileWidth);
            MakeBorderBox(tileBox10,
                out TileBoxTL10, out TileBoxT10, out TileBoxTR10, out TileBoxR10,
                out TileBoxBR10, out TileBoxB10, out TileBoxBL10, out TileBoxL10, out TileBoxM10);
            tileBox11 = new Rectangle((int)pos11.X, (int)pos11.Y, bonusTileWidth, bonusTileWidth);
            MakeBorderBox(tileBox11,
                out TileBoxTL11, out TileBoxT11, out TileBoxTR11, out TileBoxR11,
                out TileBoxBR11, out TileBoxB11, out TileBoxBL11, out TileBoxL11, out TileBoxM11);
            tileBox12 = new Rectangle((int)pos12.X, (int)pos12.Y, bonusTileWidth, bonusTileWidth);
            MakeBorderBox(tileBox12,
                out TileBoxTL12, out TileBoxT12, out TileBoxTR12, out TileBoxR12,
                out TileBoxBR12, out TileBoxB12, out TileBoxBL12, out TileBoxL12, out TileBoxM12);
        }

        private void DrawTiles(SpriteBatch m_sprite)
        {
                m_sprite.Draw(BorderTopLeft, TileBoxTL1, color1);
                m_sprite.Draw(BorderTopRight, TileBoxTR1, color1);
                m_sprite.Draw(BorderBottomLeft, TileBoxBL1, color1);
                m_sprite.Draw(BorderBottomRight, TileBoxBR1, color1);
                m_sprite.Draw(BorderLeft, TileBoxL1, color1);
                m_sprite.Draw(BorderRight, TileBoxR1, color1);
                m_sprite.Draw(BorderTop, TileBoxT1, color1);
                m_sprite.Draw(BorderBottom, TileBoxB1, color1);
                m_sprite.Draw(BorderMiddle, TileBoxM1, color1);

                m_sprite.Draw(BorderTopLeft, TileBoxTL2, color2);
                m_sprite.Draw(BorderTopRight, TileBoxTR2, color2);
                m_sprite.Draw(BorderBottomLeft, TileBoxBL2, color2);
                m_sprite.Draw(BorderBottomRight, TileBoxBR2, color2);
                m_sprite.Draw(BorderLeft, TileBoxL2, color2);
                m_sprite.Draw(BorderRight, TileBoxR2, color2);
                m_sprite.Draw(BorderTop, TileBoxT2, color2);
                m_sprite.Draw(BorderBottom, TileBoxB2, color2);
                m_sprite.Draw(BorderMiddle, TileBoxM2, color2);

                m_sprite.Draw(BorderTopLeft, TileBoxTL3, color3);
                m_sprite.Draw(BorderTopRight, TileBoxTR3, color3);
                m_sprite.Draw(BorderBottomLeft, TileBoxBL3, color3);
                m_sprite.Draw(BorderBottomRight, TileBoxBR3, color3);
                m_sprite.Draw(BorderLeft, TileBoxL3, color3);
                m_sprite.Draw(BorderRight, TileBoxR3, color3);
                m_sprite.Draw(BorderTop, TileBoxT3, color3);
                m_sprite.Draw(BorderBottom, TileBoxB3, color3);
                m_sprite.Draw(BorderMiddle, TileBoxM3, color3);

                m_sprite.Draw(BorderTopLeft, TileBoxTL4, color4);
                m_sprite.Draw(BorderTopRight, TileBoxTR4, color4);
                m_sprite.Draw(BorderBottomLeft, TileBoxBL4, color4);
                m_sprite.Draw(BorderBottomRight, TileBoxBR4, color4);
                m_sprite.Draw(BorderLeft, TileBoxL4, color4);
                m_sprite.Draw(BorderRight, TileBoxR4, color4);
                m_sprite.Draw(BorderTop, TileBoxT4, color4);
                m_sprite.Draw(BorderBottom, TileBoxB4, color4);
                m_sprite.Draw(BorderMiddle, TileBoxM4, color4);

                m_sprite.Draw(BorderTopLeft, TileBoxTL5, color5);
                m_sprite.Draw(BorderTopRight, TileBoxTR5, color5);
                m_sprite.Draw(BorderBottomLeft, TileBoxBL5, color5);
                m_sprite.Draw(BorderBottomRight, TileBoxBR5, color5);
                m_sprite.Draw(BorderLeft, TileBoxL5, color5);
                m_sprite.Draw(BorderRight, TileBoxR5, color5);
                m_sprite.Draw(BorderTop, TileBoxT5, color5);
                m_sprite.Draw(BorderBottom, TileBoxB5, color5);
                m_sprite.Draw(BorderMiddle, TileBoxM5, color5);

                m_sprite.Draw(BorderTopLeft, TileBoxTL6, color6);
                m_sprite.Draw(BorderTopRight, TileBoxTR6, color6);
                m_sprite.Draw(BorderBottomLeft, TileBoxBL6, color6);
                m_sprite.Draw(BorderBottomRight, TileBoxBR6, color6);
                m_sprite.Draw(BorderLeft, TileBoxL6, color6);
                m_sprite.Draw(BorderRight, TileBoxR6, color6);
                m_sprite.Draw(BorderTop, TileBoxT6, color6);
                m_sprite.Draw(BorderBottom, TileBoxB6, color6);
                m_sprite.Draw(BorderMiddle, TileBoxM6, color6);

                m_sprite.Draw(BorderTopLeft, TileBoxTL7, color7);
                m_sprite.Draw(BorderTopRight, TileBoxTR7, color7);
                m_sprite.Draw(BorderBottomLeft, TileBoxBL7, color7);
                m_sprite.Draw(BorderBottomRight, TileBoxBR7, color7);
                m_sprite.Draw(BorderLeft, TileBoxL7, color7);
                m_sprite.Draw(BorderRight, TileBoxR7, color7);
                m_sprite.Draw(BorderTop, TileBoxT7, color7);
                m_sprite.Draw(BorderBottom, TileBoxB7, color7);
                m_sprite.Draw(BorderMiddle, TileBoxM7, color7);

                m_sprite.Draw(BorderTopLeft, TileBoxTL8, color8);
                m_sprite.Draw(BorderTopRight, TileBoxTR8, color8);
                m_sprite.Draw(BorderBottomLeft, TileBoxBL8, color8);
                m_sprite.Draw(BorderBottomRight, TileBoxBR8, color8);
                m_sprite.Draw(BorderLeft, TileBoxL8, color8);
                m_sprite.Draw(BorderRight, TileBoxR8, color8);
                m_sprite.Draw(BorderTop, TileBoxT8, color8);
                m_sprite.Draw(BorderBottom, TileBoxB8, color8);
                m_sprite.Draw(BorderMiddle, TileBoxM8, color8);

                m_sprite.Draw(BorderTopLeft, TileBoxTL9, color9);
                m_sprite.Draw(BorderTopRight, TileBoxTR9, color9);
                m_sprite.Draw(BorderBottomLeft, TileBoxBL9, color9);
                m_sprite.Draw(BorderBottomRight, TileBoxBR9, color9);
                m_sprite.Draw(BorderLeft, TileBoxL9, color9);
                m_sprite.Draw(BorderRight, TileBoxR9, color9);
                m_sprite.Draw(BorderTop, TileBoxT9, color9);
                m_sprite.Draw(BorderBottom, TileBoxB9, color9);
                m_sprite.Draw(BorderMiddle, TileBoxM9, color9);

                m_sprite.Draw(BorderTopLeft, TileBoxTL10, color10);
                m_sprite.Draw(BorderTopRight, TileBoxTR10, color10);
                m_sprite.Draw(BorderBottomLeft, TileBoxBL10, color10);
                m_sprite.Draw(BorderBottomRight, TileBoxBR10, color10);
                m_sprite.Draw(BorderLeft, TileBoxL10, color10);
                m_sprite.Draw(BorderRight, TileBoxR10, color10);
                m_sprite.Draw(BorderTop, TileBoxT10, color10);
                m_sprite.Draw(BorderBottom, TileBoxB10, color10);
                m_sprite.Draw(BorderMiddle, TileBoxM10, color10);

                m_sprite.Draw(BorderTopLeft, TileBoxTL11, color11);
                m_sprite.Draw(BorderTopRight, TileBoxTR11, color11);
                m_sprite.Draw(BorderBottomLeft, TileBoxBL11, color11);
                m_sprite.Draw(BorderBottomRight, TileBoxBR11, color11);
                m_sprite.Draw(BorderLeft, TileBoxL11, color11);
                m_sprite.Draw(BorderRight, TileBoxR11, color11);
                m_sprite.Draw(BorderTop, TileBoxT11, color11);
                m_sprite.Draw(BorderBottom, TileBoxB11, color11);
                m_sprite.Draw(BorderMiddle, TileBoxM11, color11);

                m_sprite.Draw(BorderTopLeft, TileBoxTL12, color12);
                m_sprite.Draw(BorderTopRight, TileBoxTR12, color12);
                m_sprite.Draw(BorderBottomLeft, TileBoxBL12, color12);
                m_sprite.Draw(BorderBottomRight, TileBoxBR12, color12);
                m_sprite.Draw(BorderLeft, TileBoxL12, color12);
                m_sprite.Draw(BorderRight, TileBoxR12, color12);
                m_sprite.Draw(BorderTop, TileBoxT12, color12);
                m_sprite.Draw(BorderBottom, TileBoxB12, color12);
                m_sprite.Draw(BorderMiddle, TileBoxM12, color12);
        }

        private void DrawShopBox(SpriteBatch m_sprite)
        {
            m_sprite.Draw(BorderTopLeft, ShopBoxTL, Color.White);
            m_sprite.Draw(BorderTopRight, ShopBoxTR, Color.White);
            m_sprite.Draw(BorderBottomLeft, ShopBoxBL, Color.White);
            m_sprite.Draw(BorderBottomRight, ShopBoxBR, Color.White);
            m_sprite.Draw(BorderLeft, ShopBoxL, Color.White);
            m_sprite.Draw(BorderRight, ShopBoxR, Color.White);
            m_sprite.Draw(BorderTop, ShopBoxT, Color.White);
            m_sprite.Draw(BorderBottom, ShopBoxB, Color.White);
            m_sprite.Draw(BorderMiddle, ShopBoxM, Color.White);
        }

        private void DrawBigBox(SpriteBatch m_sprite)
        {
            m_sprite.Draw(BorderTopLeft, MenuBoxTL, Color.White);
            m_sprite.Draw(BorderTopRight, MenuBoxTR, Color.White);
            m_sprite.Draw(BorderBottomLeft, MenuBoxBL, Color.White);
            m_sprite.Draw(BorderBottomRight, MenuBoxBR, Color.White);
            m_sprite.Draw(BorderLeft, MenuBoxL, Color.White);
            m_sprite.Draw(BorderRight, MenuBoxR, Color.White);
            m_sprite.Draw(BorderTop, MenuBoxT, Color.White);
            m_sprite.Draw(BorderBottom, MenuBoxB, Color.White);
            m_sprite.Draw(BorderMiddle, MenuBoxM, Color.White);
        }
    }
}
