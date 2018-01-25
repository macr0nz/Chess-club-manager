using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Enum;
using Chess_club_manager.Helpers;
using Chess_club_manager.Models;
using Chess_club_manager.Repository;
using Quartz;

namespace Chess_club_manager.BackgroundJobs
{
    public class TournamentStartJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            using (var unitOfWork = new ChessClubManagerUnitOfWork())
            {
                var currentDateTime = DateTime.Now;

                var tournamentsToStart = unitOfWork.TournamentsRepository
                    .All()
                    .Where(x => !x.IsStarted && (x.Start <= currentDateTime || x.Start <= DbFunctions.AddSeconds(currentDateTime, 10)))
                    .Include(x => x.Players)
                    .ToList();


                if (!tournamentsToStart.Any()) return;


                foreach (var tournament in tournamentsToStart)
                {
                    tournament.IsStarted = true;

                    //generate tours for current tournament
                    switch (tournament.Format)
                    {
                        case TournamentType.Round:
                        {
                            GenerateRoundTypeTours(tournament);

                        } break;

                        case TournamentType.Olympic:
                        {


                        } break;

                        case TournamentType.Swiss:
                        {


                        }break;

                        default: break;
                    }
                    
                    //log
                    ApplicationLogger.Log($"Tournament Auto Start: {tournament.Name}", LogType.Info);
                    
                    unitOfWork.TournamentsRepository.Update(tournament);
                }

                //unitOfWork.TournamentsRepository.UpdateRange(tournamentsToStart);
                
            }
        }

        private void GenerateRoundTypeTours(Tournament tournament)
        {
            var playersCount = tournament.Players.Count;
            var players = tournament.Players.ToList();
            var toursCount = RoundHardcodeTable.GetToursCountByPlayers(playersCount);
            
            tournament.MaxToursCount = toursCount;

            if(toursCount == 0) { return; }

            if (tournament.Tours == null)
            {
                tournament.Tours = new List<TournamentTour>(toursCount);
            }

            //if players are 2 -> make 1 tour
            if (toursCount == 1)
            {
                var tour1 = new TournamentTour
                {
                    Tournament = tournament,
                    Number = 1,
                };

                var games1 = new List<TourGame>
                {
                    new TourGame
                    {
                        Tour = tour1,
                        LeftPlayer = tournament.Players.FirstOrDefault(),
                        RightPlayer = tournament.Players.LastOrDefault(),
                    }
                };

                tour1.Games = games1;

                tournament.Tours.Add(tour1);

                return;
            }

            //if players are > 2


            //for (var i = 1; i <= toursCount; i++)
            //{
            //    var tour = new TournamentTour
            //    {
            //        Tournament = tournament,
            //        Number = i
            //    };

            //    var games = new List<TourGame>();

            //    //var gamesIndexes = RoundHardcodeTable.GetGamesIndexesByTourNumber(tourNumber: tour.Number, toursCount: toursCount);

            //    //foreach (var index in gamesIndexes)
            //    //{
            //    //    games.Add(new TourGame
            //    //    {
            //    //        Tour = tour,
            //    //        LeftPlayer = players[index.First],
            //    //        RightPlayer = players[index.Second]
            //    //    });
            //    //}

            //    tour.Games = games;

            //    tournament.Tours.Add(tour);
            //}


            /////////////////////////////////////////
            //TEST ONLY
            var rand = new Random();
            //var players = tournament.Players.ToList();
            //TEST ONLY

            

            var gamesInATour = players.Count / 2;


            for (var i = 0; i < toursCount; i++)
            {
                var tour = new TournamentTour();

                var games = new List<TourGame>(gamesInATour);


                for (var j = 0; j < gamesInATour; j++)
                {
                    games.Add(new TourGame
                    {
                        //TEST random players!
                        LeftPlayer = players[rand.Next(players.Count)],
                        RightPlayer = players[rand.Next(players.Count)],
                        Tour = tour
                    });
                }

                tour.Games = games;
                tour.Tournament = tournament;
                tour.IsCompleted = false;
                tour.Number = i + 1;

                tournament.Tours.Add(tour);
            }
        }

        private void GenerateSwissTypeTours(Tournament tournament)
        {
        }

        private void GenerateOlympicTypeTours(Tournament tournament)
        {
        }
    }
}