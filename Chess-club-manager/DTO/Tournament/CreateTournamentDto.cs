using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chess_club_manager.DataModel.Enum;

namespace Chess_club_manager.DTO.Tournament
{
    public class CreateTournamentDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; } 

        [DataType(DataType.Date)]
        public DateTime? FinishDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime? FinishTime { get; set; }

        public bool IsOfficial { get; set; } = false;
        public int TimeControl { get; set; } = 90;
        public string Location { get; set; }
        public bool IsPrivate { get; set; } = false;
        public int MaxPlayersCount { get; set; }
        
        [AllowHtml]
        public string Info { get; set; }

        [Required]
        public TournamentType Format { get; set; } = TournamentType.Round;
    }
}