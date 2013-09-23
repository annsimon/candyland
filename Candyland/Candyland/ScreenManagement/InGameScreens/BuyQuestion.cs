using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Candyland
{
    class BuyQuestion : YesNoScreen
    {
        private BonusTile shopItem;
        private List<BonusTile> forSale;

        public BuyQuestion(BonusTile bonus, List<BonusTile> saleStuff)
        {
            shopItem = bonus;
            forSale = saleStuff;
            question = bonus.Name + " Kaufen?";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (enterPressed && answer)
            {
                ScreenManager.SceneManager.getBonusTracker().AddSoldItem(shopItem.ID);
                forSale.RemoveAt(forSale.IndexOf(shopItem));
                ScreenManager.SceneManager.getBonusTracker().chocoChipsSpent += shopItem.Price;
                ScreenManager.ResumeLast(this);
            }
        }
    }
}
