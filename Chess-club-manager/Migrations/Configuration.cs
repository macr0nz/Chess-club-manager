using Chess_club_manager.Repository;

namespace Chess_club_manager.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            
        }

        protected override void Seed(ApplicationDbContext context)
        {
           
        }
    }
}
