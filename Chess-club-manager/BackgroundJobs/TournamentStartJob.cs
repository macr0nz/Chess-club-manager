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
            var players = tournament.Players.ToList();
            var playersCount = players.Count;

            //tours count = N-1 (even players) || N (!even)
            var toursCount = playersCount.IsEven() ? playersCount - 1 : playersCount;
            
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
            
            //randomize player's indexes?

            for (var i = 1; i <= toursCount; i++)
            {
                var tour = new TournamentTour
                {
                    Number = i,
                    Tournament = tournament,
                    Games = new List<TourGame>()
                };

                //get games
                var columnsCount = playersCount.IsEven() ? playersCount / 2 : (playersCount + 1) / 2;
                var row1 = players.GetRange(0, columnsCount);
                var row2 = players.GetRange(columnsCount, players.Count);
                if (!playersCount.IsEven())
                {
                    row2.Add(null);
                }
                row2.Reverse();

                for (var col = 0; col < columnsCount; col++)
                {
                    tour.Games.Add(new TourGame
                    {
                        Tour = tour,
                        LeftPlayer = row1[col],
                        RightPlayer = row2[col]
                    });
                }
                
                tournament.Tours.Add(tour);

                //shift players index
                //1 2 3  =>  1 6 2
                //6 5 4      5 4 3


            }
            


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