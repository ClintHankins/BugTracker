using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BugTracker1.Startup))]
namespace BugTracker1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
