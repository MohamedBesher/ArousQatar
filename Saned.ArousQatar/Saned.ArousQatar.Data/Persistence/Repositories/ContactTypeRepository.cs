using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public class ContactTypeRepository : EntityBaseRepository<ContactType>, IContactTypeRepository
    {
        public ContactTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public async  Task<List<ContactType>> GetAllAsync()
        {
            var res = (await DbContext.Database.SqlQuery<ContactType>("EXEC ContactTypeAll").ToListAsync());
            return res;
        }

        public async Task<List<ContactType>> GetAllAsync(int start = 0, int number = 10)
        {
            return (await GetAllAsync("ContactType", start, number));
        }

        public async Task<ContactType> GetSingleAsync(int id)
        {
            return (await GetSingleAsync("ContactType", id));
        }

        public async Task<bool> IsRelatedToContactInformation(int typeId)
        {
            SqlParameter id = new SqlParameter("id", SqlDbType.Int) { Value = typeId };
            return
                (await
                    DbContext.Database.SqlQuery<int>("ContactTypeIsRelatedToContactInformation", id)
                        .FirstOrDefaultAsync()) > 0;
        }
    }
}
