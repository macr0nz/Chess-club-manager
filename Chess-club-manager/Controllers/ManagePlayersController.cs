using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    [Authorize]
    public class ManagePlayersController : Controller
    {
        private readonly IRepository<Player> playersRepository;

        public ManagePlayersController()
        {
            this.playersRepository = new ChessClubManagerRepository<Player>();
        }

        // GET: ManagePlayers
        public ActionResult Index()
        {
            var allPlayers = this.playersRepository.All().ToList();

            return View(allPlayers);
        }

        // GET: ManagePlayers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var player = this.playersRepository.All().SingleOrDefault(x => x.Id == id);

            if (player == null)
            {
                return HttpNotFound();
            }

            return View(player);
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePlayerDto createPlayerDto)
        {
            if (ModelState.IsValid)
            {
                this.playersRepository.Add(new Player
                {
                    FirstName = createPlayerDto.FirstName,
                    LastName = createPlayerDto.LastName,
                    BirthDay = createPlayerDto.BirthDay,
                    Info = createPlayerDto.Info,
                });
                return RedirectToAction("Index");
            }

            return View(createPlayerDto);
        }

        // GET: ManagePlayers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var player = this.playersRepository.All().Where(x => x.Id == id).Select(x => new EditPlayerDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Info = x.Info,
                BirthDay = x.BirthDay,
            }).SingleOrDefault();

            if (player == null)
            {
                return HttpNotFound();
            }
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
                player.BirthDay = editPlayerDto.BirthDay;
                player.Info = editPlayerDto.Info;

                this.playersRepository.Update(player);

                return RedirectToAction("Index");
            }
            return View(editPlayerDto);
        }

        // GET: ManagePlayers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var player = this.playersRepository.All().SingleOrDefault(x => x.Id == id);

            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: ManagePlayers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var player = this.playersRepository.All().SingleOrDefault(x => x.Id == id);

            if (player == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.playersRepository.Delete(player);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.playersRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
