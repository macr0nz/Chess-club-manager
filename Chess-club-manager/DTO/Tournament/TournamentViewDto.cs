using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Chess_club_manager.DataModel.Enum;
using Chess_club_manager.Models;

namespace Chess_club_manager.DTO.Tournament
{
    public class TournamentViewDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Finish { get; set; }
        public bool IsOfficial { get; set; }
        public int TimeControl { get; set; }
        public string Location { get; set; }
        public bool IsPrivate { get; set; }
        public int MaxPlayersCount { get; set; }
        public int MaxToursCount { get; set; }
        public string Info { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatorId { get; set; }
        public string CreatorName { get; set; }

        public TournamentType Format { get; set; }

        public ICollection<TournamentViewUserLightDto> Arbitrators { get; set; }

        

        public ICollection<TournamentViewPlayerLightDto> Players { get; set; }

        //state
        public bool IsStarted { get; set; }
        public bool IsCompleted { get; set; }
        //public TournamentTable TournamentTable { get; set; }
        public List<TourGame> TourGames { get; set; }

        //stage?
        public bool AccessPassed { get; set; }


        //tours
        public ICollection<TournamentTour> Tours { get; set; }

    }

    public class TournamentViewUserLightDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstNameLastName { get; set; }
    }

    public class TournamentViewPlayerLightDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstNameLastName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Title { get; set; }

        public int CurrentRating { get; set; }
    }


}