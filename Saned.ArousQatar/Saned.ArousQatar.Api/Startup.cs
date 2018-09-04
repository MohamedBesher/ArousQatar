using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Saned.ArousQatar.Api;
using Saned.ArousQatar.Api.Providers;
using System;
using System.EnterpriseServices;
using System.Web.Http;
using Saned.ArousQatar.Data.Persistence.Infrastructure;

// the “assembly” attribute which states which class to fire on start-up
[assembly: OwinStartup(typeof(Startup))]
namespace Saned.ArousQatar.Api
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"> this parameter will be supplied by the host at run-time. 
        /// This “app” parameter is an interface which will be used to compose the application for our Owin server.</param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
            HttpConfiguration config = new HttpConfiguration();
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
        public void ConfigureOAuth(IAppBuilder app)
        {

            IDbFactory _dbFactory = new DbFactory();
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(100000),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
            googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "11459241082-e98ko6sbncbv5ij8k6untl7503hb69jt.apps.googleusercontent.com",
                ClientSecret = "QfmDtCqrTSTJgl1kPHGUXAP5",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(googleAuthOptions);

            //Configure Facebook External Login
            facebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "1452466754765014",
                AppSecret = "e126c89830bce8fca50422324dc6ba2e",

                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(facebookAuthOptions);

        }
    }
}