﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.DTO.Tournament;
using Chess_club_manager.DTO.Tournament.Tour;
using Chess_club_manager.Filters;
using Chess_club_manager.Models;
using Chess_club_manager.Repository;

namespace Chess_club_manager.Controllers
{
    [Culture]
    public class TournamentsController : Controller
    {
        private readonly IRepository<Tournament> _tournamentsRepository;
        private readonly IRepository<TournamentTour> _toursRepository;
        private readonly IRepository<TourGame> _tourGamesRepository;
        
        public TournamentsController()
        {
            this._tournamentsRepository = new ChessClubManagerRepository<Tournament>();
            this._toursRepository = new ChessClubManagerRepository<TournamentTour>();
            this._tourGamesRepository = new ChessClubManagerRepository<TourGame>();
        }

        //mock constructor
        public TournamentsController(IRepository<Tournament> mockTournamentRepository, IRepository<TourGame> mockTourGamesRepository, IRepository<TournamentTour> mockToursRepository)
        {
            this._tournamentsRepository = mockTournamentRepository;
            this._tourGamesRepository = mockTourGamesRepository;
            this._toursRepository = mockToursRepository;
        }

        // GET: Tournaments
        public ActionResult Index()
        {
            var allTournaments = this._tournamentsRepository.All()
                .OrderByDescending(x => x.CreatedDate)
                .AsNoTracking()
                .Select(x => new TournamentDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Start = x.Start,
                    Finish = x.Finish,
                    IsOfficial = x.IsOfficial,
                    TimeControl = x.TimeControl,
                    Location = x.Location,
                    IsPrivate = x.IsPrivate,
                    MaxPlayersCount = x.MaxPlayersCount,
                    IsStarted = x.IsStarted,
                    IsCompleted = x.IsCompleted,
                    AssignedPlayersCount = x.Players.Count
                }).ToList();

            //if need nested data 
            //var allTwithCreators = _tournamentsRepository.All().Include("Creator").ToList();
            
            return View(allTournaments);
        }

        public ActionResult Details(int id)
        {
            var tournament = this._tournamentsRepository.All()
                .Include(x => x.Creator)
                .Include(x => x.Players)
                .Include(x => x.Arbitrators)
                .Include(x => x.Tours)
                .Include(x => x.Tours.Select(y => y.Games))
                .AsNoTracking().SingleOrDefault(x => x.Id == id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            var access = false;
            if (Request.IsAuthenticated)
            {
                access = tournament.Arbitrators.Select(x => x.UserName).Contains(User.Identity.Name);

                if (!access)
                {
                    access = User.IsInRole("moderator");

                    if (!access)
                    {
                        access = User.IsInRole("admin");
                    }
                }
            }

            var tournamentView = new TournamentViewDto
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Location = tournament.Location,
                Start = tournament.Start,
                Finish = tournament.Finish,
                Info = tournament.Info,
                IsOfficial = tournament.IsOfficial,
                IsPrivate = tournament.IsPrivate,
                MaxPlayersCount = tournament.MaxPlayersCount,
                MaxToursCount = tournament.MaxToursCount,
                CreatorName = $"{tournament.Creator.FirstName} {tournament.Creator.LastName}",
                CreatorId = tournament.Creator.Id,
                Format = tournament.Format,

                Players = tournament.Players.Select(x => new TournamentViewPlayerLightDto
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    FirstNameLastName = $"{x.FirstName} {x.LastName}",
                    BirthDay = x.BirthDay,
                    CurrentRating = x.CurrentRating,
                    Title = x.Title
                }).ToList(),

                Arbitrators = tournament.Arbitrators.Select(x => new TournamentViewUserLightDto
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    FirstNameLastName = $"{x.FirstName} {x.LastName}"
                }).ToList(),

                IsStarted = tournament.IsStarted,
                IsCompleted = tournament.IsCompleted,
                CreatedDate = tournament.CreatedDate,
                TimeControl = tournament.TimeControl,
                AccessPassed = access,

                Tours = tournament.Tours
            };

            if (tournament.IsCompleted)
            {
                //tournamentView.TournamentTable = GenerateResultTable(tournament);

                var allTours = tournament.Tours;
                var allGamesId = new List<int>();
                foreach (var tournamentTour in allTours)
                {
                    allGamesId.AddRange(tournamentTour.Games.Select(x => x.Id));
                }

                var tournamentGames = _tourGamesRepository.All()
                    .Include(x => x.LeftPlayer)
                    .Include(x => x.RightPlayer)
                    .Where(x => allGamesId.Contains(x.Id))
                    .ToList();

                tournamentView.TourGames = tournamentGames;
            }

            return View(tournamentView);
        }

        private TournamentTable GenerateResultTable(Tournament tournament)
        {
            var table = new TournamentTable {Rows = new List<TournamentTableRow>()};

            //TODO: generate tournament table


            return table;
        }

        public ActionResult TourDetails(int id)
        {
            var tour = this._toursRepository
                .All()
                .Include(x => x.Tournament)
                .Include(x => x.Tournament.Arbitrators)
                .Include(x => x.Games)
                .Include(x => x.Games.Select(g => g.LeftPlayer))
                .Include(x => x.Games.Select(g => g.RightPlayer))
                .SingleOrDefault(x => x.Id == id);

            if (tour == null)
            {
                return HttpNotFound();
            }

            var editAccess = false;
            if (User != null)
            {
                editAccess = tour.Tournament.Arbitrators.Select(x => x.UserName).Contains(User.Identity.Name);
            }

            var tourDetails = new TourDetailsDto
            {
                Id = tour.Id,
                EditAccess = editAccess,
                IsCompleted = tour.IsCompleted,
                CompletedDateTime = tour.CompletedDateTime,
                Games = tour.Games,
                Number = tour.Number,
                TournamentName = tour.Tournament.Name,
                TournamentId = tour.Tournament.Id
            };

            return View(tourDetails);
        }

        

    }
}