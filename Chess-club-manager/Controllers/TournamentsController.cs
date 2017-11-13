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
                    IsCompleted = x.IsCompleted
                }).ToList();

            //if need nested data 
            //var allTwithCreators = _tournamentsRepository.All().Include("Creator").ToList();
            
            return View(allTournaments);
        }

        public ActionResult Details(int id)
        {
            ViewBag.id = id;

            return View();
        }
    }
}