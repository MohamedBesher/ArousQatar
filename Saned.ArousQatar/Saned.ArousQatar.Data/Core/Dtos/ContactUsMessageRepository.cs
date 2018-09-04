using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using Saned.ArousQatar.Data.Persistence.Repositories;

namespace Saned.ArousQatar.Data.Core.Dtos
{
    public class ContactUsMessageRepository : EntityBaseRepository<ContactUsMessage>, IContactUsMessageRepository
    {
        public ContactUsMessageRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<List<ContactUsMessageDto>> GetAllAsync(int pageNumber=1, int pageSize=10, string filter=null, bool? isArchieve=null)
        {
            return (await DbContext.Database.SqlQuery<ContactUsMessageDto>
            ("ContactUsMessage_GetAll @PageNumber,@PageSize,@Filter,@IsArchieved",
                new SqlParameter("@PageNumber", pageNumber),
                new SqlParameter("@PageSize", pageSize),
                Getparamter(filter, "Filter"),
                Getparamter(isArchieve, "IsArchieved")
            ).ToListAsync());
        }

        public Task<ContactUsMessage> GetSingleAsync(int id)
        {
            return DbContext.ContactUsMessage.FirstOrDefaultAsync(u=>u.Id==id);
        }
    }
}