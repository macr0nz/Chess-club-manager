using System.Collections.Generic;

namespace Chess_club_manager.DTO.Tournament.Tour
{
    public class TourEditDto
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public int TournamentId { get; set; }
        public string TournamentName { get; set; }

        public ICollection<TourGameLightDto> Games { get; set; }
    }
}