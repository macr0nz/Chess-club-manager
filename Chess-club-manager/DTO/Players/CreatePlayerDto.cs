using System;
using System.ComponentModel.DataAnnotations;

namespace Chess_club_manager.DTO.Players
{
    public class CreatePlayerDto
    {
        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }


        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "BirthDay")]
        public DateTime? BirthDay { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Info")]
        [DataType(DataType.MultilineText)]
        public string Info { get; set; }
    }
}