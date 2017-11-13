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
        
        public ManageTournamentsController()
        {
            this._tournamentsRepository = new ChessClubManagerRepository<Tournament>();
        }

        // GET: ManageTournaments
        [Authorize(Roles = "admin, moderator")]
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

                        tournament.Arbitrators = new List<ApplicationUser> { currentUser };
                    }


                    context.Tournaments.Add(tournament);
                    context.SaveChanges();

                    //redirect to this tournament page
                    return RedirectToAction("Details", "Tournaments", new { id = tournament.Id });
                }
                
            }

            return View(trnCreateDto);
        }
    }
}