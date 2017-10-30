using System;

namespace Chess_club_manager.DTO.Players
{
    public class EditPlayerDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Info { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}