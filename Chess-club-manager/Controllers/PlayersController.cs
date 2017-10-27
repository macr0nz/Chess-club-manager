using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.DTO.Players;
using Chess_club_manager.Repository;

namespace Chess_club_manager.Controllers
{
    public class PlayersController : Controller
    {
        private IRepository<Player> playersRepository;

        public PlayersController()
        {
            playersRepository = new ChessClubManagerRepository<Player>();
        }

        // GET: Players
        public ActionResult Index()
        {
            var allPlayers = this.playersRepository.All().Select(x => new ViewPlayerDto
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

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var player = this.playersRepository.All().Where(p => p.Id == id).Select(x => new PlayerDetailsDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BirthDay = x.BirthDay,
                Info = x.Info,
                CurrentRating = x.CurrentRating,
                Title = x.Title,
                Tournaments = x.Tournaments,
            }).SingleOrDefault();

            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }
    }
}