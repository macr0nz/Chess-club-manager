using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Chess_club_manager.Startup))]
namespace Chess_club_manager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            
        }
    }
}
