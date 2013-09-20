using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class CheckBox
    {
        public bool selected { get; set; }
        public bool checkedOff { get; set; }
        public Vector2 position { get; set; }
        private Rectangle box;
        private Color color;

        private Texture2D checkMark;

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

        public CheckBox(bool checkedOff, Vector2 pos, AssetManager assets, GameScreen screen)
        {
            this.checkedOff = checkedOff;
            position = pos;
            box = new Rectangle((int)pos.X, (int)pos.Y, 60, 60);
            checkMark = assets.checkMark;

            BorderTopLeft = assets.dialogTL;
            BorderTopRight = assets.dialogTR;
            BorderBottomLeft = assets.dialogBL;
            BorderBottomRight = assets.dialogBR;
            BorderLeft = assets.dialogL;
            BorderRight = assets.dialogR;
            BorderTop = assets.dialogT;
            BorderBottom = assets.dialogB;
            BorderMiddle = assets.dialogC;

            screen.MakeBorderBox(box,
                out BoxTL, out BoxT, out BoxTR, out BoxR,
                out BoxBR, out BoxB, out BoxBL, out BoxL, out BoxM);
        }

        public void Draw(SpriteBatch m_sprite)
        {
            if (selected) color = Color.GreenYellow;
            else color = Color.White;

            DrawBoxBorder(m_sprite);
            if(checkedOff)
                m_sprite.Draw(checkMark, new Rectangle(box.Left + 10, box.Top + 10, box.Width - 20, box.Height -20), Color.White);

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
