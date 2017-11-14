using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.DTO.Players;
using Chess_club_manager.DTO.Tournament;
using Chess_club_manager.Filters;
using Chess_club_manager.Models;
using Chess_club_manager.Repository;

namespace Chess_club_manager.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        private readonly IRepository<ApplicationUser> playersRepository;
        private readonly IRepository<News> newsRepository;
        private readonly IRepository<Tournament> tournamentsRepository;

        
        public HomeController()
        {
            this.playersRepository = new ChessClubManagerRepository<ApplicationUser>();
            this.newsRepository = new ChessClubManagerRepository<News>();
            this.tournamentsRepository = new ChessClubManagerRepository<Tournament>();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChangeCulture(string lang)
        {
            if (Request.UrlReferrer != null)
            {
                var returnUrl = Request.UrlReferrer.AbsolutePath;
                 
                var cultures = new List<string>() { "ru", "en", "md" };

                if (!cultures.Contains(lang))
                {
                    lang = "ru";
                }
                

                var cookie = Request.Cookies["lang"];

                if (cookie != null)
                {
                    cookie.Value = lang;
                }  
                else
                {
                    cookie = new HttpCookie("lang")
                    {
                        HttpOnly = false,
                        Value = lang,
                        Expires = DateTime.Now.AddYears(1)
                    };
                }

                Response.Cookies.Add(cookie);

                return Redirect(returnUrl);
            }

            return RedirectToAction("Index");
        }

        public ActionResult TopPlayers()
        {
            var playersNumberToReturn = 6;

            var players = this.playersRepository.All()
                .AsNoTracking()
                .OrderByDescending(p => p.CurrentRating)
                .Take(playersNumberToReturn)
                .Select(x => new HomePagePlayerView
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    //Info = x.Info,
                    Title = x.Title,
                    CurrentRating = x.CurrentRating,
                }).ToList();
            
            return PartialView(players);
        }

        public ActionResult TopNews()
        {
            var news = this.newsRepository.All()
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedDate)
                .Take(4)
                .ToList();

            return PartialView(news);
        }

        public ActionResult TopTournaments()
        {
            var topTournaments = new HomePageTournamentView();

            topTournaments.CurrentTournaments = this.tournamentsRepository.All().AsNoTracking()
                .Where(x => x.IsStarted && !x.IsCompleted)
                .Select(x => new HomePageTournament
                {
                    Id = x.Id,
                    Name = x.Name,
                    Start = x.Start,
                    Finish = x.Finish,
                    IsPrivate = x.IsPrivate,
                    IsOfficial = x.IsOfficial,
                    MaxPlayersCount = x.MaxPlayersCount,
                    Format = x.Format
                }).Take(5).ToList();

            topTournaments.FutureTournaments = this.tournamentsRepository.All().AsNoTracking()
                .Where(x => !x.IsStarted && x.Start > DateTime.Now)
                .Select(x => new HomePageTournament
                {
                    Id = x.Id,
                    Name = x.Name,
                    Start = x.Start,
                    Finish = x.Finish,
                    IsPrivate = x.IsPrivate,
                    IsOfficial = x.IsOfficial,
                    MaxPlayersCount = x.MaxPlayersCount,
                    Format = x.Format
                }).Take(5).ToList();

            topTournaments.LastTournaments = this.tournamentsRepository.All().AsNoTracking()
                .Where(x =>  x.IsCompleted && x.Finish > DateTime.Now )
                .Select(x => new HomePageTournament
                {
                    Id = x.Id,
                    Name = x.Name,
                    Start = x.Start,
                    Finish = x.Finish,
                    IsPrivate = x.IsPrivate,
                    IsOfficial = x.IsOfficial,
                    MaxPlayersCount = x.MaxPlayersCount,
                    Format = x.Format
                }).Take(5).ToList();

            return PartialView(topTournaments);
        }

        public ActionResult About()
        {
            ViewBag.Message = $"{Resources.Resource.ApplicationName} description page.";

            return View();
        }

        
    }
}