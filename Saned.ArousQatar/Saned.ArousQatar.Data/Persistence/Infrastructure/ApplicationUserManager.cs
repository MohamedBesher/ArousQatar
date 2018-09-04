using Microsoft.AspNet.Identity;
using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Persistence.Services;
using Saned.ArousQatar.Data.Persistence.Validators;
using System;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Persistence.Infrastructure
{
    public class ApplicationUserManager<TUser> : UserManager<TUser> where TUser : ApplicationUser
    {

        public ApplicationUserManager() : base(new ApplicationUserStore<TUser>())
        {


        }

        public ApplicationUserManager(IUserCustomStore<TUser> store)
               : base(store)
        {
            UserValidator = new ApplicationUserValidator<TUser>(this)
            {
                //RequireUniqueEmail = true ,
            };

            this.EmailService = new EmailService();
            //var provider = new DpapiDataProtectionProvider("AppName");
            //this.UserTokenProvider = new DataProtectorTokenProvider<TUser, string>(provider.Create("ASP.NET Identity")); ; //new DataProtectorTokenProvider<ApplicationUser, long>(provider.Create("PasswordReset"));

        }
        public virtual Task<ApplicationUser> FindByPhoneNumberUserManagerAsync(string phoneNumber)
        {
            IUserCustomStore<ApplicationUser> userCustomStore = Store as IUserCustomStore<ApplicationUser>;
            //if ( phoneNumber == null ) {
            //    throw new ArgumentNullException( );
            //}
            return userCustomStore?.FindByPhoneNumberAsync(phoneNumber);
        }

    }
}

