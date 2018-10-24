using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ADIMS.Startup))]
namespace ADIMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
