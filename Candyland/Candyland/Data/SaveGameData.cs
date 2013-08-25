using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Candyland
{
    public struct SaveGameData
    {
        public string guycurrentAreaID;
        public string guycurrentLevelID;
        public string helpercurrentAreaID;
        public string helpercurrentLevelID;

        public Dictionary<string, bool> chocoChipState;
        public int chocoCount;
        public int chocoTotal;
    }
}
