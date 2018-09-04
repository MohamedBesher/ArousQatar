using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System.Linq;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public class EmailSettingRepository : IEmailSettingRepository
    {
        private readonly ApplicationDbContext _context;

        public EmailSettingRepository()
        {
            _context = new ApplicationDbContext();
        }
        public EmailSetting GetEmailSetting(string type)
        {
            return _context.EmailSettings.SingleOrDefault(u => u.EmailSettingType == type.Trim());
        }

        //public EmailSettingRepository(IDbFactory dbFactory) : base(dbFactory)
        //{

        //}
        ////public EmailSettingRepository(IApplicationDbContext context)
        ////{
        ////    _context = context;
        ////}

        //public EmailSetting GetEmailSetting(string type)
        //{
        //    return FindBy(u => u.EmailSettingType == type.Trim()).FirstOrDefault();
        //}
    }
}