using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using System.Data;
using System.Linq;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public class ContactInformationRepository : EntityBaseRepository<ContactInformation>, IContactInformationRepository
    {
        public ContactInformationRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        //public async Task<List<ContactInformation>> GetAllAsync(int start = 0, int number = 10)
        //{
        //    return (await GetAllAsync("ContactInformation", start, number));
        //}

        public async Task<List<ContactInformationDto>> GetAllAsync(int PageNumber = 1, int PageSize = 10, string Filter = null)
        {
           // Filter = Filter.Trim();
            var res = (await DbContext.Database.SqlQuery<ContactInformationDto>("EXEC ContactInformationGetAll @PageNumber, @PageSize, @Filter",
                 new SqlParameter("PageNumber", SqlDbType.Int) { Value = PageNumber },
                 new SqlParameter("PageSize", SqlDbType.Int) { Value = PageSize },
                 Getparamter(Filter, "Filter")).ToListAsync());
            return res;
        }

        public async Task<ContactInformation> GetSingleAsync(int id)
        {
            return (await GetSingleAsync("ContactInformation", id));
        }

        public List<string> GetAllWithTypes(int id)
        {
            return DbContext.ContactInformations.Where(u=>u.ContactTypeId==id).Select(u=>u.Contact).ToList();
        }
    }
}
