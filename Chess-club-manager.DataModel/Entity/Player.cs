using System;
using System.Collections.Generic;

namespace Chess_club_manager.DataModel.Entity
{
    public class Player : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay{ get; set; }
        public string Title { get; set; }
        public int CurrentRating { get; set; }
        public string Info { get; set; }

        public ICollection<Tournament> Tournaments { get; set; }

    }
}