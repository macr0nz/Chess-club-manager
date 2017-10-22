using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chess_club_manager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //top 5 players orderby rating
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "ANS Chess Club Manager description page.";

            return View();
        }

        
    }
}