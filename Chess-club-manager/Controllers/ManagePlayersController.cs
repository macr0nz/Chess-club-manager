using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.DTO.Players;
using Chess_club_manager.Filters;
using Chess_club_manager.Models;
using Chess_club_manager.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Chess_club_manager.Controllers
{
    [Culture]
    [Authorize(Roles = "admin, moderator")]
    public class ManagePlayersController : Controller
    {
        private readonly IRepository<ApplicationUser> playersRepository;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManagePlayersController()
        {
            this.playersRepository = new ChessClubManagerRepository<ApplicationUser>();
            
        }

        public ManagePlayersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            this.playersRepository = new ChessClubManagerRepository<ApplicationUser>();

            this.UserManager = userManager;
            this.SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        // GET: ManagePlayers
        public ActionResult Index()
        {
            var allPlayers = this.playersRepository.All()
                .AsNoTracking()
                .Select(x => new ViewManagePlayerDto
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Title = x.Title,
                    CurrentRating = x.CurrentRating,
                    BirthDay = x.BirthDay,
                    Info = x.Info,
                    Roles = x.Roles
                }).ToList();

            return View(allPlayers);
        }

        // GET: ManagePlayers/Details/5
        public ActionResult Details(string id)
        {
            return RedirectToAction("Details", "Players", new {id = id});
        }

        // GET: ManagePlayers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManagePlayers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreatePlayerDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    Email = model.Email,
                    BirthDay = model.BirthDay,
                    Info = model.Info,
                    PhoneNumber = model.PhoneNumber,

                    //set default user image
                    ImagePath = "/Resources/Images/user.png"
                };

                //default password if no email
                var password = "qwe123";

                if (!string.IsNullOrEmpty(model.Email))
                {
                    var dateTimeStamp = DateTime.Now.GetHashCode();
                    if (dateTimeStamp < 0)
                    {
                        dateTimeStamp *= -1;
                    }

                    password = model.UserName + dateTimeStamp.ToString();
                }
                
                var result = await this.UserManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    var roleresult = this.UserManager.AddToRole(user.Id, "user");

                    if (!string.IsNullOrEmpty(model.Email))
                    {
                        using (var emailNotificationService = new EmailNotificationService())
                        {
                            emailNotificationService.SendRegistrationMail(model.Email, model.UserName, password);
                        }
                    }

                    
                    return RedirectToAction("Index", "ManagePlayers");
                }
                else
                {
                    this.AddModelErrors(result);
                }
            }

            return View(model);
        }

        private void AddModelErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        // GET: ManagePlayers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var player = this.playersRepository.All().Where(x => x.Id == id).Select(x => new EditPlayerDto
            {
                Id = x.Id,
                UserName = x.UserName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Info = x.Info,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                BirthDay = x.BirthDay
            }).SingleOrDefault();

            if (player == null)
            {
                return HttpNotFound();
            }

            player.IsModerator = UserManager.IsInRole(player.Id, "moderator");
            player.IsAdmin = UserManager.IsInRole(player.Id, "admin");
            player.IsUser = true; // always?

            return View(player);
        }

        // POST: ManagePlayers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPlayerDto editPlayerDto)
        {
            if (ModelState.IsValid)
            {
                var player = this.playersRepository.All().SingleOrDefault(x => x.Id == editPlayerDto.Id);

                if (player == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                player.FirstName = editPlayerDto.FirstName;
                player.LastName = editPlayerDto.LastName;
                if (editPlayerDto.BirthDay != null)
                {
                    player.BirthDay = editPlayerDto.BirthDay;
                }
                player.Info = editPlayerDto.Info;
                player.UserName = editPlayerDto.UserName;
                player.Email = editPlayerDto.Email;
                player.PhoneNumber = editPlayerDto.PhoneNumber;

                //is moderator true
                if (editPlayerDto.IsModerator)
                {
                    if (!UserManager.IsInRole(player.Id, "moderator"))
                    {
                        UserManager.AddToRole(player.Id, "moderator");
                    }
                }
                else
                {
                    if (UserManager.IsInRole(player.Id, "moderator"))
                    {
                        UserManager.RemoveFromRole(player.Id, "moderator");
                    }
                }

                //is admin true
                if (editPlayerDto.IsAdmin)
                {
                    if (!UserManager.IsInRole(player.Id, "admin"))
                    {
                        UserManager.AddToRole(player.Id, "admin");
                    }
                }
                else
                {
                    if (UserManager.IsInRole(player.Id, "admin"))
                    {
                        UserManager.RemoveFromRole(player.Id, "admin");
                    }
                }

                //is user true
                //always?

                this.playersRepository.Update(player);

                return RedirectToAction("Index");
            }
            return View(editPlayerDto);
        }

        //// GET: ManagePlayers/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var player = this.playersRepository.All().SingleOrDefault(x => x.Id == id);

        //    if (player == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(player);
        //}

        //// POST: ManagePlayers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    var player = this.playersRepository.All().SingleOrDefault(x => x.Id == id);

        //    if (player == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    this.playersRepository.Delete(player);

        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        this.playersRepository.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
