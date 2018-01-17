using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Chess_club_manager.Helpers
{
    public static class DateTimeHelper
    {
        
        public static string ToUIformat(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }

            var dateTimeNotNull = dateTime.Value;

            return dateTimeNotNull.ToString("dd-MMM-yyyy hh:mm");
        }

        public static string ToUIformat(this DateTime dateTime)
        {
            return dateTime.ToString("dd-MMM-yyyy hh:mm");
        }

        public static string ToShortDateUI(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }

            var dateTimeNotNull = dateTime.Value;

            return dateTimeNotNull.ToString("dd-MMM-yyyy");
        }
        
    }
}