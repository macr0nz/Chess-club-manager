using System;
using System.Collections.Generic;
using Chess_club_manager.DataModel.Entity;

namespace Chess_club_manager.Models
{
    public class TournamentTour : Entity
    {
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        public int Number { get; set; }

        public ICollection<TourGame> Games { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime? CompletedDateTime { get; set; }

    }
}