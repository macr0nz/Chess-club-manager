using Chess_club_manager.DataModel.Entity;

namespace Chess_club_manager.Models
{
    public class TourGame : Entity
    {
        public int TourId { get; set; }
        public TournamentTour Tour { get; set; }

        public string LeftPlayerId { get; set; }
        public ApplicationUser LeftPlayer { get; set; }

        public string RightPlayerId { get; set; }
        public ApplicationUser RightPlayer { get; set; }

        public GameResult Result { get; set; }
    }

    public enum GameResult
    {
        LeftWin = 1,
        RightWin = 2,
        Draw = 3
    }
}