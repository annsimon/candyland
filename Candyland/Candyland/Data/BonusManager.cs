using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Candyland
{
    public class BonusManager
    {
        public List<string> soldItems { get; set; }
        public List<BonusTile> conceptArts { get; set; }
        public List<BonusTile> bonusLevel { get; set; }

        public BonusManager()
        {
            soldItems = new List<string>(30);
            conceptArts = new List<BonusTile>(15);
            bonusLevel = new List<BonusTile>(15);

            conceptArts.Add(new BonusTile("CA1", "Cupcakes 1", "Concept Art", 1, "Images/ConceptArt/Cupcakes1"));
            conceptArts.Add(new BonusTile("CA2", "Cupcakes 2", "Concept Art", 2, "Images/ConceptArt/Cupcakes2"));
            conceptArts.Add(new BonusTile("CA3", "Cupcakes 3", "Concept Art", 3, "Images/ConceptArt/Cupcakes3"));
            conceptArts.Add(new BonusTile("CA4", "Cupcakes 4", "Concept Art", 4, "Images/ConceptArt/Cupcakes4"));
            //conceptArts.Add(new BonusTile("CA5", "Flower", "Concept Art", 5, "testBonus"));
            //conceptArts.Add(new BonusTile("CA6", "Flower", "Concept Art", 6, "testBonus"));
            //conceptArts.Add(new BonusTile("CA7", "Flower", "Concept Art", 7, "testBonus"));
            //conceptArts.Add(new BonusTile("CA8", "Flower", "Concept Art", 8, "testBonus"));
            //conceptArts.Add(new BonusTile("CA9", "Flower", "Concept Art", 9, "testBonus"));
            //conceptArts.Add(new BonusTile("CA10", "Flower", "Concept Art", 10, "testBonus"));
            //conceptArts.Add(new BonusTile("CA11", "Flower", "Concept Art", 11, "testBonus"));
            //conceptArts.Add(new BonusTile("CA12", "Flower", "Concept Art", 12, "testBonus"));
            //conceptArts.Add(new BonusTile("CA13", "Flower", "Concept Art", 13, "testBonus"));
            //conceptArts.Add(new BonusTile("CA14", "Flower", "Concept Art", 14, "testBonus"));
            //conceptArts.Add(new BonusTile("CA15", "Flower", "Concept Art", 15, "testBonus"));
        }

        public void AddSoldItem(string id)
        {
            soldItems.Add(id);
        }
    }
}
