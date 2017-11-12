using System;

namespace Chess_club_manager.DTO.Tournament
{
    public class TournamentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Finish { get; set; }
        public bool IsOfficial { get; set; }
        public int TimeControl { get; set; }
        public string Location { get; set; }
        public bool IsPrivate { get; set; }
        public int MaxPlayersCount { get; set; }
        
       //special "8/10 pl." -example
        public int AssignedPlayersCount { get; set; }

        //state
        public bool IsStarted { get; set; }
        public bool IsCompleted { get; set; }
    }
}