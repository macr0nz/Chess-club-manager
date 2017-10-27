using System;
using System.Data.Entity;
using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Chess_club_manager.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }


        public DbSet<Player> Players { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Arbitrator> Arbitrators { get; set; }
        public DbSet<News> News { get; set; }
            

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}