using System.Collections.Generic;

namespace Chess_club_manager.Models
{
    public class TournamentTable
    {
        public List<TournamentTableRow> Rows { get; set; }
    }

    public class TournamentTableRow
    {
        public string UserName { get; set; }
        public int Rating { get; set; }
        public int Points { get; set; }
        public int Place { get; set; }
        public List<GameResult> Results { get; set; }

    }
}