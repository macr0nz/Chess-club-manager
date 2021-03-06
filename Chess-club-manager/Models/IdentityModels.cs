﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Chess_club_manager.Models
{
    public class ApplicationUser : IdentityUser
    {
        //field username by dafault
        //field email by dafault
        //field phoneNumber by dafault

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Title { get; set; }

        //public DateTime? LastLogin { get; set; }

        public int CurrentRating { get; set; }
        public string Info { get; set; }
        public string ImagePath { get; set; }

        public ICollection<Tournament> CreatedTournaments { get; set; }
        public ICollection<Tournament> PlayedTournaments { get; set; }
        public ICollection<Tournament> ArbittedTournaments { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            return userIdentity;
        }

        public string GetUserRolesString()
        {
            var rolesBuilder = new StringBuilder();

            foreach (var role in this.Roles)
            {
                rolesBuilder.Append(role.RoleId);
                rolesBuilder.Append(", ");
            }

            return rolesBuilder.ToString().TrimEnd(',');
        }
    }

    
}