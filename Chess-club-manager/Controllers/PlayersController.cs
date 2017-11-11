using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.DTO.Players;
using Chess_club_manager.Filters;
using Chess_club_manager.Models;
using Chess_club_manager.Repository;

namespace Chess_club_manager.Controllers
{
    [Culture]
    public class PlayersController : Controller
    {
        private readonly IRepository<ApplicationUser> _playersRepository;

        public PlayersController()
        {
            _playersRepository = new ChessClubManagerRepository<ApplicationUser>();
        }

        // GET: Players
        public ActionResult Index()
        {
            //except admin
            var allPlayers = this._playersRepository.All().Select(x => new ViewPlayerDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Title = x.Title,
                CurrentRating = x.CurrentRating,
                BirthDay = x.BirthDay,
                Info = x.Info
            }).ToList();

            return View(allPlayers);
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var player = this._playersRepository.All().Where(p => p.Id == id).Select(x => new PlayerDetailsDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BirthDay = x.BirthDay,
                Info = x.Info,
                CurrentRating = x.CurrentRating,
                Title = x.Title,
                Tournaments = x.Tournaments,
                UserName = x.UserName,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                Roles = x.Roles
            }).SingleOrDefault();

            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        public ActionResult PlayersChart(string id)
        {
            //get logic
            //form array logic
            //return array to js

            return PartialView();
        }
    }
}