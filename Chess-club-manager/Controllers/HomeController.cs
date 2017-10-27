using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.DTO.Players;
using Chess_club_manager.Repository;

namespace Chess_club_manager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Player> playersRepository;

        public HomeController()
        {
            this.playersRepository = new ChessClubManagerRepository<Player>();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TopPlayers()
        {
            var playersNumberToReturn = 6;

            var players = this.playersRepository.All()
                .OrderByDescending(p => p.CurrentRating)
                .Take(playersNumberToReturn)
                .Select(x => new HomePagePlayerView
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Info = x.Info,
                    CurrentRating = x.CurrentRating,
                }).ToList();
            
            return PartialView(players);
        }

        public ActionResult About()
        {
            ViewBag.Message = "ANS Chess Club Manager description page.";

            return View();
        }

        
    }
}