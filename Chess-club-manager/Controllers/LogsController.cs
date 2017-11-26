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
    [Authorize(Roles = "admin, moderator")]
    public class LogsController : Controller
    {
        private IRepository<Log> _logsRepository;

        public LogsController()
        {
            this._logsRepository = new ChessClubManagerRepository<Log>();
        }

        public ActionResult Index()
        {
            var allLogs = this._logsRepository
                .All()
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            return View(allLogs);
        }
    }
}