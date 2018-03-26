using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "GameResultDefault", ResourceType = typeof(Resources.Resource))]
        Default = 0,

        [Display(Name = "1 - 0")]
        LeftWin = 1,

        [Display(Name = "0 - 1")]
        RightWin = 2,
        
        [Display(Name = "GameResultDraw", ResourceType = typeof(Resources.Resource))]
        Draw = 3
    }
}