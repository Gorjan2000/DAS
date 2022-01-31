using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Domasna3.Startup))]
namespace Domasna3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
