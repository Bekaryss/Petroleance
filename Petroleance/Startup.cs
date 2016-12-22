using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Petroleance.Startup))]
namespace Petroleance
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
