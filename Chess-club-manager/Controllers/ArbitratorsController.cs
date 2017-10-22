using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Repository;
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
    }
}