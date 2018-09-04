using Microsoft.AspNet.Identity;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Core.Dtos;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public enum EmailType
    {
        EmailConfirmation = 1,
        ForgetPassword = 2,
    }

    public class AuthRepository : IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        private ApplicationDbContext _context;

        private readonly ApplicationUserManagerImpl _userManager;

        public AuthRepository()
        {
            _context = new ApplicationDbContext();
            _userManager = new ApplicationUserManagerImpl();
            _unitOfWork = new UnitOfWork(_context);
        }

        #region user

        public async Task<IdentityResult> RegisterUser(RegisterUserData data)
        {
            var user = GetApplicationUser(
                data.Name,
                data.PhoneNumber,
                data.Email,
                data.UserName,
                "User");

            

            var result = await _userManager.CreateAsync(user, data.Password);
            if (result.Succeeded)
            {
                await AddRoleToUser(user.Id, "User");
                await SendEmailConfirmation(user);
            }
            return result;
        }


        public async Task ForgetPassword(string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            await SendPasswordResetToken(user);

        }
        public async Task<IdentityResult> ResetPassword(string userId = "", string code = "", string newPassword = "")
        {

            IdentityResult result = await _userManager.ResetPasswordAsync(userId, code, newPassword);
            return result;
        }
        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            ApplicationUser user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public async Task<bool> IsEmailConfirme(string userId)
        {
            return await _userManager.IsEmailConfirmedAsync(userId);
        }
        public async Task<bool> IsUserArchieve(string userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
            return user.IsDeleted != null && user.IsDeleted.Value;
        }

        public async Task<ApplicationUser> FindUser(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<IdentityResult> ConfirmEmail(string userId, string code)
        {
            IdentityResult result = await this._userManager.ConfirmEmailAsync(userId, code);
            return result;
        }

        public IList<string> GetRoles(string userId)
        {
            IList<string> lst = _userManager.GetRoles(userId);
            return lst;
        }

        private EmailSetting GetEmailMessage(string toName, string code, string messageTemplate)
        {
            EmailSetting emailSettings = _unitOfWork.EmailSetting.GetEmailSetting(messageTemplate);
            //List<int> types=_unitOfWork.ContactTypes.GetAll().Select(u=>u.Id).ToList();
            List<string> emaillist = _unitOfWork.ContactInformations.GetAllWithTypes(1);
            List<string> phonelist = _unitOfWork.ContactInformations.GetAllWithTypes(2);
            List<string> placelist = _unitOfWork.ContactInformations.GetAllWithTypes(3);
            string email = string.Join(",", emaillist);
            string phone = string.Join(",", phonelist);
            string place = string.Join(",", placelist);
           
            emailSettings.MessageBodyAr = emailSettings.MessageBodyAr.Replace("@FullName", toName);
            emailSettings.MessageBodyAr = emailSettings.MessageBodyAr.Replace("@code",  code);
            emailSettings.MessageBodyAr = emailSettings.MessageBodyAr.Replace("@email", email);
            emailSettings.MessageBodyAr = emailSettings.MessageBodyAr.Replace("@phone", phone);
            emailSettings.MessageBodyAr = emailSettings.MessageBodyAr.Replace("@place", place);
            return emailSettings;
        }
        private ApplicationUser GetApplicationUser(string name, string phone, string email, string userName, string role = "user", bool isApprove = false, bool isSelfAdded = false, string status = "New")
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = userName,
                PhoneNumber = phone,
                Email = email,
                Name = name
            };
            return user;
        }

        private async Task AddRoleToUser(string userId, string role)
        {
            await _userManager.AddToRoleAsync(userId, role);
        }

        private async Task SendEmailConfirmation(ApplicationUser user)
        {

            var code = await GenerateToken(user.Id, EmailType.EmailConfirmation);
            await SendEmail(user, code, EmailType.EmailConfirmation.GetHashCode().ToString());
        }
        public async Task ReSendEmailConfirmation(ApplicationUser user)
        {

            var code = await GenerateToken(user.Id, EmailType.EmailConfirmation);
            await SendEmail(user, code, EmailType.EmailConfirmation.GetHashCode().ToString());
        }
        private async Task SendPasswordResetToken(ApplicationUser user)
        {
            var code = await GenerateToken(user.Id, EmailType.ForgetPassword);
            await SendEmail(user, code, EmailType.ForgetPassword.GetHashCode().ToString());
        }
        private async Task<string> GenerateToken(string userId, EmailType emailType)
        {
            switch (emailType)
            {
                case EmailType.EmailConfirmation:
                    //return await _userManager.GenerateEmailConfirmationTokenAsync(userId);
                    return await GenerateTokenNumber(userId);

                case EmailType.ForgetPassword:
                    //return await _userManager.GeneratePasswordResetTokenAsync(userId);
                    return await GenerateResetTokenNumber(userId);

                default:
                    return "";
            }

        }
        public async Task<string> GenerateTokenNumber(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            var generateRandomCode = GenerateRandomCode();
            user.ConfirmedEmailToken = generateRandomCode;
            await _userManager.UpdateAsync(user);
            return generateRandomCode;

        }

        public async Task<string> GenerateResetTokenNumber(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            var userResetPasswordlToken = GenerateRandomCode();
            user.ResetPasswordlToken = userResetPasswordlToken;
            await _userManager.UpdateAsync(user);
            return userResetPasswordlToken;

        }

        private static string GenerateRandomCode()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                GenerateRandomCode();
            }
            return r;
        }



        private async Task SendEmail(ApplicationUser user, string code, string messageTemplate)
        {
            EmailSetting emailMessage = GetEmailMessage(user.UserName, code, messageTemplate);
            await _userManager.SendEmailAsync(user.Id, messageTemplate, emailMessage.MessageBodyAr);
        }
        public async Task<IdentityResult> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            IdentityResult result = await _userManager.ChangePasswordAsync(userId, oldPassword, newPassword);
            return result;
        }


        #endregion

        #region token

        public Client FindClient(string clientId)
        {
            var client = _context.Clients.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken =
                _context.RefreshTokens
                    .SingleOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _context.RefreshTokens.Add(token);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _context.RefreshTokens.Remove(refreshToken);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _context.RefreshTokens.ToList();
        }

        #endregion

        #region Soical
        public async Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo)
        {
            ApplicationUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
          
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await AddRoleToUser(user.Id, "User");
            }

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }
        #endregion

        //public void Dispose()
        //{
        //    _context.Dispose();
        //    _userManager.Dispose();
        //    GC.SuppressFinalize(this);
        //}
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this._context == null)
            {
                return;
            }

            this._context.Dispose();
            this._userManager.Dispose();
            this._context = null;
            this._unitOfWork.Dispose();
        }

        public async Task<ApplicationUser> FindUserByUserName(string userName)
        {
            ApplicationUser user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == userName);
            return user;
        }

        public  ApplicationUser FindUserByName(string userName)
        {
            ApplicationUser user = _userManager.Users.SingleOrDefault(x => x.UserName == userName);
            return user;
        }
        public async Task<ApplicationUser> FindUserByUserId(string id)
        {
            ApplicationUser user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == id);
            return user;
        }


        public async Task<IdentityResult> EditImage(string picture, string id)
        {
            ApplicationUser user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == id);
            user.PhotoUrl = picture;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<ApplicationUser> FindUserById(string userId)
        {
            ApplicationUser user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
            return user;
        }
    }
    //public enum EmailType
    //{
    //    EmailConfirmation = 1,
    //    ForgetPassword = 2,
    //}

    //public class AuthRepository : IDisposable
    //{

    //    private IDbFactory _dbFactory { get; }
    //    private ApplicationDbContext _context;
    //    //private EmailSettingRepository EmailSetting { get; }
    //    private readonly ApplicationUserManagerImpl _userManager;

    //    public AuthRepository(IDbFactory dbFactory)
    //    {
    //        _dbFactory = dbFactory;
    //        _context = new ApplicationDbContext();
    //        _userManager = new ApplicationUserManagerImpl();
    //        //EmailSetting = new EmailSettingRepository(_dbFactory);

    //    }
    //    public AuthRepository()
    //    {
    //        _context = new ApplicationDbContext();
    //        _userManager = new ApplicationUserManagerImpl();

    //    }
    //    //public AuthRepository(IUnitOfWork unitOfWork)
    //    //{
    //    //    _context = new ApplicationDbContext();
    //    //    _userManager = new ApplicationUserManagerImpl();
    //    //    _unitOfWork = unitOfWork;

    //    //}

    //    #region user


    //    public async Task<IdentityResult> RegisterUser2(string userName, string password, string PhoneNumber, string email)
    //    {
    //        ApplicationUser user = new ApplicationUser
    //        {
    //            UserName = userName,
    //            PhoneNumber = PhoneNumber,
    //            Email = email
    //        };


    //        var result = await _userManager.CreateAsync(user, password);
    //        if (result.Succeeded)
    //        {
    //            await _userManager.AddToRoleAsync(user.Id, "User");
    //            await SendEmailConfirmation(user);
    //        }
    //        return result;
    //    }

    //    public async Task<IdentityResult> RegisterAgent(string name, string phone, string email, string userName, string password)
    //    {

    //        var user = GetApplicationUser(name, phone, email, userName, "Agent", false, true);

    //        var result = await _userManager.CreateAsync(user, password);

    //        if (result.Succeeded)
    //        {
    //            await AddRoleToUser(user);
    //            await SendEmailConfirmation(user);
    //        }

    //        return result;
    //    }

    //    public async Task ForgetPassword(string email)
    //    {

    //        var user = await _userManager.FindByEmailAsync(email);
    //        await SendPasswordResetToken(user);

    //    }
    //    public async Task<IdentityResult> ResetPassword(string userId = "", string code = "", string newPassword = "")
    //    {

    //        IdentityResult result = await _userManager.ResetPasswordAsync(userId, code, newPassword);
    //        return result;
    //    }
    //    public async Task<ApplicationUser> FindUser(string userName, string password)
    //    {
    //        ApplicationUser user = await _userManager.FindAsync(userName, password);
    //        return user;
    //    }

    //    public async Task<bool> IsEmailConfirme(string userId)
    //    {
    //        return await _userManager.IsEmailConfirmedAsync(userId);
    //    }
    //    public async Task<bool> IsUserArchieve(string userId)
    //    {
    //        var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
    //        return user.IsDeleted != null && user.IsDeleted.Value;
    //    }

    //    public async Task<ApplicationUser> FindUser(string email)
    //    {
    //        ApplicationUser user = await _userManager.FindByEmailAsync(email);
    //        return user;
    //    }

    //    public async Task<IdentityResult> ConfirmEmail(string userId, string code)
    //    {
    //        IdentityResult result = await this._userManager.ConfirmEmailAsync(userId, code);
    //        return result;
    //    }

    //    public IList<string> GetRoles(string userId)
    //    {
    //        IList<string> lst = _userManager.GetRoles(userId);
    //        return lst;
    //    }

    //    private EmailSetting GetEmailMessage(string toName, string code, string messageTemplate)
    //    {
    //        EmailSetting emailSettings = EmailSetting.GetEmailSetting(messageTemplate);
    //        emailSettings.MessageBodyAr = emailSettings.MessageBodyAr.Replace("@FullName", toName);
    //        emailSettings.MessageBodyAr = emailSettings.MessageBodyAr.Replace("@code", "Code=" + code);

    //        return emailSettings;
    //    }
    //    private ApplicationUser GetApplicationUser(string name, string phone, string email, string userName, string role = "user", bool isApprove = false, bool isSelfAdded = false, string status = "New")
    //    {
    //        ApplicationUser user = new ApplicationUser
    //        {
    //            UserName = userName,
    //            PhoneNumber = phone,
    //            Email = email,
    //            Name = name
    //        };
    //        return user;
    //    }

    //    private async Task AddRoleToUser(ApplicationUser user)
    //    {
    //        await _userManager.AddToRoleAsync(user.Id, "");
    //    }

    //    private async Task SendEmailConfirmation(ApplicationUser user)
    //    {

    //        var code = await GenerateToken(user.Id, EmailType.EmailConfirmation);
    //        await SendEmail(user, code, EmailType.EmailConfirmation.GetHashCode().ToString());
    //    }
    //    private async Task SendPasswordResetToken(ApplicationUser user)
    //    {
    //        var code = await GenerateToken(user.Id, EmailType.ForgetPassword);
    //        await SendEmail(user, code, EmailType.ForgetPassword.GetHashCode().ToString());
    //    }

    //    private async Task<string> GenerateToken(string userId, EmailType emailType)
    //    {
    //        switch (emailType)
    //        {
    //            case EmailType.EmailConfirmation:
    //                return await _userManager.GenerateEmailConfirmationTokenAsync(userId);
    //            case EmailType.ForgetPassword:
    //                return await _userManager.GeneratePasswordResetTokenAsync(userId);
    //            default:
    //                return "";
    //        }

    //    }

    //    private async Task SendEmail(ApplicationUser user, string code, string messageTemplate)
    //    {
    //        EmailSetting emailMessage = GetEmailMessage(user.Name, code, messageTemplate);
    //        await _userManager.SendEmailAsync(user.Id, messageTemplate, emailMessage.MessageBodyAr);
    //    }

    //    public async Task<IdentityResult> ChangePassword(string userId, string oldPassword, string newPassword)
    //    {
    //        IdentityResult result = await _userManager.ChangePasswordAsync(userId, oldPassword, newPassword);
    //        return result;
    //    }


    //    #endregion

    //    #region token

    //    public Client FindClient(string clientId)
    //    {
    //        var client = _context.Clients.Find(clientId);

    //        return client;
    //    }

    //    public async Task<bool> AddRefreshToken(RefreshToken token)
    //    {

    //        var existingToken =
    //            _context.RefreshTokens
    //                .SingleOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);

    //        if (existingToken != null)
    //        {
    //            var result = await RemoveRefreshToken(existingToken);
    //        }

    //        _context.RefreshTokens.Add(token);

    //        return await _context.SaveChangesAsync() > 0;
    //    }

    //    public async Task<bool> RemoveRefreshToken(string refreshTokenId)
    //    {
    //        var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);

    //        if (refreshToken != null)
    //        {
    //            _context.RefreshTokens.Remove(refreshToken);
    //            return await _context.SaveChangesAsync() > 0;
    //        }

    //        return false;
    //    }

    //    public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
    //    {
    //        _context.RefreshTokens.Remove(refreshToken);
    //        return await _context.SaveChangesAsync() > 0;
    //    }

    //    public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
    //    {
    //        var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);

    //        return refreshToken;
    //    }

    //    public List<RefreshToken> GetAllRefreshTokens()
    //    {
    //        return _context.RefreshTokens.ToList();
    //    }

    //    #endregion

    //    #region Soical
    //    public async Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo)
    //    {
    //        ApplicationUser user = await _userManager.FindAsync(loginInfo);

    //        return user;
    //    }

    //    public async Task<IdentityResult> CreateAsync(ApplicationUser user)
    //    {
    //        var result = await _userManager.CreateAsync(user);

    //        return result;
    //    }

    //    public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
    //    {
    //        var result = await _userManager.AddLoginAsync(userId, login);

    //        return result;
    //    }
    //    #endregion

    //    //public void Dispose()
    //    //{
    //    //    _context.Dispose();
    //    //    _userManager.Dispose();
    //    //    GC.SuppressFinalize(this);
    //    //}
    //    public void Dispose()
    //    {
    //        this.Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (!disposing)
    //        {
    //            return;
    //        }

    //        if (this._context == null)
    //        {
    //            return;
    //        }

    //        this._context.Dispose();
    //        this._userManager.Dispose();
    //        this._context = null;
    //    }

    //    public async Task<ApplicationUser> FindUserByUserId(string id)
    //    {
    //        ApplicationUser user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == id);
    //        return user;
    //    }

    //    public async Task ReSendEmailConfirmation(ApplicationUser user)
    //    {
    //        var code = await GenerateToken(user.Id, EmailType.EmailConfirmation);
    //        await SendEmail(user, code, EmailType.EmailConfirmation.GetHashCode().ToString());
    //    }
    //}
}