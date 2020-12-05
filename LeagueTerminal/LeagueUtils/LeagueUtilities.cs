using RiotSharp;
using RiotSharp.Endpoints.ChampionMasteryEndpoint;
using RiotSharp.Endpoints.StaticDataEndpoint.Champion;
using RiotSharp.Endpoints.SummonerEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeagueTerminal.LeagueUtils
{
    public static class LeagueUtilities
    {

        public static string[] GetTopPlayedChampions(RiotApi api, Summoner summoner, Dictionary<string, ChampionStatic> champions )
        {
            List<ChampionMastery> championMasteries;
            try
            {
                championMasteries = api.ChampionMastery.GetChampionMasteriesAsync(summoner.Region, summoner.Id).Result;
            }
            catch (RiotSharpException ex)
            {
                // Handle the exception however you want.
                return null;
            }
            string[] masteries = new string[3]; 
            for (int i = 0; i < 3; i++)
            {
                var championMastery = championMasteries[i];    
                var id = championMastery.ChampionId;
                var name = champions.Values.Single(x => x.Id == id).Name;
                var level = championMastery.ChampionLevel;
                var points = championMastery.ChampionPoints;

                masteries[i] = $" •  **Level {level} {name}** {points} Points";

            }

            return masteries;
        }
    }
}
