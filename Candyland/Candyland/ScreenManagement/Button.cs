using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class Button
    {
        public bool selected { get; set; }
        public Vector2 position { get; set; }
        private Rectangle buttonBox;
        private string text;
        private SpriteFont font;

        private Color color;

        private Rectangle BoxTL;
        private Rectangle BoxTR;
        private Rectangle BoxBL;
        private Rectangle BoxBR;
        private Rectangle BoxL;
        private Rectangle BoxR;
        private Rectangle BoxT;
        private Rectangle BoxB;
        private Rectangle BoxM;

        private Texture2D BorderTopLeft;
        private Texture2D BorderTopRight;
        private Texture2D BorderBottomLeft;
        private Texture2D BorderBottomRight;
        private Texture2D BorderLeft;
        private Texture2D BorderRight;
        private Texture2D BorderTop;
        private Texture2D BorderBottom;
        private Texture2D BorderMiddle;

        public Button(string caption, Vector2 pos, AssetManager assets, GameScreen screen)
        {
            text = caption;
            position = pos;
            font = assets.mainText;
            buttonBox = new Rectangle((int)pos.X, (int)pos.Y, (int)font.MeasureString(text).X + 20, (int)font.MeasureString(text).Y + 20);
            BorderTopLeft = assets.dialogTL;
            BorderTopRight = assets.dialogTR;
            BorderBottomLeft = assets.dialogBL;
            BorderBottomRight = assets.dialogBR;
            BorderLeft = assets.dialogL;
            BorderRight = assets.dialogR;
            BorderTop = assets.dialogT;
            BorderBottom = assets.dialogB;
            BorderMiddle = assets.dialogC;

            screen.MakeBorderBox(buttonBox,
                out BoxTL, out BoxT, out BoxTR, out BoxR,
                out BoxBR, out BoxB, out BoxBL, out BoxL, out BoxM);
        }

        public void Draw(SpriteBatch m_sprite)
        {
            if (selected) color = Color.GreenYellow;
            else color = Color.White;

            DrawBoxBorder(m_sprite);
            Vector2 textPos = new Vector2(position.X + 10, position.Y + 10);
            m_sprite.DrawString(font, text, textPos, Color.Black);

            selected = false;
        }

        private void DrawBoxBorder(SpriteBatch m_sprite)
        {
            m_sprite.Draw(BorderTopLeft, BoxTL, color);
            m_sprite.Draw(BorderTopRight, BoxTR, color);
            m_sprite.Draw(BorderBottomLeft, BoxBL, color);
            m_sprite.Draw(BorderBottomRight, BoxBR, color);
            m_sprite.Draw(BorderLeft, BoxL, color);
            m_sprite.Draw(BorderRight, BoxR, color);
            m_sprite.Draw(BorderTop, BoxT, color);
            m_sprite.Draw(BorderBottom, BoxB, color);
            m_sprite.Draw(BorderMiddle, BoxM, color);
        }
    }
}
