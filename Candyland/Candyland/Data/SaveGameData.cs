using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Candyland
{
    public struct SaveGameData
    {
        public string guycurrentLevelID;

        public int chocoCount;

        public Dictionary<string, bool> actionActorVisibility;

        public Dictionary<string, bool> chocoChipState;

        public Dictionary<string, bool> actionState;
    }
}
