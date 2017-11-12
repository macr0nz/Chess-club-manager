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
    [Authorize(Roles = "admin, moderator")]
    public class ManageTournamentsController : Controller
    {
        private readonly IRepository<Tournament> _tournamentsRepository;

        public ManageTournamentsController()
        {
            this._tournamentsRepository = new ChessClubManagerRepository<Tournament>();
        }
        // GET: ManageTournaments
        public ActionResult Index()
        {
            var allTwithCreators = _tournamentsRepository.All().Include("Creator").ToList();

            var allTournaments = allTwithCreators
                .Select(x => new ManageTournamentDto
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
                    CreatorId = x.Creator.Id,
                    CreatorName = x.Creator.UserName
                }).ToList();

            return View(allTournaments);
        }
    }
}