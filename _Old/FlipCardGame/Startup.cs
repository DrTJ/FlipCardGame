using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FlipCardGame.Startup))]
namespace FlipCardGame
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
