using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http;
using System.Web.Http.Filters;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Persistence.Repositories;

namespace Saned.ArousQatar.Api.Attributes
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {

        
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);

            }
            else
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            AuthRepository repo = new AuthRepository();

            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated )
            {
                string name = actionContext.RequestContext.Principal.Identity.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    ApplicationUser u=  repo.FindUserByName(name);
                    if (u == null)
                    {
                    actionContext.ModelState.AddModelError("", "User Not found");
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation,"DeletedUser");
                    // base.HandleUnauthorizedRequest(actionContext); 
                    }
                   
                    
                }
                else               
                    base.HandleUnauthorizedRequest(actionContext);
                
            }
            else
            {
                base.HandleUnauthorizedRequest(actionContext);

            }



        }
    }





  
}