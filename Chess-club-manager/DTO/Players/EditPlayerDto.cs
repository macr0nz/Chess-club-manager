﻿using System;

namespace Chess_club_manager.DTO.Players
{
    public class EditPlayerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        //public string Title { get; set; }
        //public int CurrentRating { get; set; }
        public string Info { get; set; }
    }
}