using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Candyland
{
    /// <summary>
    /// This class tracks the bonus items/abilities that can be collected/obtained during the game.
    /// It is saved.
    /// </summary>
    public class BonusTracker
    {
        /// <summary>
        /// chocoChipState is a dictionary that saves if a ChocoChip has
        /// been collected by the player for each ChocoChip by id (string) as bool
        /// </summary>
        public Dictionary<string, bool> chocoChipState {get; set;}
        /// <summary>
        /// chocoCount saves the number of ChocoChips the player collected
        /// </summary>
        public int chocoCount { get; set; }
        /// <summary>
        /// chocoTotal saves the number of ChocoChips in the scene
        /// </summary>
        public int chocoTotal { get; set; }
        /// <summary>
        /// how many of the collected chips where spent in the shop
        /// </summary>
        public int chocoChipsSpent { get; set; }

        public List<string> soldItems { get; set; }
        public List<BonusTile> conceptArts { get; set; }
        public List<BonusTile> bonusLevel { get; set; }

        public BonusTracker()
        {
            chocoChipState = new Dictionary<string, bool>();
            chocoCount = 0;
            chocoTotal = 0;

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
