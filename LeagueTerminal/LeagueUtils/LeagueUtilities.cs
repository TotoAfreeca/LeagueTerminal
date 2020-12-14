using RiotSharp;
using RiotSharp.Endpoints.ChampionMasteryEndpoint;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Endpoints.StaticDataEndpoint.Champion;
using RiotSharp.Endpoints.StaticDataEndpoint.Item;
using RiotSharp.Endpoints.SummonerEndpoint;
using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeagueTerminal.LeagueUtils
{
    public static class LeagueUtilities
    {

        static public RiotApi Api = RiotApi.GetDevelopmentInstance("RGAPI-0cc43c67-4700-4ae7-8313-7ff40260f101");
        static private string LatestVersion { get; set; }
        static public Dictionary<string, ChampionStatic> Champions { get; set; }

        static public List<string> ItemTags { get; set; }
        static public Dictionary<int, ItemStatic> Items { get; set; }
        public static void SetupApi()
        {

            LatestVersion = Api.DataDragon.Versions.GetAllAsync().Result[0];
            Champions = Api.DataDragon.Champions.GetAllAsync(LatestVersion).Result.Champions;
            Items = Api.DataDragon.Items.GetAllAsync(LatestVersion).Result.Items;
            ItemTags = GetItemTags();

            //Armor
            //Damage
            //SpellDamage
            //AttackSpeed
        }

        public static string[] GetTopPlayedChampions(Summoner summoner)
        {
            List<ChampionMastery> championMasteries;
            try
            {
                championMasteries = Api.ChampionMastery.GetChampionMasteriesAsync(summoner.Region, summoner.Id).Result;
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
                var name = Champions.Values.Single(x => x.Id == id).Name;
                var level = championMastery.ChampionLevel;
                var points = championMastery.ChampionPoints;

                masteries[i] = $" •  Level {level} {name} {points} Points";

            }

            return masteries;
        }

        public static List<MatchHistoryElement> GetMatchHistoryList(RiotApi api, Region searchRegion, Summoner playerSummoner, Dictionary<string, ChampionStatic> champions)
        {
            
            MatchList matchData = null;
            try
            {
                matchData = api.Match.GetMatchListAsync(searchRegion, playerSummoner.AccountId).Result;
            }
            catch (RiotSharpException e)
            {
                throw e;
            }

            List<MatchHistoryElement> list = new List<MatchHistoryElement>();

            foreach (var matchReference in matchData.Matches)
                list.Add(new MatchHistoryElement(matchReference, champions));

            return list;
        }

        public static List<ChampionListElement> GetChampionListElements()
        {
            List<ChampionListElement> championListElements = new List<ChampionListElement>();

            foreach(ChampionStatic champion in Champions.Values)
                championListElements.Add(new ChampionListElement(champion));

            return championListElements;
        }
        public static List<ItemListElement> GetItemListElements()
        {
            List<ItemListElement> itemListElements = new List<ItemListElement>();

            foreach (ItemStatic item in Items.Values)
                itemListElements.Add(new ItemListElement(item));

            return itemListElements;
        }

        public static List<ItemListElement> GetItemListByTag(string tag)
        {
            List<ItemListElement> itemListElements = new List<ItemListElement>();

            foreach (ItemStatic item in Items.Values)
                if(item.Tags.Contains(tag.Trim()))
                    itemListElements.Add(new ItemListElement(item));

            return itemListElements;
        }

        public static List<string> GetItemTags()
        {
            var temTags = new List<string>();
            foreach (var item in Items.Values)
            {
                foreach (string tag in item.Tags)
                    if (!temTags.Contains(tag))
                        temTags.Add(tag);
            }
            return temTags;
        }
    }
}
