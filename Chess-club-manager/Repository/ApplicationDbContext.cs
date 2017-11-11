using System;
using System.Data.Entity;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Chess_club_manager.Repository
{
    public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //Identity fields by base class
        //[dbo].[AspNetRoles]
        //[dbo].[AspNetUserClaims]
        //[dbo].[AspNetUserLogins]
        //[dbo].[AspNetUserRoles]
        //[dbo].[AspNetUsers]

        public DbSet<Tournament> Tournaments { get; set; }
        
        public DbSet<News> News { get; set; }

        public DbSet<MailSettings> MailSettings { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


    }
}