using System;
using System.Collections.Generic;
using Chess_club_manager.Models;

namespace Chess_club_manager.DTO.Tournament.Tour
{
    public class TourDetailsDto
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public string TournamentName { get; set; }

        public int Number { get; set; }

        public ICollection<TourGame> Games { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime? CompletedDateTime { get; set; }

        public bool EditAccess { get; set; }

    }
    
}