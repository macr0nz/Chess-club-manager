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
        private readonly IRepository<News> newsRepository;

        public HomeController()
        {
            this.playersRepository = new ChessClubManagerRepository<Player>();
            this.newsRepository = new ChessClubManagerRepository<News>();
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

        public ActionResult TopNews()
        {
            var news = this.newsRepository.All()
                .OrderByDescending(p => p.CreatedDate)
                .Take(4)
                .ToList();

            return PartialView(news);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Chess Club Manager description page.";

            return View();
        }

        
    }
}