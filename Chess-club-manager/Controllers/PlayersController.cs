using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
    public class PlayersController : Controller
    {
        private readonly ChessClubManagerUnitOfWork _unitOfWork;

        public PlayersController()
        {
            this._unitOfWork = new ChessClubManagerUnitOfWork();
        }

        // GET: Players
        public ActionResult Index()
        {
            //except admin
            var allPlayers = _unitOfWork.UsersRepository.All()
                .AsNoTracking()
                .Select(x => new ViewPlayerDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Title = x.Title,
                    CurrentRating = x.CurrentRating,
                    BirthDay = x.BirthDay,
                    Info = x.Info,
                    ImagePath = x.ImagePath
                }).ToList();

            return View(allPlayers);
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var player = this._unitOfWork.UsersRepository
                .All()
                .Where(p => p.Id == id)
                .Select(x => new PlayerDetailsDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BirthDay = x.BirthDay,
                Info = x.Info,
                CurrentRating = x.CurrentRating,
                Title = x.Title,
                Tournaments = x.PlayedTournaments,
                UserName = x.UserName,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                Roles = x.Roles,
                ImagePath = x.ImagePath
            }).SingleOrDefault();

            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        public ActionResult PlayersChart(string id)
        {
            var user = this._unitOfWork.UsersRepository.All().SingleOrDefault(p => p.Id == id);

            if (user == null)
            {
                return HttpNotFound("Invalid request. User not found");
            }

            ViewBag.CharLabel = $"{user.FirstName} {user.LastName} - {Resources.Resource.PlayerChart}";

            //get logic
            //form array logic
            //return array to js

            return PartialView();
        }

        public ActionResult PlayersGameHistory(string id)
        {
            var player = this._unitOfWork.UsersRepository.All().SingleOrDefault(p => p.Id == id);

            if (player == null)
            {
                return HttpNotFound("Invalid request. User not found");
            }

            var tournaments = _unitOfWork.TournamentsRepository.All()
                .AsNoTracking()
                //.Include(t => t.Players)
                .Where(p => p.Players.Select(pl => pl.Id).Contains(player.Id))
                .AsEnumerable()
                .Select(x => new TournamentHistoryDto
                {
                    Id = x.Id,
                    IsCompleted = x.IsCompleted,
                    Name = x.Name,
                    Location = x.Location,
                    IsPrivate = x.IsPrivate,
                    IsOfficial = x.IsOfficial,
                })
                .ToList();

            return PartialView(tournaments);
        }
    }
}