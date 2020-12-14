using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueTerminal.ItemSetClasses
{
    public class Block
    {
        public string hideIfSummonerSpell { get; set; }
        public List<Item> items { get; set; }
        public string showIfSummonerSpell { get; set; }
        public string type { get; set; }
    }

}
