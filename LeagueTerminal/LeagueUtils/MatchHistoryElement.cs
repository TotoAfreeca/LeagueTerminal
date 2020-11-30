using RiotSharp.Endpoints.MatchEndpoint;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueTerminal.LeagueUtils
{
    public class MatchHistoryElement
    {
        public Participant GameParticipant { get; set; }

        public MatchHistoryElement(Participant participant)
        {
            GameParticipant = participant;
        }

        public override string ToString()
        {
            var winner = GameParticipant.Stats.Winner;
            var k = GameParticipant.Stats.Kills;
            var d = GameParticipant.Stats.Deaths;
            var a = GameParticipant.Stats.Assists;
            var kda = d == 0 ? 0 : (k + a) / (float)d;

            return string.Format("K/D/A {0}/{1}/{2} ({3:0.00})", k, d, a, kda);

        }
    }
}
