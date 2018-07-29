using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentSystem.Clients.Mvc.Startup))]
namespace StudentSystem.Clients.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
