using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(thingslineWeb.Startup))]
namespace thingslineWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
