using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class SlideControl
    {
        public bool selected { get; set; }
        public bool active { get; set; }
        public int maxValue;
        public int value;
        public string comment = "";
        private SpriteFont font;
        public int stepSize;

        public Vector2 position { get; set; }
        private Rectangle knob;
        private Color color;

        private Texture2D slideTexture;
        private Texture2D slider;
        private Texture2D sliderSelected;
        private Texture2D arrows;

        public SlideControl(int max, int oldValue, Vector2 pos, AssetManager assets)
        {
            maxValue = max;
            value = oldValue;
            stepSize = 250 / max;
            position = pos;
            font = assets.mainText;
            slideTexture = assets.slideControl;
            slider = assets.slider;
            sliderSelected = assets.sliderSelected;
            arrows = assets.sliderArrows;
            knob = new Rectangle(0, 0, 100, 50);
        }

        public void Draw(SpriteBatch m_sprite)
        {
            // keep value in correct scope
            if (value < 0) value = 0;
            if (value > maxValue) value = maxValue;

            // move slider
            knob.Location = new Point ((int)position.X + value * stepSize - 10, (int) position.Y + 50);

            if (selected) color = Color.GreenYellow;
            else color = Color.White;

            m_sprite.Draw(slideTexture, new Rectangle((int)position.X, (int)position.Y, slideTexture.Width, slideTexture.Height), Color.White);
            if (selected) m_sprite.Draw(sliderSelected, knob, Color.White);
            else m_sprite.Draw(slider, knob, Color.White);
            if (active) m_sprite.Draw(arrows, new Rectangle((int)position.X + 70, (int)position.Y + 50, 200, 100), Color.White);
            m_sprite.DrawString(font, comment, new Vector2((int)position.X + 167 - (int)font.MeasureString(comment).X / 2, (int)position.Y + 90), Color.Black);
            selected = false;
        }

    }
}
