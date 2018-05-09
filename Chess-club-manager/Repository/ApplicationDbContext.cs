using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
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

        public DbSet<TournamentTour> TournamentTours { get; set; }

        public DbSet<TourGame> TourGames { get; set; }

        
        public DbSet<News> News { get; set; }

        public DbSet<MailSettings> MailSettings { get; set; }

        public DbSet<Log> Logs { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //debug write all SQL commands
            Database.Log += str => { Debug.Write(str); };
        }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(x => x.CreatedTournaments)
                .WithRequired(x => x.Creator).HasForeignKey(x => x.CreatorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(user => user.PlayedTournaments)
                .WithMany(trn => trn.Players)
                .Map(m =>
                {
                    m.ToTable("UsersPlayedTournaments");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("TournamentId");
                });


            modelBuilder.Entity<ApplicationUser>()
                .HasMany(user => user.ArbittedTournaments)
                .WithMany(trn => trn.Arbitrators)
                .Map(m =>
                {
                    m.ToTable("UsersArbittedTournaments");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("TournamentId");
                });


            base.OnModelCreating(modelBuilder);
        }

    }
}