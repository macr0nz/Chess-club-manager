using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Chess_club_manager.DTO.Tournament
{
    public class EditTournamentDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FinishDate { get; set; }
        [DataType(DataType.Time)]
        public DateTime? FinishTime { get; set; }
        public bool IsOfficial { get; set; }
        public int TimeControl { get; set; }
        public string Location { get; set; }
        public bool IsPrivate { get; set; }
        public int MaxPlayersCount { get; set; }

        [AllowHtml]
        public string Info { get; set; }
    }
}