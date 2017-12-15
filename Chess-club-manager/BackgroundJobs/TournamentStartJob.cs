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
                    .Where(x => !x.IsStarted && (x.Start <= currentDateTime || x.Start <= DbFunctions.AddSeconds(currentDateTime, 5)))
                    .Include(x => x.Players)
                    .ToList();


                if (!tournamentsToStart.Any()) return;


                foreach (var tournament in tournamentsToStart)
                {
                    tournament.IsStarted = true;

                    //generate tours for current tour
                    switch (tournament.Format)
                    {
                        case TournamentType.Round:
                        {
                            var tournamentsCount = 0;
                            if (tournament.Players.Count > 0)
                            {
                                if (tournament.Players.Count % 2 == 0)
                                {
                                    tournamentsCount = tournament.Players.Count - 1;
                                }
                                else
                                {
                                    tournamentsCount = tournament.Players.Count;
                                }
                            }
                            tournament.MaxToursCount = tournamentsCount;

                            //TEST ONLY
                            var rand = new Random();
                            var players = tournament.Players.ToList();
                            //TEST ONLY

                            if (tournament.Tours == null)
                            {
                                tournament.Tours = new List<TournamentTour>(tournamentsCount);
                            }

                            var gamesInATour = players.Count / 2;


                            for (var i = 0; i < tournamentsCount; i++)
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
                                tour.Number = i+1;
                                
                                tournament.Tours.Add(tour);
                            }

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
                    
                }

                unitOfWork.TournamentsRepository.UpdateRange(tournamentsToStart);
                
            }
        }

    }
}