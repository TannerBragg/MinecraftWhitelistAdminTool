using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Whitelist_Administration_Tool.Startup))]
namespace Whitelist_Administration_Tool
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
