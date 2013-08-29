using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Candyland
{
    class BonusManager
    {
        public List<string> soldItems { get; set; }
        public List<BonusTile> conceptArts { get; set; }
        public List<BonusTile> bonusLevel { get; set; }

        BonusManager()
        {
            soldItems = new List<string>(30);
            conceptArts = new List<BonusTile>(15);
            bonusLevel = new List<BonusTile>(15);

            conceptArts.Add(new BonusTile("CA1", "Flower", "Concept Art", 1, "testBonus"));
            conceptArts.Add(new BonusTile("CA2", "Flower", "Concept Art", 2, "testBonus"));
            conceptArts.Add(new BonusTile("CA3", "Flower", "Concept Art", 3, "testBonus"));
            conceptArts.Add(new BonusTile("CA4", "Flower", "Concept Art", 4, "testBonus"));
            conceptArts.Add(new BonusTile("CA5", "Flower", "Concept Art", 5, "testBonus"));
            conceptArts.Add(new BonusTile("CA6", "Flower", "Concept Art", 6, "testBonus"));
            conceptArts.Add(new BonusTile("CA7", "Flower", "Concept Art", 7, "testBonus"));
            conceptArts.Add(new BonusTile("CA8", "Flower", "Concept Art", 8, "testBonus"));
            conceptArts.Add(new BonusTile("CA9", "Flower", "Concept Art", 9, "testBonus"));
            conceptArts.Add(new BonusTile("CA10", "Flower", "Concept Art", 10, "testBonus"));
            conceptArts.Add(new BonusTile("CA11", "Flower", "Concept Art", 11, "testBonus"));
            conceptArts.Add(new BonusTile("CA12", "Flower", "Concept Art", 12, "testBonus"));
            conceptArts.Add(new BonusTile("CA13", "Flower", "Concept Art", 13, "testBonus"));
            conceptArts.Add(new BonusTile("CA14", "Flower", "Concept Art", 14, "testBonus"));
            conceptArts.Add(new BonusTile("CA15", "Flower", "Concept Art", 15, "testBonus"));
        }

        public void AddSoldItem(string id)
        {
            soldItems.Add(id);
        }
    }
}
