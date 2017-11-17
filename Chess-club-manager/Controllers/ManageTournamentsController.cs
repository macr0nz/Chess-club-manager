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
        private readonly IRepository<Tournament> _tournamentsRepository;
        private readonly IRepository<ApplicationUser> _usersRepository;
        
        public ManageTournamentsController()
        {
            this._tournamentsRepository = new ChessClubManagerRepository<Tournament>();
            this._usersRepository = new ChessClubManagerRepository<ApplicationUser>();
        }

        // GET: ManageTournaments
        [Authorize(Roles = "admin, moderator")]
        public ActionResult Index()
        {
            var allTwithCreators = _tournamentsRepository.All().Include("Creator").ToList();

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
                using (var context = new ApplicationDbContext())
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

                    var currentUser = context.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);
                    if (currentUser != null)
                    {
                        tournament.Creator = currentUser;
                        tournament.CreatorId = currentUser.Id;

                        tournament.Arbitrators = new List<ApplicationUser> {currentUser};
                    }


                    context.Tournaments.Add(tournament);
                    context.SaveChanges();

                    //redirect to this tournament page
                    return RedirectToAction("Details", "Tournaments", new { id = tournament.Id });
                }
                
            }

            return View(trnCreateDto);
        }

        //[TEST]
        public ActionResult AddRandomPlayers(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var tournament = context.Tournaments.SingleOrDefault(x => x.Id == id);
                if (tournament == null)
                {
                    return HttpNotFound();
                }


                var users = context.Users.Take(5).ToList();

                if (tournament.Players == null)
                {
                    tournament.Players = new List<ApplicationUser>();
                }

                foreach (var user in users)
                {
                    tournament.Players.Add(user);
                }

                context.SaveChanges();
            }
            
            return RedirectToAction("Details", "Tournaments", new {id = id});
        }

        public ActionResult ManageArbitrators(int? id)
        {
            if (id == null) return HttpNotFound();

            var tournament = this._tournamentsRepository.All()
                .Include("Arbitrators")
                .Include("Creator")
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


            var allUsers = this._usersRepository.All()
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
            using (var context = new ApplicationDbContext())
            {
                var tournament = context.Tournaments
                    .Include("Arbitrators")
                    .SingleOrDefault(x => x.Id == tournamentId);

                var user = context.Users.SingleOrDefault(x => x.Id == userId);

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

                tournament.Arbitrators.Add(user);

                context.SaveChanges();

                return RedirectToAction("ManageArbitrators", new { id = tournament.Id });
            }
            
        }

        
        [HttpGet]
        public ActionResult RemoveArbitratorFromTournament(int tournamentId, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var tournament = context.Tournaments
                    .Include("Arbitrators")
                    .SingleOrDefault(x => x.Id == tournamentId);

                var user = context.Users.SingleOrDefault(x => x.Id == userId);

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
                        context.SaveChanges();
                    }
                }

                return RedirectToAction("ManageArbitrators", new { id = tournament.Id });
            }
        }


    }
}