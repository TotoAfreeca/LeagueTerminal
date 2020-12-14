using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueTerminal.ItemSetClasses
{
    public class Root
    {
        public string title { get; set; }
        public List<int> associatedMaps { get; set; }
        public List<int> associatedChampions { get; set; }
        public List<Block> blocks { get; set; }
    }
}
