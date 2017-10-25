using System;
using System.Collections.Generic;
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
    public class ArbitratorsController : Controller
    {
        private IRepository<Arbitrator> arbitratorsRepository;

        public ArbitratorsController()
        {
            arbitratorsRepository = new ChessClubManagerRepository<Arbitrator>();
        }

        public ActionResult Index()
        {
            var allArbitrators = arbitratorsRepository.All().ToList();

            return View(allArbitrators);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var arbitrator = this.arbitratorsRepository.All().Where(p => p.Id == id).Select(x => new ViewArbitratorDto
            {
                Id = x.Id,
                LastName = x.LastName,
                FirstName = x.FirstName,
                Title = x.Title,
            }).SingleOrDefault();

            if (arbitrator == null)
            {
                return HttpNotFound();
            }
            return View(arbitrator);
        }
    }
}