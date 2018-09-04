using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Core.Models;
using System;
using Saned.ArousQatar.Data.Persistence.Repositories;
using UtiltyManagemnt;

namespace Saned.ArousQatar.Data.Persistence.Tools
{
    public class EmailManager : IDisposable
    {
        readonly ApplicationDbContext _context;

        public EmailManager()
        {
            _context = new ApplicationDbContext();

        }

        public string SendActivationEmail(string messageTamplate, string toEmail, string messageBodyAr)
        {
            string result = "";

            EmailSettingRepository emailSettingRepository = new EmailSettingRepository();
            EmailSetting emailSettings = emailSettingRepository.GetEmailSetting(messageTamplate);

            result = Utilty.SendMail(emailSettings.Host, emailSettings.FromEmail, emailSettings.Password, toEmail, emailSettings.SubjectAr, messageBodyAr, "");

            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources

            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
