﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Chess_club_manager.DTO.Players
{
    public class CreatePlayerDto
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        //public string Title { get; set; }
        //public int CurrentRating { get; set; }
        public string Info { get; set; }
    }
}