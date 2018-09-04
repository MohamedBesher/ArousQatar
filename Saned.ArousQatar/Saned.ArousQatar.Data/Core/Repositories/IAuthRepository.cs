using Microsoft.AspNet.Identity;
using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IAuthRepository
    {
        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);
        Task<bool> AddRefreshToken(RefreshToken token);
        Task<IdentityResult> ChangePassword(string userId, string oldPassword, string newPassword);
        Task<IdentityResult> ConfirmEmail(string userId, string code);
        Task<IdentityResult> CreateAsync(ApplicationUser user);
        void Dispose();
        Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo);
        Client FindClient(string clientId);
        Task<RefreshToken> FindRefreshToken(string refreshTokenId);
        Task<ApplicationUser> FindUser(string email);
        Task<ApplicationUser> FindUser(string userName, string password);
        Task ForgetPassword(string email);
        List<RefreshToken> GetAllRefreshTokens();
        IList<string> GetRoles(string userId);
        Task<bool> IsEmailConfirme(string userId);
        Task<bool> IsUserArchieve(string userId);
        Task<IdentityResult> RegisterAgent(string name, string phone, string email, string userName, string password);
        Task<IdentityResult> RegisterUser(string userName, string password, string role, string PhoneNumber, string email);
        Task<bool> RemoveRefreshToken(string refreshTokenId);
        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);
        Task<IdentityResult> ResetPassword(string userId = "", string code = "", string newPassword = "");
    }
}