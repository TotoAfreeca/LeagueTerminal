using System;
using RiotSharp;
using RiotSharp.Misc;
using System.Linq;
using Terminal.Gui;
using System.Collections.Generic;

namespace LeagueTerminal
{
    class Program
    {
        static void Main(string[] args)
        {



            Application.Init();
            var top = Application.Top;

            // Creates the top-level window to show
            var win = new Window("MyApp")
            {
                X = 0,
                Y = 1, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            top.Add(win);

            // Creates a menubar, the item "New" has a help menu.
            var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_New", "Creates new file", null),
                new MenuItem ("_Close", "", null),
                new MenuItem ("_Quit", "", null)
            }),
            new MenuBarItem ("_Edit", new MenuItem [] {
                new MenuItem ("_Copy", "", null),
                new MenuItem ("C_ut", "", null),
                new MenuItem ("_Paste", "", null)
            })
        });
            top.Add(menu);



            bool okpressed = false;
            var ok = new Button(3, 6, "OK");
            ok.Clicked += () =>
            {
                Application.RequestStop();
                okpressed = true;
            };
            var cancel = new Button(10, 3, "Cancel");
            cancel.Clicked += () =>
            {
                Application.RequestStop();
            };

            var dialog = new Dialog("Login", 60, 10, ok, cancel);

            var entry = new TextField()
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill(),
                Height = 1
            };

            dialog.Add(entry);
            Application.Run(dialog);
            if (okpressed)
                Console.WriteLine(entry.Text);



            var api = RiotApi.GetDevelopmentInstance("RGAPI-eae505c3-091c-431c-9266-198f5e8dd95d");

            var allVersion = api.DataDragon.Versions.GetAllAsync().Result;
            var latestVersion = allVersion[0]; // Example of version: "10.23.1"
            var champions = api.DataDragon.Champions.GetAllAsync(latestVersion).Result.Champions.Values;


            //Console.WriteLine($"Match history for Truskawka");


            var summoner = api.Summoner.GetSummonerByNameAsync(Region.Eune, "Truskawka").Result;
            var summId = summoner.AccountId;

            var matchData = api.Match.GetMatchListAsync(Region.Eune, summId).Result;
            List<string> list = new List<string>();

            for (int i = 0; i < 5; i++)
            {

                var matchReference = matchData.Matches[i];
                var match = api.Match.GetMatchAsync(RiotSharp.Misc.Region.Eune, matchReference.GameId).Result;

                // Get participant stats object of summoner (imaqtpie)
                var particpantsId = match.ParticipantIdentities.Single(x => x.Player.AccountId == summoner.AccountId);
                var participantsStats = match.Participants.Single(x => x.ParticipantId == particpantsId.ParticipantId);

                // Do stuff with stats

                var winner = participantsStats.Stats.Winner;
                var champname = champions.Single(x => x.Id == participantsStats.ChampionId).Name;
                var k = participantsStats.Stats.Kills;
                var d = participantsStats.Stats.Deaths;
                var a = participantsStats.Stats.Assists;
                var kda = (k + a) / (float)d;

                // Print #, win/loss, champion.
                //Console.WriteLine("{0,3}) {1,-4} ({2})", i + 1, winner ? "Win" : "Loss", champname);
                // Print champion, K/D/A
                string el = string.Format("K/D/A {0}/{1}/{2} ({3:0.00})", k, d, a, kda);
                list.Add(el);
            }
            ListView listView = new ListView(list)
            {
                X = 0,
                Y = 0,
                Width = 60,
                Height = 20
            };

            win.Add(listView);
            Application.Run();
        }
    }
}
