using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BugTracker_Phoenix.Startup))]
namespace BugTracker_Phoenix
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
