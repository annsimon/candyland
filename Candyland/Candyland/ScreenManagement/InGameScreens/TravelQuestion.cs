using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Candyland
{
    class TravelQuestion : YesNoScreen
    {
        UpdateInfo m_updateInfo;
        GameScreen dialogScreen;
        GameScreen travelScreen;
        int selectedTeleportIndex;

        public TravelQuestion(UpdateInfo updateInfo, GameScreen dialog, GameScreen travel, int teleportIndex)
        {
            m_updateInfo = updateInfo;
            dialogScreen = dialog;
            travelScreen = travel;
            selectedTeleportIndex = teleportIndex;
        }

        public override void Open(Game game, AssetManager assets)
        {
            base.Open(game, assets);

            question = "Per Teleport Reisen?";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // teleport to selected level, if not already there
            if (enterPressed && answer)
            {
                m_updateInfo.currentguyLevelID = m_updateInfo.activeTeleports.ElementAt(selectedTeleportIndex);
                int cutOff = m_updateInfo.activeTeleports.ElementAt(selectedTeleportIndex).LastIndexOf('.');
                m_updateInfo.currentguyAreaID = m_updateInfo.activeTeleports.ElementAt(selectedTeleportIndex).Substring(0, cutOff);

                m_updateInfo.reset = true;
                ScreenManager.RemoveScreen(dialogScreen);
                ScreenManager.RemoveScreen(travelScreen);
                ScreenManager.ResumeLast(this);
            }
        }
    }
}
