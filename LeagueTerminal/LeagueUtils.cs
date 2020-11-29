//using RiotSharp;
//using RiotSharp.Misc;
//using RiotSharp.Endpoints.MatchEndpoint;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace LeagueTerminal
//{
//    public static class LeagueUtils
//    {
//        private static RiotApi riotApi = RiotApi.GetDevelopmentInstance("RGAPI-1614cb68-eb4e-41cd-a22b-f166889b235d");
//        private static int region = (int)Region.Eune;
//        public static List<Participant> GetStatsByName(string name)
//        {
//            List<Participant> stats = new List<Participant>();

//            var summoner = riotApi.Summoner.GetSummonerByNameAsync(Region.Eune, name).Result;
//            var summId = summoner.AccountId;

//            var matchData = riotApi.Match.GetMatchListAsync(Region.Eune, summId).Result;

//            for (int i = 0; i < 5; i++)
//            {

//                var matchReference = matchData.Matches[i];
//                var match = riotApi.Match.GetMatchAsync(RiotSharp.Misc.Region.Eune, matchReference.GameId).Result;

//                // Get participant stats object of summoner (imaqtpie)
//                var particpantsId = match.ParticipantIdentities.Single(x => x.Player.AccountId == summoner.AccountId);
//                Participant participantsStats = match.Participants.Single(x => x.ParticipantId == particpantsId.ParticipantId);



//                // Do stuff with stats

//                var win = participantsStats.Stats.Winner;
//                var champname = champions.Single(x => x.Id == participantsStats.ChampionId).Name;
//                var k = participantsStats.Stats.Kills;
//                var d = participantsStats.Stats.Deaths;
//                var a = participantsStats.Stats.Assists;
//                var kda = (k + a) / (float)d;

//                // Print #, win/loss, champion.
//                Console.WriteLine("{0,3}) {1,-4} ({2})", i + 1, win ? "Win" : "Loss", champname);
//                // Print champion, K/D/A
//                Console.WriteLine("     K/D/A {0}/{1}/{2} ({3:0.00})", k, d, a, kda);
//            }
//        }
//    }
//}
