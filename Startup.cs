using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AphidBT.Startup))]
namespace AphidBT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
