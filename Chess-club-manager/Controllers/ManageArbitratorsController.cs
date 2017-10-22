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
using Chess_club_manager.DTO.Arbitrators;
using Chess_club_manager.Repository;

namespace Chess_club_manager.Controllers
{
    [Authorize]
    public class ManageArbitratorsController : Controller
    {
        private IRepository<Arbitrator> arbitratorsRepository;

        public ManageArbitratorsController()
        {
            this.arbitratorsRepository = new ChessClubManagerRepository<Arbitrator>();
        }

        
        public ActionResult Index()
        {
            var allArbitrators = this.arbitratorsRepository.All().ToList();

            return View(allArbitrators);
        }

        // GET: ManageArbitrators/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var arbitrator = this.arbitratorsRepository.All().SingleOrDefault(p => p.Id == id);

            if (arbitrator == null)
            {
                return HttpNotFound();
            }
            return View(arbitrator);
        }

        // GET: ManageArbitrators/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageArbitrators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateArbitratorDto createArbitratorDto)
        {
            if (ModelState.IsValid)
            {
                this.arbitratorsRepository.Add(new Arbitrator
                {
                    FirstName = createArbitratorDto.FirstName,
                    LastName = createArbitratorDto.LastName,
                    Title = createArbitratorDto.Title
                });
                
                return RedirectToAction("Index");
            }

            return View(createArbitratorDto);
        }

        // GET: ManageArbitrators/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var editArbitrator = this.arbitratorsRepository.All().Where(p => p.Id == id).Select(x => new EditArbitratorDto
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Title = x.Title,
                Id = x.Id
            }).SingleOrDefault();

            if (editArbitrator == null)
            {
                return HttpNotFound();
            }
            return View(editArbitrator);
        }

        // POST: ManageArbitrators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditArbitratorDto editArbitratorDto)
        {
            if (ModelState.IsValid)
            {
                var arbitrator = this.arbitratorsRepository.All().SingleOrDefault(p => p.Id == editArbitratorDto.Id);

                if (arbitrator == null) return View(editArbitratorDto);

                arbitrator.FirstName = editArbitratorDto.FirstName;
                arbitrator.LastName = editArbitratorDto.LastName;
                arbitrator.Title = editArbitratorDto.Title;

                this.arbitratorsRepository.Update(arbitrator);

                return RedirectToAction("Index");
            }
            return View(editArbitratorDto);
        }

        // GET: ManageArbitrators/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var arbitrator = this.arbitratorsRepository.All().SingleOrDefault(p => p.Id == id);

            if (arbitrator == null)
            {
                return HttpNotFound();
            }
            return View(arbitrator);
        }

        // POST: ManageArbitrators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var arbitrator = this.arbitratorsRepository.All().SingleOrDefault(p => p.Id == id);
            
            this.arbitratorsRepository.Delete(arbitrator);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.arbitratorsRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
