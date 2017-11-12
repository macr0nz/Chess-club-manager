using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Chess_club_manager.DTO.Players
{
    public class PlayerDetailsDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Title { get; set; }
        public int CurrentRating { get; set; }
        public string Info { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        

        public ICollection<IdentityUserRole> Roles { get; set; }

        public ICollection<Models.Tournament> Tournaments { get; set; }
    }
}