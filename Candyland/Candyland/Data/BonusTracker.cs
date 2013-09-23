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
        /// <summary>
        /// list of IDs from all sold bonus items
        /// </summary>
        public List<string> soldItems { get; set; }
        /// <summary>
        /// list of all concept art items
        /// </summary>
        public List<BonusTile> conceptArts { get; set; }
        /// <summary>
        /// list of all bonus level items
        /// </summary>
        public List<BonusTile> bonusLevel { get; set; }

        public BonusTracker()
        {
            chocoChipState = new Dictionary<string, bool>();
            chocoCount = 0;
            chocoTotal = 0;

            soldItems = new List<string>(30);
            conceptArts = new List<BonusTile>(12);
            bonusLevel = new List<BonusTile>(15);

            conceptArts.Add(new BonusTile("CA1", "Acaguy", "Concept Art", 2, "aca"));
            conceptArts.Add(new BonusTile("CA2", "Bonbon", "Concept Art", 2, "bonbon"));
            conceptArts.Add(new BonusTile("CA3", "Händler", "Concept Art", 2, "salesman"));
            conceptArts.Add(new BonusTile("CA4", "Plattform", "Concept Art", 2, "platform"));
            conceptArts.Add(new BonusTile("CA5", "Schalter", "Concept Art", 2, "switch"));
            conceptArts.Add(new BonusTile("CA6", "Begleiter", "Concept Art", 3, "helper"));
            conceptArts.Add(new BonusTile("CA7", "Held", "Concept Art", 3, "player"));
            conceptArts.Add(new BonusTile("CA8", "Lakritz", "Concept Art", 3, "lakritz"));
            conceptArts.Add(new BonusTile("CA9", "Cupcakes 1", "Concept Art", 10, "cupcake1"));
            conceptArts.Add(new BonusTile("CA10", "Cupcakes 2", "Concept Art", 15, "cupcake2"));
            conceptArts.Add(new BonusTile("CA11", "Cupcakes 3", "Concept Art", 20, "cupcake3"));
            conceptArts.Add(new BonusTile("CA12", "Cupcakes 4", "Concept Art", 25, "cupcake4"));
        }

        public void AddSoldItem(string id)
        {
            soldItems.Add(id);
        }
    }
}
