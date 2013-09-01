using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Candyland
{
    class TeleportFairyDialog : DialogListeningScreen
    {
        UpdateInfo m_updateInfo;
        Vector3 fairyPosition;
        CandyHelper candyHelper;

        public TeleportFairyDialog(CandyHelper helper, UpdateInfo updateInfo, Vector3 fairyPos, string picture = "Images/DialogImages/DefaultImage")
        {
            Text = "Wenn du möchtest, kann ich dir deinen kleinen Freund bringen";
            Picture = picture;

            m_updateInfo = updateInfo;
            fairyPosition = fairyPos;
            candyHelper = helper;
        }

        public override void Open(Game game)
        {
            base.Open(game);
        }

        public override void Update(GameTime gameTime)
        {
            bool enterPressed = ScreenManager.Input.Equals(InputState.Continue);

            // Check if text needs scrolling
            if (scrollIndex < (TextArray.Length - 1) && TextArray[scrollIndex + 1] != null)
            {
                canScroll = true;

                // If other person is still talking continue through text by pressing Enter
                if (enterPressed)
                {
                    scrollIndex++;
                }

                timePastSinceLastArrowBling += gameTime.ElapsedGameTime.Milliseconds;

                if (timePastSinceLastArrowBling > 400)
                {
                    if (timePastSinceLastArrowBling > 800)
                    {
                        arrowBlink = false;
                        timePastSinceLastArrowBling = 0;
                    }
                    else
                        arrowBlink = true;
                }
            }
            else
            {
                canScroll = false;
                if (enterPressed)
                {
                    ScreenManager.RemoveScreen(this);
                    m_updateInfo.m_screenManager.ActivateNewScreen(new GetHelperQuestion(candyHelper, m_updateInfo, fairyPosition));
                }
            }
        }

    }
}
