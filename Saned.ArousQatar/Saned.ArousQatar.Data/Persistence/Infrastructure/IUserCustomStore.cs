using Microsoft.AspNet.Identity;
using Saned.ArousQatar.Data.Core.Models;
using System;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Persistence.Infrastructure
{
    public interface IUserCustomStore<TUser> : IUserStore<TUser>, IDisposable where TUser : ApplicationUser, IUser<string>
    {
        Task<TUser> FindByPhoneNumberAsync(string phoneNumber);

    }
}