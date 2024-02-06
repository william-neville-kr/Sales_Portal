using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhysicianPortal.Web.Startup))]
namespace PhysicianPortal.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.MapSignalR();
            GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
}
