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

        // used to return to maingame istead of just going back into the dialog menu
        private GameScreen lastScreen;

        public TravelScreen(string saleID, UpdateInfo info, GameScreen dialogScreen)
        {
            salesmanID = saleID;
            m_updateInfo = info;
            lastScreen = dialogScreen;
        }

        public override void Open(Game game)
        {
            this.isFullscreen = true;

            background = ScreenManager.Content.Load<Texture2D>("ScreenTextures/travelScreen");
            teleportSpot = ScreenManager.Content.Load<Texture2D>("Images/Map/AvailablePos");
            currentSpot = ScreenManager.Content.Load<Texture2D>("Images/Map/CurrentPos");
            selectedSpot = ScreenManager.Content.Load<Texture2D>("Images/Map/SelectedPos");

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
                    case "7.0": teleportPositions[index] = (new Vector2(60, 100)); break; //manually set position of map
                }
                index++;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.Input.Equals(InputState.Back))
            {
                ScreenManager.ResumeLast(this);
            }

            bool enterPressed = false;

            // look at input and update button selection
            switch (ScreenManager.Input)
            {
                case InputState.Continue: enterPressed = true; break;
                case InputState.Left: activeIndex--; break;
                case InputState.Right: activeIndex++; break;
            }
            if (activeIndex == currentSpotIndex) activeIndex++; // current spot can't be selected for travelling
            if (activeIndex >= numOfTeleportOptions) activeIndex = 0;
            if (activeIndex < 0) activeIndex = numOfTeleportOptions - 1;

            // Open Question
            if (enterPressed && (activeIndex != currentSpotIndex))
            {
                ScreenManager.ActivateNewScreen(new TravelQuestion(m_updateInfo, lastScreen, this, activeIndex));
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
                    m_sprite.Draw(currentSpot, new Rectangle((int)pos.X, (int)pos.Y, 20, 20), Color.White);
                else if(index == activeIndex)
                    m_sprite.Draw(selectedSpot, new Rectangle((int)pos.X, (int)pos.Y, 20, 20), Color.White);
                else
                    m_sprite.Draw(teleportSpot, new Rectangle((int)pos.X, (int)pos.Y, 20, 20), Color.White);

                index++;
            }

            m_sprite.End();
        }
    }
}
