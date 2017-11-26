using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess_club_manager.DataModel.Enum;

namespace Chess_club_manager.DataModel.Entity
{
    public class Log : Entity
    {
        public LogType Type { get; set; }
        public string Message { get; set; }
    }
}
