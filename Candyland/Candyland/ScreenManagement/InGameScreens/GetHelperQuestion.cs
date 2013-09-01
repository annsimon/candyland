using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Candyland
{
    class GetHelperQuestion : YesNoScreen
    {
        UpdateInfo m_updateInfo;
        Vector3 fairyPosition;
        CandyHelper candyHelper;

        public GetHelperQuestion(CandyHelper helper, UpdateInfo updateInfo, Vector3 fairyPos)
        {
            m_updateInfo = updateInfo;
            fairyPosition = fairyPosition;
            candyHelper = helper;
        }

        public override void Open(Game game)
        {
            base.Open(game);

            question = "Soll ich deinen kleinen\nFreund für dich holen?";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // teleport helper to fairy in  candy guy's current level
            if (enterPressed && answer)
            {
                m_updateInfo.currenthelperAreaID = m_updateInfo.currentguyAreaID;
                m_updateInfo.currenthelperLevelID = m_updateInfo.currentguyLevelID;
                candyHelper.setPosition(fairyPosition);

                ScreenManager.ResumeLast(this);
            }
        }
    }
}
