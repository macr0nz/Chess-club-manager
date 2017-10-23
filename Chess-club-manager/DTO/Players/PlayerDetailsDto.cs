using System;
using System.Collections.Generic;
using Chess_club_manager.DataModel.Entity;

namespace Chess_club_manager.DTO.Players
{
    public class PlayerDetailsDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Title { get; set; }
        public int CurrentRating { get; set; }
        public string Info { get; set; }

        public ICollection<Tournament> Tournaments { get; set; }
    }
}