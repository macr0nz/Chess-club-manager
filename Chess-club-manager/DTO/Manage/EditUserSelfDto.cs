using System;
using System.ComponentModel.DataAnnotations;

namespace Chess_club_manager.DTO.Manage
{
    public class EditUserSelfDto
    {
        public string Id { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }

        
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}