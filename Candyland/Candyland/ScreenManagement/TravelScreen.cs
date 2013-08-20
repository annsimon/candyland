using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class TravelScreen : GameScreen
    {
        Texture2D background;
        Texture2D teleportSpot;
        Texture2D currentSpot;
        Texture2D selectedSpot;

        private string salesmanID;
        private UpdateInfo m_updateInfo;

        private int numOfTeleportOptions;
        private int activeIndex = 0;
        private int currentSpotIndex;
        private Vector2[] teleportPositions;

        public TravelScreen(string saleID, UpdateInfo info)
        {
            salesmanID = saleID;
            m_updateInfo = info;
        }

        public override void Open(Game game)
        {
            this.isFullscreen = true;

            background = ScreenManager.Content.Load<Texture2D>("ScreenTextures/travelScreen");
            teleportSpot = ScreenManager.Content.Load<Texture2D>("ScreenTextures/testScreen");
            currentSpot = ScreenManager.Content.Load<Texture2D>("ScreenTextures/testBonus");
            selectedSpot = ScreenManager.Content.Load<Texture2D>("ScreenTextures/creditScreen");

            numOfTeleportOptions = m_updateInfo.activeTeleports.Count;

            // fill list with active teleport spots
            int index = 0;
            teleportPositions = new Vector2[numOfTeleportOptions];
            foreach (string id in m_updateInfo.activeTeleports)
            {
                if (salesmanID == id)
                    currentSpotIndex = index;
                switch (id)
                {
                    case "7.0": teleportPositions[index] = (new Vector2(30, 50)); break; //manually set position of map
                }
                index++;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.Input.Equals(InputState.Enter))
            {
                ScreenManager.ResumeLast(this);
            }

            bool enterPressed = false;

            // look at input and update button selection
            switch (ScreenManager.Input)
            {
                case InputState.Enter: enterPressed = true; break;
                case InputState.Left: activeIndex--; break;
                case InputState.Right: activeIndex++; break;
            }
            if (activeIndex >= numOfTeleportOptions) activeIndex = 0;
            if (activeIndex < 0) activeIndex = numOfTeleportOptions - 1;

            // teleport to selected level
            if (enterPressed)
            {
                m_updateInfo.currentLevelID = m_updateInfo.activeTeleports.ElementAt(activeIndex);
                m_updateInfo.currentAreaID = m_updateInfo.activeTeleports.ElementAt(activeIndex).Substring(0, 1);
                m_updateInfo.reset = true;
                ScreenManager.ResumeLast(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            int screenWidth = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            int screenHeight = ScreenManager.Game.GraphicsDevice.Viewport.Height;

            SpriteBatch m_sprite = ScreenManager.SpriteBatch;

            m_sprite.Begin();
            m_sprite.Draw(background, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

            // draw activated teleport positions on map
            int index = 0;
            foreach (Vector2 pos in teleportPositions)
            {
                if(index == currentSpotIndex)
                    m_sprite.Draw(currentSpot, new Rectangle((int)pos.X, (int)pos.Y, 10, 10), Color.White);
                else if(index == activeIndex)
                    m_sprite.Draw(selectedSpot, new Rectangle((int)pos.X, (int)pos.Y, 10, 10), Color.White);
                else
                    m_sprite.Draw(teleportSpot, new Rectangle((int)pos.X, (int)pos.Y, 10, 10), Color.White);

                index++;
            }

            m_sprite.End();
        }
    }
}
