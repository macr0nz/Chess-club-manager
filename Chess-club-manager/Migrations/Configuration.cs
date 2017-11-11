using Chess_club_manager.DataModel.Entity;
using Chess_club_manager.Repository;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Chess_club_manager.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(ApplicationDbContext context)
        {
            context.Roles.AddOrUpdate(x => x.Id,
                new IdentityRole {Id = "adminRole", Name = "admin"},
                new IdentityRole {Id = "moderatorRole", Name = "moderator"},
                new IdentityRole {Id = "userRole", Name = "user"}
                );

            context.MailSettings.AddOrUpdate(x => x.Id,
                new MailSettings
                {
                    Id = 1,
                    MailFrom = "chessclubmanagerinfo@yandex.ru",
                    MailPassword = "qwerty1234",
                    EnableSsl = true,
                    MailPort = 587,
                    MailServer = "smtp.yandex.ru"
                });
        }
    }
}
