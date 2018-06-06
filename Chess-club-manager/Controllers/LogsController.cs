using System.Linq;
using System.Web.Mvc;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Repository;
using Chess_club_manager.Filters;
using Chess_club_manager.Repository;

namespace Chess_club_manager.Controllers
{
    [Culture]
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

        //public async Task Seed()
        //{
        //    //var context = new ApplicationDbContext();

        //    //context.News.Add(new News()
        //    //{
        //    //    Content = "try to seed",
        //    //    CreatedDate = DateTime.Now,
        //    //    Title = "seed test"
        //    //});

        //    //context.SaveChanges();

        //    var _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        //    var user = new ApplicationUser
        //    {
        //        Title = "Admin",
        //        BirthDay = DateTime.Now.AddDays(-100),
        //        FirstName = "Lord",
        //        LastName = "Stark",
        //        UserName = "admin",
        //        Email = "admin@this.com"
        //    };

        //    var result = await _userManager.CreateAsync(user, "qwe123");

        //    await _userManager.AddToRoleAsync(user.Id, "admin");
        //    await _userManager.AddToRoleAsync(user.Id, "user");

        //    Response.Write("success seed");
        //}

    }
}