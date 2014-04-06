using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReactiveApp.Web.Startup))]
namespace ReactiveApp.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
