using Microsoft.AspNet.Identity;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Persistence.Validators
{
    public class ApplicationUserValidator<TUser> : UserValidator<TUser>
        where TUser : ApplicationUser
    {
        public bool PhoneIsRequire { get; set; }
        private ApplicationUserManager<TUser> Manager { get; }
        public ApplicationUserValidator(ApplicationUserManager<TUser> manager) : base(manager)
        {
            Manager = manager;
        }

        public override async Task<IdentityResult> ValidateAsync(TUser item)
        {
            IdentityResult baseResult = await base.ValidateAsync(item);
            List<string> errors = new List<string>(baseResult.Errors);

            if (Manager != null)
            {
                var otherAccount = await Manager.FindByPhoneNumberUserManagerAsync(item.PhoneNumber);
                if (otherAccount != null && otherAccount.Id != item.Id)
                {
                    string errorMsg = "Phone Number '" + item.PhoneNumber + "' is already taken.";
                    errors.Add(errorMsg);
                }


            }
            return errors.Any()
                ? IdentityResult.Failed(errors.ToArray())
                : IdentityResult.Success;
        }

    }
}