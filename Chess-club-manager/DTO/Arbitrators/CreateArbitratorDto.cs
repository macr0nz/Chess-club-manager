using System.ComponentModel.DataAnnotations;

namespace Chess_club_manager.DTO.Arbitrators
{
    public class CreateArbitratorDto
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }
}