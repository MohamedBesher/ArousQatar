using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public class ComplaintRepository : EntityBaseRepository<Complaint>, IComplaintRepository
    {
        public ComplaintRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<List<Complaint>> GetAllAsync(int start = 0, int number = 10)
        {
            return (await GetAllAsync("Complaint", start, number));
        }

        public async Task<List<ComplaintDto>> GetAllUsersComplaintsAsync(int start, int number)
        {
            return (await DbContext.Database.SqlQuery<ComplaintDto>("ComplaintUserGetAll").ToListAsync());
        }

        public async Task<List<ComplaintDto>> GetAllAdvertisementComplaintsAsync(int index, int rowNumber, string filter)
        {
            var indexParamter = new SqlParameter("index", index);
            var rowNumberParamter = new SqlParameter("rowNumber", rowNumber);
            var filterParamter = new SqlParameter("filter", filter);
            return (await DbContext.Database.SqlQuery<ComplaintDto>("ComplaintAdvertisementGetAll  @index,@rowNumber,@filter",
                indexParamter, rowNumberParamter, filterParamter).ToListAsync());
        }



        public async Task<List<ComplaintDto>> GetAllUsersComplaintsArchieveAsync(int start, int number)
        {
            return (await DbContext.Database.SqlQuery<ComplaintDto>("ComplaintUserArchieveGetAll").ToListAsync());
        }

        public async Task<List<ComplaintDto>> GetAllAdvertisementComplaintsArchieveAsync(int start, int number)
        {
            return (await DbContext.Database.SqlQuery<ComplaintDto>("ComplaintAdvertisementArchieveGetAll").ToListAsync());
        }

        public async Task<Complaint> GetSingleAsyncByUser(int id, string userId)
        {
            return await DbContext.Complaints.SingleOrDefaultAsync(l => l.AdvertismentId == id && l.ApplicationUserId == userId);
        }


        public async Task<Complaint> GetSingleAsync(int id)
        {
            return (await GetSingleAsync("Complaint", id));
        }


    }
}
