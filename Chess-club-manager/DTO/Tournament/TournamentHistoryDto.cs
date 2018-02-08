namespace Chess_club_manager.DTO.Tournament
{
    public class TournamentHistoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsOfficial { get; set; }
        public string Location { get; set; }
        public bool IsPrivate { get; set; }
        

        //state
        public bool IsCompleted { get; set; }
    }
}