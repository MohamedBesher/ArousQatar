using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Tools;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using Saned.ArousQatar.Data.Persistence.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Api.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {


        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {


            var singleOrDefault = context.Parameters.Where(f => f.Key == "role").Select(f => f.Value).SingleOrDefault();
            if (singleOrDefault != null)
            {
                string uid = singleOrDefault[0];
                context.OwinContext.Set<string>("role", uid);
            }


            string clientId;
            string clientSecret;
            Client client;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                //context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            using (AuthRepository repo = new AuthRepository())
            {
                client = repo.FindClient(context.ClientId);
            }

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
          var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";
            // var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "http://localhost:12893";
          //var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "http://admin.arousqatar.com";
            // var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "http://arousqataradmin.saned-projects.com";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });


            ApplicationUser user;
            using (AuthRepository repo = new AuthRepository())
            {
                user = await repo.FindUser(context.UserName, context.Password);
                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                else
                {
                    if (CheckUserValid(context, user)) return;
                }


            }

            var identity = SetClaimsIdentity(context, user);


            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "as:client_id", context.ClientId ?? string.Empty
                    },
                    {
                        "userName", context.UserName
                    },
                   
                     {
                        "PhotoUrl", user.PhotoUrl?? string.Empty
                    }
                });


            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);


        }

        private static bool CheckUserValid(OAuthGrantResourceOwnerCredentialsContext context, ApplicationUser user)
        {
            bool isDelete = false;
            //bool isApprove = false;
            if (user.IsDeleted != null) isDelete = user.IsDeleted.Value;
            var isEmailConfirme = user.EmailConfirmed;
            //if (user.IsApprove != null) isApprove = user.IsApprove.Value;
            if (!isEmailConfirme || isDelete)
            {
                if (isDelete)
                    context.SetError("invalid_grant", "1-User are Arhieve");
                if (!isEmailConfirme)
                    context.SetError("invalid_grant", "2-Email Need To Confirm");
                //if (!isApprove)
                //    context.SetError("invalid_grant", "3-Wating To Approve");
                return true;
            }
            return false;
        }

        private static ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, ApplicationUser user)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            using (AuthRepository repo = new AuthRepository())
            {
                var userRoles = repo.GetRoles(user.Id);
                foreach (var role in userRoles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }
            }
            return identity;
        }
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }


        //public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        //{
        //    if(context.IsTokenEndpoint && context.Request.Method == "OPTIONS")
        //    {
        //        context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
        //        context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "authorization" });
        //        context.RequestCompleted();
        //        return Task.FromResult(0);
        //    }

        //    return base.MatchEndpoint(context);

          
        //}
    }
}