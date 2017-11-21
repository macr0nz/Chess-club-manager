using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chess_club_manager.DTO.Tournament
{
    public class ManageTournamentPlayersViewDto
    {
        public int TournamentId { get; set; }
        public string TournamentName { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Finish { get; set; }
        public bool IsOfficial { get; set; }
        public bool IsPrivate { get; set; }

        public IEnumerable<PlayerLightDto> Players { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }

        public string CreatorName { get; set; }
        public string CreatorId { get; set; }
    }

    public class PlayerLightDto
    {
        public string Id { get; set; }
        //avatar link
        public string FirstNameLastName { get; set; }
        public string Title { get; set; }
        public int CurrentRating { get; set; }
    }
}