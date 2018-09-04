using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Persistence.Providers;

namespace Saned.ArousQatar.Data.Persistence.Infrastructure
{
    public class ApplicationUserManagerImpl : ApplicationUserManager<ApplicationUser>
    {
        public ApplicationUserManagerImpl() : base(new ApplicationUserStoreImpl())
        {
            this.UserTokenProvider = new ApplicationTokenProvider<ApplicationUser>();

        }


    }
}