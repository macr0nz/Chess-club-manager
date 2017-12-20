using Chess_club_manager.Models;

namespace Chess_club_manager.DTO.Tournament.Tour
{
    public class TourGameLightDto
    {
        public int Id { get; set; }
        public int TourId { get; set; }

        public string LeftPlayerId { get; set; }
        public string LeftPlayerName { get; set; }

        public string RightPlayerId { get; set; }
        public string RightPlayerName { get; set; }

        public GameResult Result { get; set; }
    }
}