using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(Inscoo.Startup))]
namespace Inscoo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {          
            ConfigureAuth(app);
        }
    }
}