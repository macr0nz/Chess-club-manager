using System;
using System.ComponentModel.DataAnnotations;

namespace Chess_club_manager.DTO.Players
{
    public class EditPlayerDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }

        [DataType(DataType.MultilineText)]
        public string Info { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public bool IsModerator { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsUser { get; set; }
    }
}