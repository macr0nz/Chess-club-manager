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
    [Authorize]
    public class ManageTournamentsController : Controller
    {
        private readonly ChessClubManagerUnitOfWork _unitOfWork;

        public ManageTournamentsController()
        {
            this._unitOfWork = new ChessClubManagerUnitOfWork();
        }

        // GET: ManageTournaments
        [Authorize(Roles = "admin, moderator")]
        public ActionResult Index()
        {
            var allTwithCreators = this._unitOfWork.TournamentsRepository
                .All().Include("Creator").ToList();

            var allTournaments = allTwithCreators
                .OrderByDescending(x => x.CreatedDate)
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

        [Authorize(Roles = "user, moderator, admin")]
        public ActionResult Create()
        {
            //initialized with default props
            var createTournamentModel = new CreateTournamentDto();

            return View(createTournamentModel);
        }

        [HttpPost]
        [Authorize(Roles = "user, moderator, admin")]
        public ActionResult Create(CreateTournamentDto trnCreateDto)
        {
            if (ModelState.IsValid)
            {
                var tournament = new Tournament
                {
                    Format = trnCreateDto.Format,
                    Info = trnCreateDto.Info,
                    IsPrivate = trnCreateDto.IsPrivate,
                    MaxPlayersCount = trnCreateDto.MaxPlayersCount,
                    Name = trnCreateDto.Name,
                    Location = trnCreateDto.Location,
                    TimeControl = trnCreateDto.TimeControl,
                    Start = trnCreateDto.StartDate.Add(trnCreateDto.StartTime.TimeOfDay),
                    IsOfficial = trnCreateDto.IsOfficial
                };

                if (trnCreateDto.FinishDate != null)
                {
                    tournament.Finish = trnCreateDto.FinishDate;
                    if (trnCreateDto.FinishTime != null)
                    {
                        tournament.Finish.Value.Add(trnCreateDto.FinishTime.Value.TimeOfDay);
                    }
                }


                var currentUser = this._unitOfWork.UsersRepository.All()
                    .SingleOrDefault(x => x.UserName == User.Identity.Name);

                if (currentUser != null)
                {
                    tournament.Creator = currentUser;
                    tournament.CreatorId = currentUser.Id;

                    tournament.Arbitrators = new List<ApplicationUser> {currentUser};
                }


                this._unitOfWork.TournamentsRepository.Add(tournament);


                //redirect to this tournament page
                return RedirectToAction("Details", "Tournaments", new {id = tournament.Id});


            }

            return View(trnCreateDto);
        }

        public ActionResult ManagePlayers(int? id)
        {
            if (id == null) return HttpNotFound();

            var tournament = this._unitOfWork.TournamentsRepository
                .All()
                .Include(x => x.Players)
                .Include(x => x.Creator)
                .Include(x => x.Arbitrators)
                .AsNoTracking()
                .SingleOrDefault(x => x.Id == id);

            if (tournament == null) return HttpNotFound();

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

            if (!access) return RedirectToAction("Details", "Tournaments", new { id = tournament.Id });


            var allUsers = this._unitOfWork.UsersRepository
                .All()
                .AsNoTracking()
                .AsEnumerable()
                .Where(p => !tournament.Players.Select(x => x.UserName).Contains(p.UserName))
                .OrderBy(x => x.FirstName)
                .Select(x => new SelectListItem
                {
                    Value = x.Id,
                    Text = $"{x.FirstName} {x.LastName}"
                }).ToList();
            

            var tournamentView = new ManageTournamentPlayersViewDto()
            {
                TournamentId = tournament.Id,
                TournamentName = tournament.Name,
                Start = tournament.Start,
                Finish = tournament.Finish,
                IsOfficial = tournament.IsOfficial,
                IsPrivate = tournament.IsPrivate,
                Players = tournament.Players.Select(x => new PlayerLightDto()
                {
                    Id = x.Id,
                    CurrentRating = x.CurrentRating,
                    FirstNameLastName = $"{x.FirstName} {x.LastName}",
                    Title = x.Title
                }).ToList(),
                Users = allUsers,
                CreatorId = tournament.Creator.Id,
                CreatorName = $"{tournament.Creator.FirstName} {tournament.Creator.LastName}"
            };

            return View(tournamentView);
        }
        

        [HttpPost]
        public ActionResult AddPlayerToATournament(int tournamentId, string userId)
        {
            var tournament = this._unitOfWork.TournamentsRepository
                .All()
                .Include(x => x.Players)
                .Include(x => x.Arbitrators)
                .SingleOrDefault(x => x.Id == tournamentId);

            var user = this._unitOfWork.UsersRepository
                .All().SingleOrDefault(x => x.Id == userId);

            if (tournament == null || user == null)
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

            if (!access) return RedirectToAction("Details", "Tournaments", new { id = tournament.Id });

            tournament.Players.Add(user);

            this._unitOfWork.TournamentsRepository.Update(tournament);

            return RedirectToAction("ManagePlayers", new { id = tournament.Id });
        }

        
        [HttpGet]
        public ActionResult RemovePlayerFromTournament(int tournamentId, string userId)
        {
            var tournament = this._unitOfWork.TournamentsRepository
                .All()
                .Include(x => x.Players)
                .SingleOrDefault(x => x.Id == tournamentId);

            var user = this._unitOfWork.UsersRepository
                .All().SingleOrDefault(x => x.Id == userId);

            if (tournament == null || user == null)
            {
                return HttpNotFound();
            }

            var userToRemove = tournament.Players.SingleOrDefault(x => x.Id == userId);

            if (userToRemove != null)
            {
                tournament.Players.Remove(user);

                this._unitOfWork.TournamentsRepository.Update(tournament);
            }

            return RedirectToAction("ManagePlayers", new { id = tournament.Id });
        }

        

        public ActionResult ManageArbitrators(int? id)
        {
            if (id == null) return HttpNotFound();

            var tournament = this._unitOfWork.TournamentsRepository
                .All()
                .Include(x => x.Arbitrators)
                .Include(x => x.Creator)
                .AsNoTracking()
                .SingleOrDefault(x => x.Id == id);

            if (tournament == null) return HttpNotFound();

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

            if (!access) return RedirectToAction("Details", "Tournaments", new {id = tournament.Id});


            var allUsers = this._unitOfWork.UsersRepository
                .All()
                .AsNoTracking()
                .AsEnumerable()
                .Where(p => !tournament.Arbitrators.Select(x => x.UserName).Contains(p.UserName))
                .OrderBy(x => x.FirstName)
                .Select(x => new SelectListItem
                {
                    Value = x.Id,
                    Text = $"{x.FirstName} {x.LastName}"
                }).ToList();



            var tournamentView = new ManageTournamentArbitratorsViewDto
            {
                TournamentId = tournament.Id,
                TournamentName = tournament.Name,
                Start = tournament.Start,
                Finish = tournament.Finish,
                IsOfficial = tournament.IsOfficial,
                IsPrivate = tournament.IsPrivate,
                Arbitrators = tournament.Arbitrators.Select(x => new ArbitratorLightDto
                {
                    Id = x.Id,
                    CurrentRating = x.CurrentRating,
                    FirstNameLastName = $"{x.FirstName} {x.LastName}",
                    Title = x.Title
                }).ToList(),
                Users = allUsers,
                CreatorId = tournament.Creator.Id,
                CreatorName = $"{tournament.Creator.FirstName} {tournament.Creator.LastName}"
            };
            
            return View(tournamentView);
        }

        [HttpPost]
        public ActionResult AddArbitratorToATournament(int tournamentId, string userId)
        {
            var tournament = this._unitOfWork.TournamentsRepository
                .All()
                .Include("Arbitrators")
                .SingleOrDefault(x => x.Id == tournamentId);

            var user = this._unitOfWork.UsersRepository
                .All().SingleOrDefault(x => x.Id == userId);

            if (tournament == null || user == null)
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

            if (!access) return RedirectToAction("Details", "Tournaments", new {id = tournament.Id});

            tournament.Arbitrators.Add(user);

            this._unitOfWork.TournamentsRepository.Update(tournament);

            return RedirectToAction("ManageArbitrators", new {id = tournament.Id});
        }


        [HttpGet]
        public ActionResult RemoveArbitratorFromTournament(int tournamentId, string userId)
        {

            var tournament = this._unitOfWork.TournamentsRepository
                .All()
                .Include("Arbitrators")
                .SingleOrDefault(x => x.Id == tournamentId);

            var user = this._unitOfWork.UsersRepository
                .All().SingleOrDefault(x => x.Id == userId);

            if (tournament == null || user == null)
            {
                return HttpNotFound();
            }

            var userToRemove = tournament.Arbitrators.SingleOrDefault(x => x.Id == userId);

            if (userToRemove != null)
            {
                //block to revome creator from arbitrators
                if (userToRemove != tournament.Creator)
                {
                    tournament.Arbitrators.Remove(user);
                    this._unitOfWork.TournamentsRepository.Update(tournament);
                }
            }

            return RedirectToAction("ManageArbitrators", new {id = tournament.Id});
        }
    }


}
