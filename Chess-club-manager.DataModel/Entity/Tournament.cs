using System;
using Chess_club_manager.DataModel.Enum;

namespace Chess_club_manager.DataModel.Entity
{
    public class Tournament : Entity
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public TournamentType Type { get; set; }

        public Arbitrator Arbitrator { get; set; }

        public bool TimeControll { get; set; }
        public string Info { get; set; }

        public TournamentState TournamentState { get; set; }

        public Player FirstPlayer { get; set; }
        public Player SecondPlayer { get; set; }
        
    }
}