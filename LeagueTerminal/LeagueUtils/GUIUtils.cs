using RiotSharp;
using RiotSharp.Endpoints.ChampionMasteryEndpoint;
using RiotSharp.Endpoints.LeagueEndpoint;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Endpoints.StaticDataEndpoint.Champion;
using RiotSharp.Endpoints.SummonerEndpoint;
using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminal.Gui;

namespace LeagueTerminal.LeagueUtils
{
    class GUIUtils
    {
        static private View TopView { get; set; }
        static private View SummonerSearchView { get; set; }

        static private View SummonerInfoView { get; set; }
        static private View MatchHistoryView { get; set ; }
        static private View MatchDetailsView { get; set; }



        static private RiotApi Api = RiotApi.GetDevelopmentInstance("RGAPI-37051223-8fac-4b1e-b523-73371cb41e71");

        static private string LatestVersion { get; set; }
        static private TextField UsernameTextField { get; set; }

        static private string SummonerName { get; set; }

        static private ListView MatchHistoryListView { get; set; }

        static private Region SearchRegion { get; set; }

        //var latestVersion = allVersion[0]; // Example of version: "10.23.1"
        static private Dictionary<string, ChampionStatic> Champions { get; set; }

        public static void SetupMainView(View myView)
        {
            LatestVersion = Api.DataDragon.Versions.GetAllAsync().Result[0];
            Champions = Api.DataDragon.Champions.GetAllAsync(LatestVersion).Result.Champions;

            TopView = myView;

            // Creates a menubar, the item "New" has a help menu.
            var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_New", "Creates new file", null),
                new MenuItem ("_Close", "", null),
                new MenuItem ("_Quit", "", null)
            }),
            new MenuBarItem ("Server", new MenuItem [] {
                new MenuItem ("EUNE", "", () => { SetSearchRegion(Region.Eune); }),
                new MenuItem ("EUW", "", () => { SetSearchRegion(Region.Euw); }),
                new MenuItem ("NA", "", () => { SetSearchRegion(Region.Na); }),
                new MenuItem ("KR", "", () => { SetSearchRegion(Region.Kr); })
            })
        });
            SetSearchRegion(Region.Eune);
            myView.Add(menu);



            SetupMatchHistoryView();
            SetupSummonerInfo();
            SetupMatchInfo();
            SetupSummonerSearchView();
            myView.Add(SummonerSearchView);
            myView.Add(SummonerInfoView);
            myView.Add(MatchHistoryView);
            myView.Add(MatchDetailsView);
        }

        private static void SetSearchRegion(Region searchRegion)
        {
            SearchRegion = searchRegion;
        }

        private static View SetupMenuBar()
        {
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
            return menu;
        }

        private static void SetupSummonerSearchView()
        {
            var summonerSearchInfoWindow = new FrameView("Find summoner")
            {
                X = Pos.Percent(0),
                Y = 1, // Leave place for top level menu and Summoner search

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Percent(10) + 1
            };

            UsernameTextField = new TextField("")
            {
                X = Pos.Center(),
                Y = Pos.Percent(3),
                Width = 30,
                Height = 1
            };
            summonerSearchInfoWindow.Add(UsernameTextField);

            var searchSummoner = new Button("Search")
            {
                X = Pos.Center() + 30,
                Y = Pos.Percent(3)
            };
            searchSummoner.Clicked += SearchButtonClickedHandler;

            summonerSearchInfoWindow.Add(searchSummoner);
            SummonerSearchView = summonerSearchInfoWindow;
        }
        private static void SetupSummonerInfo()
        {
            var summonerInfoWindow = new FrameView("Summoner Information")
            {
                X = Pos.Percent(0),
                Y = Pos.Percent(10) + 1, // Leave place for top level menu and Summoner search

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Percent(35),
                Height = Dim.Fill()
            };
            SummonerInfoView = summonerInfoWindow;
        }
        private static void SetupMatchHistoryView()
        {
            var matchHistoryWindow = new FrameView("Match History")
            {
                X = Pos.Percent(35),
                Y = Pos.Percent(10) + 1, // Leave place for top level menu and Summoner search

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Percent(25),
                Height = Dim.Fill()
            };
            MatchHistoryListView = new ListView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            matchHistoryWindow.Add(MatchHistoryListView);
            MatchHistoryView = matchHistoryWindow;
        }




        private static void SetupMatchInfo()
        {
            var matchInfoView = new FrameView("Match information")
            {
                X = Pos.Percent(60),
                Y = Pos.Percent(10) + 1, // Leave place for top level menu and Summoner search

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            MatchDetailsView = matchInfoView;
        }

        private static void SearchButtonClickedHandler()
        {

            Summoner summoner = null;
            try
            {
                summoner = Api.Summoner.GetSummonerByNameAsync(SearchRegion, UsernameTextField.Text.ToString()).Result;
            }
            catch (Exception e)
            {
                DrawInfoDialog("There is no summoner " + UsernameTextField.Text.ToString()+ " in selected region");
                return;
            }
            
            var summId = summoner.AccountId;

            var summonerLeagues = Api.League.GetLeagueEntriesBySummonerAsync(SearchRegion, summoner.Id).Result;
            MatchList matchData = null;
            try
            {
                matchData = Api.Match.GetMatchListAsync(SearchRegion, summId).Result;
            }
            catch(RiotSharpException e)
            {
                DrawInfoDialog("This user hasn't play any games this season.");
                return;
            }
                List<MatchHistoryElement> list = new List<MatchHistoryElement>();

            Dictionary<Participant, string> dic = new Dictionary<Participant, string>();
            for (int i = 0; i < 10; i++)
            {

                var matchReference = matchData.Matches[i];
                var match = Api.Match.GetMatchAsync(SearchRegion, matchReference.GameId).Result;

                // Get participant stats object of summoner (imaqtpie)
                var particpantsId = match.ParticipantIdentities.Single(x => x.Player.AccountId == summoner.AccountId);
                var participant = match.Participants.Single(x => x.ParticipantId == particpantsId.ParticipantId);

                // Do stuff with stats

                list.Add(new MatchHistoryElement(participant));
            }

            //List<ChampionMastery> championMasteries;
            //try
            //{
            //    championMasteries = Api.ChampionMastery.GetChampionMasteriesAsync(SearchRegion, summoner.Id).Result.Where(x =>;
            //}
            //catch (RiotSharpException ex)
            //{
            //    // Handle the exception however you want.
            //    return;
            //}

            RedrawUserInfo(summoner, summonerLeagues);
            RedrawMatchHistory(list);
        }


        private static void RedrawUserInfo(Summoner summoner, List<LeagueEntry> summonerLeagues)
        {
            var label = new Label("Summoner Name: " + summoner.Name)
            {
                X = Pos.Percent(0),
                Y = Pos.Percent(0),
                Width = Dim.Fill(),
                Height = 1
            };
            SummonerInfoView.Add(label);

            var levelLabel = new Label("Summoner level: " + summoner.Level)
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(label),
                Width = Dim.Fill(),
                Height = 1
            };
            SummonerInfoView.Add(levelLabel);


            var emptylabel = new Label(" ")
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(levelLabel),
                Width = Dim.Fill(),
                Height = 1
            };
            SummonerInfoView.Add(emptylabel);

            var queue = new Label("Queue")
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(emptylabel),
                Width = Dim.Fill(),
                Height = 1
            };
            SummonerInfoView.Add(queue);

            var prev = queue;
            foreach (LeagueEntry entry in summonerLeagues)
            {


                var queueType = new Label(string.Format("{0,-15} | {1,-15} | {2,-15}", entry.QueueType, (entry.Tier + " " + entry.Rank), (entry.Wins + "//" + entry.Losses)))
                {
                    X = Pos.Percent(0),
                    Y = prev.Y +1,
                    Width = Dim.Fill(),
                    Height = 1
                };
                prev = queueType;
                SummonerInfoView.Add(queueType);
                
            }
        }

        private static void RedrawMatchHistory(List<MatchHistoryElement> matchHistoryElements)
        {
            MatchHistoryListView.SetSource(matchHistoryElements);
        }

        private static void DrawInfoDialog(string message)
        {

            var ok = new Button("Ok");
            var entry = new TextField()
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill(),
                Height = 1
            };
            ok.Clicked += () => { Application.RequestStop(); };
            entry.Text = message;
            var dialog = new Dialog("Error", 60, 7, ok);
            dialog.Add(entry);
            Application.Run(dialog);
        }
    }
}
