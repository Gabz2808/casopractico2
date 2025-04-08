using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CAAP2.Startup))]
namespace CAAP2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
