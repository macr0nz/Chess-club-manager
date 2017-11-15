using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.DTO.Tournament;
using Chess_club_manager.Filters;
using Chess_club_manager.Models;
using Chess_club_manager.Repository;

namespace Chess_club_manager.Controllers
{
    [Culture]
    public class TournamentsController : Controller
    {
        private readonly IRepository<Tournament> _tournamentsRepository;

        public TournamentsController()
        {
            this._tournamentsRepository = new ChessClubManagerRepository<Tournament>();
        }

        // GET: Tournaments
        public ActionResult Index()
        {
            var allTournaments = this._tournamentsRepository.All()
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
                .Include("Creator")
                .Include("Players")
                .Include("Arbitrators")
                .AsNoTracking().SingleOrDefault(x => x.Id == id);

            if (tournament == null)
            {
                return HttpNotFound();
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
                TimeControl = tournament.TimeControl
            };

            return View(tournamentView);
        }
    }
}