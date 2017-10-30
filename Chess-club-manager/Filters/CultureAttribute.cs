using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Chess_club_manager.Filters
{
    public class CultureAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string cultureName = null;
             
            var cultureCookie = filterContext.HttpContext.Request.Cookies["lang"];

            cultureName = cultureCookie != null ? cultureCookie.Value : "ru";
            
            var cultures = new List<string>() { "ru", "en", "md" };

            if (!cultures.Contains(cultureName))
            {
                cultureName = "ru";
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
    }
}