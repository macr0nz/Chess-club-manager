using System;
using System.Collections.Generic;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.DataModel.Enum;

namespace Chess_club_manager.Models
{
    public sealed class Tournament : Entity
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Finish { get; set; }
        public bool IsOfficial { get; set; }
        public int TimeControl { get; set; }
        public string Location { get; set; }
        public bool IsPrivate { get; set; }
        public int MaxPlayersCount { get; set; }
        public string Info { get; set; }

        public int MaxToursCount { get; set; }

        public TournamentType Format { get; set; }

        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }

        public ICollection<ApplicationUser> Players { get; set; }
        public ICollection<ApplicationUser> Arbitrators { get; set; }

        
        //state
        public bool IsStarted { get; set; }
        public bool IsCompleted { get; set; }

        //results
        public ICollection<TournamentTour> Tours { get; set; }
    }
}