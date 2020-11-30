using System;
using RiotSharp;
using RiotSharp.Misc;
using System.Linq;
using Terminal.Gui;
using System.Collections.Generic;
using RiotSharp.Endpoints.MatchEndpoint;
using LeagueTerminal.LeagueUtils;

namespace LeagueTerminal
{

    class Program
    {


        static void Main(string[] args)
        {
            Application.Init();
            var top = Application.Top;

        //    // Creates a menubar, the item "New" has a help menu.
        //    var menu = new MenuBar(new MenuBarItem[] {
        //    new MenuBarItem ("_File", new MenuItem [] {
        //        new MenuItem ("_New", "Creates new file", null),
        //        new MenuItem ("_Close", "", null),
        //        new MenuItem ("_Quit", "", () => { top.Running = false; })
        //    }),
        //    new MenuBarItem ("_Edit", new MenuItem [] {
        //        new MenuItem ("_Copy", "", null),
        //        new MenuItem ("C_ut", "", null),
        //        new MenuItem ("_Paste", "", null)
        //    })
        //});
        //    top.Add(menu);
           

            //bool okpressed = false;
            //var ok = new Button(3, 6, "OK");
            //ok.Clicked += () =>
            //{
            //    Application.RequestStop();
            //    okpressed = true;
            //};
            //var cancel = new Button(10, 6, "Cancel");
            //cancel.Clicked += () =>
            //{
            //    Application.RequestStop();
            //};

            //var dialog = new Dialog("Login", 60, 10, ok, cancel);

            //var entry = new TextField()
            //{
            //    X = 1,
            //    Y = 1,
            //    Width = Dim.Fill(),
            //    Height = 1
            //};

            //dialog.Add(entry);
            //Application.Run(dialog);
            //if (okpressed)
            //    Console.WriteLine(entry.Text);



            //var api = RiotApi.GetDevelopmentInstance("RGAPI-eace1ade-b6ee-48cd-b957-364dfc3b8744");

            //var allVersion = api.DataDragon.Versions.GetAllAsync().Result;
            //var latestVersion = allVersion[0]; // Example of version: "10.23.1"
            //var champions = api.DataDragon.Champions.GetAllAsync(latestVersion).Result.Champions.Values;


            ////Console.WriteLine($"Match history for Truskawka");


            //var summoner = api.Summoner.GetSummonerByNameAsync(Region.Eune, "Truskawka").Result;
            //var summId = summoner.AccountId;

            //var matchData = api.Match.GetMatchListAsync(Region.Eune, summId).Result;
            //List<MatchHistoryElement> list = new List<MatchHistoryElement>();

            //Dictionary<Participant, string> dic = new Dictionary<Participant, string>();
            //for (int i = 0; i < 5; i++)
            //{

            //    var matchReference = matchData.Matches[i];
            //    var match = api.Match.GetMatchAsync(RiotSharp.Misc.Region.Eune, matchReference.GameId).Result;

            //    // Get participant stats object of summoner (imaqtpie)
            //    var particpantsId = match.ParticipantIdentities.Single(x => x.Player.AccountId == summoner.AccountId);
            //    var participant= match.Participants.Single(x => x.ParticipantId == particpantsId.ParticipantId);

            //    // Do stuff with stats

            //    list.Add(new MatchHistoryElement(participant));
            //}
          

            GUIUtils.SetupMainView(top);

            Application.Run(top);
        }
    }
}
