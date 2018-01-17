using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Chess_club_manager.BackgroundJobs;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Enum;
using Chess_club_manager.Repository;

namespace Chess_club_manager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //tournament start background job 
            TournamentStartSheduler.Start();

            using (var unit = new ChessClubManagerUnitOfWork())
            {
                unit.LogsRepository.Add(new Log
                {
                    Type = LogType.Info,
                    Message = $"Aplication Started at {DateTime.Now}"
                });
            }
        }
    }
}
