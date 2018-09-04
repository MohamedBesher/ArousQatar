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
    public class AdvertisementRepository : EntityBaseRepository<Advertisment>, IAdvertisementRepository
    {
        public AdvertisementRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<AdvertisementDto>> GetAllAsync(int start = 0, int number = 10, string filter = "",bool? isArchieve = null, int? categoryId = null, bool? isPaided = null, string applicationUserId = null, bool? isExpired = null)
        {
            List<AdvertisementDto> res;
            SqlParameter categoryIdParam, isPaidedParam, isExpiredParam, applicationUserIdParam, archieveparameter;
            filter = filter.Trim();
            var startPar = new SqlParameter("index", SqlDbType.Int) { Value = start };
            var numberPar = new SqlParameter("rowNumber", SqlDbType.Int) { Value = number };
            var filterPar = new SqlParameter("filter", SqlDbType.NVarChar) { Value = filter };


            if (categoryId.HasValue)
            {
                categoryIdParam = new SqlParameter("CategoryId", SqlDbType.Int) { Value = categoryId };
            }
            else
            {
                categoryIdParam = new SqlParameter("CategoryId", SqlDbType.Int) { Value = DBNull.Value };
            }

            if (isPaided.HasValue)
            {
                isPaidedParam = new SqlParameter("IsPaided", SqlDbType.Bit) { Value = isPaided };
            }
            else
            {
                isPaidedParam = new SqlParameter("IsPaided", SqlDbType.Bit) { Value = DBNull.Value };
            }

            if (isExpired.HasValue)
            {
                isExpiredParam = new SqlParameter("IsExpired", SqlDbType.Bit) { Value = isExpired };
            }
            else
            {
                isExpiredParam = new SqlParameter("IsExpired", SqlDbType.Bit) { Value = DBNull.Value };
            }

            if (!string.IsNullOrEmpty(applicationUserId))
            {
                applicationUserIdParam = new SqlParameter("UserId", SqlDbType.NVarChar) { Value = applicationUserId };
            }
            else
            {
                applicationUserIdParam = new SqlParameter("UserId", SqlDbType.NVarChar) { Value = DBNull.Value };
            }

            if (isArchieve.HasValue)
            {
                 archieveparameter = new SqlParameter("IsArchieved", SqlDbType.Bit) { Value = isArchieve };
            }
            else
            {
                 archieveparameter = new SqlParameter("IsArchieved", SqlDbType.Bit) { Value = DBNull.Value };
            }

           res = (await DbContext.Database.SqlQuery<AdvertisementDto>("[AdvertisementGetAll] @index, @rowNumber, @filter  , @IsArchieved , @CategoryId ,  @IsPaided , @UserId,@IsExpired",
                                   startPar, numberPar, filterPar, archieveparameter, categoryIdParam, isPaidedParam, applicationUserIdParam, isExpiredParam).ToListAsync());

            return res;
        }

        public async Task<List<AdvertisementDto>> GetAllArchiveAsync(int start = 0, int number = 10, string filter = "", bool? isArchieve = null)
        {
            return (await GetAllAsync<AdvertisementDto>("AdvertisementArchieve", start, number, filter));
        }

        public async Task<AdvertisementDto> GetSingleDtoAsync(int id)
        {
            return (await GetSingleAsync<AdvertisementDto>("Advertisement", id));
        }

        public async Task<Advertisment> GetSingleAsync(int id)
        {
            return (await GetSingleAsync("AdvertisementDelete", id));
        }
        /// <summary>
        /// Used In PayPal as GetSingleAsync Without await
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AdvertisementDto GetSingleData(int id)
        {

            return DbContext.Database.SqlQuery<AdvertisementDto>("EXEC AdvertisementGetSingle @id",
                         new SqlParameter("id", SqlDbType.Int) { Value = id }).FirstOrDefault();
         
        }

        public async Task<int> CountArchieveAsync()
        {
            return (await DbContext.Database.SqlQuery<int>("AdvertisementArchieveCount").FirstOrDefaultAsync());
        }

        public async Task<List<AdvertisementSmallDto>> GetAllByCategoryId(int? catId = null, int start = 0, int number = 10, string filter = "", bool? isArchieve = null, bool? isActive = null)
        {
            filter = filter.Trim();
            var startPar = new SqlParameter("index", SqlDbType.Int) { Value = start };
            var numberPar = new SqlParameter("rowNumber", SqlDbType.Int) { Value = number };
            var filterPar = new SqlParameter("filter", SqlDbType.NVarChar) { Value = filter };
            var Id = Getparamter(catId, "CategoryId");//, new SqlParameter("CategoryId", SqlDbType.Int) { Value = catId };
            var archieve = new SqlParameter("IsArchieved", SqlDbType.Bit) { Value = isArchieve };
            var isActiveParam = isActive.HasValue ?
                new SqlParameter("IsActive", isActive) :
                new SqlParameter("IsActive", DBNull.Value);
            return
                (await
                    DbContext.Database.SqlQuery<AdvertisementSmallDto>(
                        "[AdvertisementGetAllByCategoryId] @index ,@rowNumber ,@filter, @IsArchieved, @CategoryId , @IsActive",
                        startPar, numberPar, filterPar, archieve, Id , isActiveParam).ToListAsync());
        }

        public async Task<List<MyAdvertisementDto>> GetAllByUsername(string username, int start = 0, int number = 10, string filter = "", bool? isArchieve = null)
        {
            filter = filter.Trim();
            var startPar = new SqlParameter("index", SqlDbType.Int) { Value = start };
            var numberPar = new SqlParameter("rowNumber", SqlDbType.Int) { Value = number };
            var filterPar = new SqlParameter("filter", SqlDbType.NVarChar) { Value = filter };
            var usernamePar = new SqlParameter("username", SqlDbType.NVarChar) { Value = username };
            var archieve = new SqlParameter("IsArchieved", SqlDbType.Bit) { Value = isArchieve };

            return
                (await
                    DbContext.Database.SqlQuery<MyAdvertisementDto>(
                        "[AdvertisementGetByUsername] @username, @index ,@rowNumber ,@filter, @IsArchieved",
                        usernamePar, startPar, numberPar, filterPar, archieve).ToListAsync());
        }

        public async Task<List<AdvertisementListDto>> GetPaginAsync(int pageNumber = 1, int? pageSize = null, bool? isDelete = null, bool? isActive = null)
        {
            return (await DbContext.Database.SqlQuery<AdvertisementListDto>
                ("Advertisments_SelectAllPaging @PageNumber,@PageSize,@IsArchieved,@IsActive",
                    new SqlParameter("@PageNumber", pageNumber),
                    Getparamter(pageSize, "PageSize"),
                    Getparamter(isDelete, "IsArchieved"),
                    Getparamter(isActive, "IsActive")
                ).ToListAsync());
        }

        public async Task<List<MyAdvertisementDto>> GetByUserId(string applicationUserId,
            int pageNumber = 1, int pageSize = 8,
            string filter = null, bool? isArchieve = null, bool? isActive = null)
        {



            return
                (await
                    DbContext.Database.SqlQuery<MyAdvertisementDto>(
                        "[AdvertisementGetByUserId] @UserId, @PageNumber ,@PageSize ,@Filter, @IsArchieved , @IsActive",
                        new SqlParameter("UserId", SqlDbType.NVarChar) { Value = applicationUserId },
                        new SqlParameter("PageNumber", SqlDbType.Int) { Value = pageNumber },
                        new SqlParameter("PageSize", SqlDbType.Int) { Value = pageSize },
                        Getparamter(filter, "Filter"), Getparamter(isArchieve, "IsArchieved")
                        , Getparamter(isActive, "IsActive")).ToListAsync());
        }

        public async Task<int> DeleteFromAllTables(int id)
        {
            return (await DbContext.Database.SqlQuery<int>
               ("Advertisment_Delete @Id",
                   new SqlParameter("@Id", id)
               ).FirstOrDefaultAsync());
        }
    }
}