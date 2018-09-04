using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public class AdvertismentPriceRepository : EntityBaseRepository<AdvertismentPrice>, IAdvertisementPriceRepository
    {
        public AdvertismentPriceRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<AdvertisementPriceDto>> GetAllAsync()
        {
            var res = (await DbContext.Database.SqlQuery<AdvertisementPriceDto>("AdvertisementPrice_GetAll").ToListAsync());
            return res;
        }

        public async Task<List<AdvertisementPriceDto>> GetAllAsync(int start = 0, int number = 10, string filter = "", bool? isArchieve = null)
        {
            filter = filter.Trim();
            var startPar = new SqlParameter("index", SqlDbType.Int) { Value = start };
            var numberPar = new SqlParameter("rowNumber", SqlDbType.Int) { Value = number };
            var filterPar = new SqlParameter("filter", SqlDbType.NVarChar) { Value = filter };
            var archieve = new SqlParameter("IsArchieved", SqlDbType.Bit) { Value = isArchieve };

            return
                (await
                    DbContext.Database.SqlQuery<AdvertisementPriceDto>(
                        "[AdvertisementPriceGetAll]  @index ,@rowNumber ,@filter, @IsArchieved",
                         startPar, numberPar, filterPar, archieve).ToListAsync());
        }

        public async Task<AdvertismentPrice> GetSingleAsync(int id)
        {
            return (await GetSingleAsync("AdvertisementPrice", id));
        }


        public async Task<AdvertismentPrice> GetSingle(int? id)
        {
            return (DbContext.AdvertismentPrices.FirstOrDefault(u=>u.Id==id));
        }

        public async Task<bool> IsDuplicateDaysCount(int daysCount, int? advertisementPriceId = null)
        {
            SqlParameter advertisementPriceIdParam;
            var daysCountParam = new SqlParameter("DaysCount", daysCount);

            if (advertisementPriceId.HasValue)
            {
                advertisementPriceIdParam = new SqlParameter("AdvertisementPriceId", advertisementPriceId.Value);
            }
            else
            {
                advertisementPriceIdParam = new SqlParameter("AdvertisementPriceId", DBNull.Value);

            }
            return (await DbContext.Database.SqlQuery<int>("AdvertisementPriceIsDuplicateDaysCount @DaysCount,@AdvertisementPriceId"
    , daysCountParam, advertisementPriceIdParam).FirstOrDefaultAsync()) > 0;
        }

        public  bool IsDuplicateDaysCount(string daysCount, int? advertisementPriceId = null)
        {
            if (advertisementPriceId == null || advertisementPriceId==0)
                return DbContext.AdvertismentPrices.Any(u => u.Period == daysCount);
            else 
             return DbContext.AdvertismentPrices.Any(u => u.Period == daysCount && (advertisementPriceId == null || u.Id != advertisementPriceId.Value)) ;
        }
        public async Task<bool> IsRelatedWithAdvertisements(int advPriceId)
        {
            return (await DbContext.Database.SqlQuery<int>("AdvertisementPriceIsRelatedWithAdvertisement @id"
                , new SqlParameter("id", SqlDbType.Int) { Value = advPriceId }).FirstOrDefaultAsync()) > 0;
        }
    }
}
