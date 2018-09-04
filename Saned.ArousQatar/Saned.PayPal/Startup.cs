using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Saned.PayPal.Startup))]
namespace Saned.PayPal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
