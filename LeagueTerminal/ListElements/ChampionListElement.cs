using RiotSharp.Endpoints.StaticDataEndpoint.Champion;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueTerminal.LeagueUtils
{
    public class ChampionListElement
    {
        public ChampionStatic Champion { get; set; }
        public string ChampionName { get; set; }

        public ChampionListElement(ChampionStatic champion)
        {
            Champion = champion;
            ChampionName = champion.Name;
        }

        public override string ToString()
        {
            return ChampionName;
        }
    }
}
