using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Endpoints.StaticDataEndpoint.Champion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeagueTerminal.LeagueUtils
{
    public class MatchHistoryElement
    {
        public MatchReference Reference{ get; set; }
        public string ChampionName { get; set; }

        public MatchHistoryElement(MatchReference matchReference, Dictionary<string, ChampionStatic> champions)
        {
            Reference = matchReference;
            ChampionName = champions.Values.Single(x => x.Id == matchReference.ChampionID).Name;
        }

        public override string ToString()
        {
            var champion = ChampionName;
            var date = Reference.Timestamp.ToString();
            

            return string.Format("{0,-14} {1,-11}", champion, date , Reference.Queue);

        }
    }
}
