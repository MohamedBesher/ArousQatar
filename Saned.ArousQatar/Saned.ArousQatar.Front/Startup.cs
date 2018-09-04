using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Saned.ArousQatar.Front.Startup))]
namespace Saned.ArousQatar.Front
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
