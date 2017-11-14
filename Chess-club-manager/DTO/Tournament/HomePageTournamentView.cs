using System;
using System.Collections.Generic;
using Chess_club_manager.DataModel.Enum;

namespace Chess_club_manager.DTO.Tournament
{
    public class HomePageTournamentView
    {
        public List<HomePageTournament> LastTournaments { get; set; }
        public List<HomePageTournament> CurrentTournaments { get; set; }
        public List<HomePageTournament> FutureTournaments { get; set; }

    }

    public class HomePageTournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Finish { get; set; }
        public bool IsOfficial { get; set; }
        public bool IsPrivate { get; set; }
        public int MaxPlayersCount { get; set; }
        public TournamentType Format { get; set; }
    }
}