//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Chess_club_manager.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class TourGames
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int Result { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string LeftPlayerId { get; set; }
        public string RightPlayerId { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual AspNetUsers AspNetUsers1 { get; set; }
        public virtual TournamentTours TournamentTours { get; set; }
    }
}