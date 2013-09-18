using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Candyland
{
    public struct SaveGameData
    {
        public bool helperIsAvailable;
        public bool selectedPlayer;

        public string guycurrentLevelID;
        public string helpercurrentLevelID;

        public List<string> activatedTeleports;

        public int chocoCount;
        public int chocoSpend;
        public Dictionary<string, bool> chocoChipState;
        public List<string> soldItems;

        public Dictionary<string, bool> actionState;
    }
}
