using Saned.ArousQatar.Data.Core.Models;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IEmailSettingRepository
    {
        EmailSetting GetEmailSetting(string type);
    }
}