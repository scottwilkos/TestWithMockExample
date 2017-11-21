using Microsoft.Owin;

using MockTestExample.Website;

[assembly: OwinStartup(typeof(Startup))]
namespace MockTestExample.Website
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
