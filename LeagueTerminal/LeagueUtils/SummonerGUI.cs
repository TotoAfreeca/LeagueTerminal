using RiotSharp;
using RiotSharp.Endpoints.ChampionMasteryEndpoint;
using RiotSharp.Endpoints.LeagueEndpoint;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Endpoints.StaticDataEndpoint.Champion;
using RiotSharp.Endpoints.StaticDataEndpoint.Item;
using RiotSharp.Endpoints.SummonerEndpoint;
using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminal.Gui;

namespace LeagueTerminal.LeagueUtils
{
    class SummonerGUI
    {
        static public View TopView;
        static public View TopViewWindow { get; set; }
        static private View SummonerSearchView { get; set; }

        static private View SummonerInfoView { get; set; }
        static private View MatchHistoryView { get; set ; }
        static private View MatchDetailsView { get; set; }




        static private TextField UsernameTextField { get; set; }

        
        static private Summoner PlayerSummoner { get; set; }

        static private ListView MatchHistoryListView { get; set; }

        static private Region SearchRegion { get; set; }

        //var latestVersion = allVersion[0]; // Example of version: "10.23.1"

        public static void SetupMainView(View myView)
        {

            Window summonerSearchWindow = new Window();
            TopViewWindow = summonerSearchWindow;
            TopView = myView;

            SetSearchRegion(Region.Eune);
            SearchRegion = Region.Eune;
            SetupMatchHistoryView();
            SetupSummonerInfo();
            SetupMatchInfo();
            SetupSummonerSearchView();
            TopViewWindow.Add(SummonerSearchView);
            TopViewWindow.Add(SummonerInfoView);
            TopViewWindow.Add(MatchHistoryView);
            TopViewWindow.Add(MatchDetailsView);
        }

        public static void SetSearchRegion(Region searchRegion)
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

            PlayerSummoner = null;
            try
            {
                PlayerSummoner = LeagueUtilities.Api.Summoner.GetSummonerByNameAsync(SearchRegion, UsernameTextField.Text.ToString()).Result;
            }
            catch (Exception e)
            {
                DrawInfoDialog("There is no summoner " + UsernameTextField.Text.ToString()+ " in selected region");
                return;
            }
            

            var summonerLeagues = LeagueUtilities.Api.League.GetLeagueEntriesBySummonerAsync(SearchRegion, PlayerSummoner.Id).Result;

            List<MatchHistoryElement> matchlist = null;

            try
            {
                matchlist = LeagueUtilities.GetMatchHistoryList(LeagueUtilities.Api, SearchRegion, PlayerSummoner, LeagueUtilities.Champions);
            }
            catch(Exception e)
            {
                DrawInfoDialog("This user hasn't play any games this season.");
                return;
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

            RedrawUserInfo(summonerLeagues);
            RedrawMatchHistory(matchlist);
        }


        private static void RedrawUserInfo(List<LeagueEntry> summonerLeagues)
        {
            SummonerInfoView.RemoveAll();

            var label = new Label("Summoner Name: " + PlayerSummoner.Name)
            {
                X = Pos.Percent(0),
                Y = Pos.Percent(0),
                Width = Dim.Fill(),
                Height = 1
            };
            SummonerInfoView.Add(label);

            var levelLabel = new Label("Summoner level: " + PlayerSummoner.Level)
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

            var masteriesArray = LeagueUtilities.GetTopPlayedChampions(PlayerSummoner);

            foreach (string mastery in masteriesArray)
            {
                var masteryInfo = new Label(mastery)
                {
                    X = Pos.Percent(0),
                    Y = prev.Y + 1,
                    Width = Dim.Fill(),
                    Height = 1
                };
                prev = masteryInfo;
                SummonerInfoView.Add(masteryInfo);
            }

        }


        private static void RedrawMatchHistory(List<MatchHistoryElement> matchHistoryElements)
        {
            MatchHistoryListView.SetSource(matchHistoryElements);
            MatchHistoryListView.OpenSelectedItem += RedrawMatchDetails;
        }

        private static void RedrawMatchDetails(ListViewItemEventArgs args)
        {
            MatchDetailsView.RemoveAll();
            //MatchHistoryElement matchHistoryElement = (MatchHistoryElement) selectedItem;
            var argss = args;
            var element = (MatchHistoryElement) args.Value;
            var match = LeagueUtilities.Api.Match.GetMatchAsync(SearchRegion, element.Reference.GameId).Result;

            // Get participant stats object of summoner (imaqtpie)
            var particpantsId = match.ParticipantIdentities.Single(x => x.Player.AccountId == PlayerSummoner.AccountId);
            var participant = match.Participants.Single(x => x.ParticipantId == particpantsId.ParticipantId);

            var label = new Label("Summoner Name: " + PlayerSummoner.Name)
            {
                X = Pos.Percent(0),
                Y = Pos.Percent(0),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(label);

            var startDate = new Label("Game start: " + match.GameCreation)
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(label),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(startDate);


            var gameDuration = new Label("Game duration: " + match.GameDuration)
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(startDate),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(gameDuration);

            var gameMode = new Label("Game Mode: " + match.GameMode)
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(gameDuration),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(gameMode);

            string sideName = participant.TeamId == 100 ? "Blue" : "Red";
            var side = new Label("Side: " + sideName)
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(gameMode),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(side);

            var emptyLabel1 = new Label(" ")
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(side),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(emptyLabel1);


            var championPlayed = new Label("Champion: " + LeagueUtilities.Champions.Values.Single(x => x.Id == participant.ChampionId).Name)
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(emptyLabel1),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(championPlayed);

            string winner = participant.Stats.Winner ? "Winner" : "Loser";
            var resultLabel = new Label("Result: " + winner)
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(championPlayed),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(resultLabel);


            var statsLabel = new Label("Stats")
            {
                X = Pos.Percent(50)-5,
                Y = Pos.Bottom(resultLabel),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(statsLabel);

            var killsLabel = new Label( string.Format("{0,-9} {1,-9}","Kills: ", participant.Stats.Kills))
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(statsLabel),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(killsLabel);

            var deathsLabel = new Label(string.Format("{0,-9} {1,-9}", "Deaths: ", participant.Stats.Deaths))
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(killsLabel),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(deathsLabel);

            var assistsLabel = new Label(string.Format("{0,-9} {1,-9}", "Assists: ", participant.Stats.Assists))
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(deathsLabel),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(assistsLabel);

            //DMG DEALT

            var emptyLabel2 = new Label(" ")
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(assistsLabel),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(emptyLabel2);

            var totalDamageDealtLabel = new Label("Damage dealt to champions")
            {
                X = Pos.Percent(50) - 14,
                Y = Pos.Bottom(emptyLabel2),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(totalDamageDealtLabel);

            var physicalLabel = new Label(string.Format("{0,-10} {1,-9}", "Physical: ", participant.Stats.PhysicalDamageDealtToChampions))
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(totalDamageDealtLabel),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(physicalLabel);

            var magicLabel = new Label(string.Format("{0,-10} {1,-9}", "Magic: ", participant.Stats.MagicDamageDealtToChampions))
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(physicalLabel),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(magicLabel);

            var trueDamage = new Label(string.Format("{0,-10} {1,-9}", "True: ", participant.Stats.TrueDamageDealtToChampions))
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(magicLabel),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(trueDamage);

            var totalDamage = new Label(string.Format("{0,-10} {1,-9}", "Total: ", participant.Stats.TotalDamageDealtToChampions))
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(trueDamage),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(totalDamage);

            //final build

            var emptyLabel3 = new Label(" ")
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(totalDamage),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(emptyLabel3);

            var items = new Label(string.Format("    {0}", "Final Build"))
            {
                X = Pos.Percent(50) - 14,
                Y = Pos.Bottom(emptyLabel3),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(items);

            string item0 = LeagueUtilities.Items.ContainsKey((int)participant.Stats.Item0) ? LeagueUtilities.Items[(int)participant.Stats.Item0].Name : "None";
            var item0label = new Label(string.Format("• {0}", item0))
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(items),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(item0label);


            string item1 = LeagueUtilities.Items.ContainsKey((int)participant.Stats.Item1) ? LeagueUtilities.Items[(int)participant.Stats.Item1].Name : "None";
            var item1label = new Label(string.Format("• {0}", item1))
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(item0label),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(item1label);


            string item2 = LeagueUtilities.Items.ContainsKey((int)participant.Stats.Item2) ? LeagueUtilities.Items[(int)participant.Stats.Item2].Name : "None";
            var item2label = new Label(string.Format("• {0}", item2))
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(item1label),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(item2label);


            string item3 = LeagueUtilities.Items.ContainsKey((int)participant.Stats.Item3) ? LeagueUtilities.Items[(int)participant.Stats.Item3].Name : "None";
            var item3label = new Label(string.Format("• {0}", item3))
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(item2label),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(item3label);


            string item4 = LeagueUtilities.Items.ContainsKey((int)participant.Stats.Item4) ? LeagueUtilities.Items[(int)participant.Stats.Item4].Name : "None";
            var item4label = new Label(string.Format("• {0}", item4))
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(item3label),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(item4label);


            string item5 = LeagueUtilities.Items.ContainsKey((int)participant.Stats.Item5) ? LeagueUtilities.Items[(int)participant.Stats.Item5].Name : "None";
            var item5label = new Label(string.Format("• {0}", item5))
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(item4label),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(item5label);


            string item6 = LeagueUtilities.Items.ContainsKey((int)participant.Stats.Item6) ? LeagueUtilities.Items[(int)participant.Stats.Item6].Name + " (trinket)" : "None";
            var item6label = new Label(string.Format("• {0}", item6))
            {
                X = Pos.Percent(0),
                Y = Pos.Bottom(item5label),
                Width = Dim.Fill(),
                Height = 1
            };
            MatchDetailsView.Add(item6label);

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
