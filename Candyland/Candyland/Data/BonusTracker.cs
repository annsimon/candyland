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
        // chocoChipState is a dictionary that saves if a ChocoChip has
        // been collected by the player for each ChocoChip by id (string) as bool
        public Dictionary<string, bool> chocoChipState {get; set;}
        // chocoCount saves the number of ChocoChips the player collected
        public int chocoCount { get; set; }
        // chocoTotal saves the number of ChocoChips in the scene
        public int chocoTotal { get; set; }

        public BonusTracker()
        {
            chocoChipState = new Dictionary<string, bool>();
            chocoCount = 0;
            chocoTotal = 0;
        }
    }
}
