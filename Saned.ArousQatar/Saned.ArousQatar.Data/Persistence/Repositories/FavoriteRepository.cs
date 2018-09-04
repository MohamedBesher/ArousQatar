using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public class FavoriteRepository : EntityBaseRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<List<FavoriteDto>> GetAllAsync(string username, int start = 0, int number = 10)
        {
            return (await DbContext.Database.SqlQuery<FavoriteDto>("EXEC FavoriteGetAll @username , @index , @rowNumber"
                , new SqlParameter("username", SqlDbType.NVarChar) { Value = username },
                new SqlParameter("index", SqlDbType.Int) { Value = start },
                new SqlParameter("rowNumber", SqlDbType.Int) { Value = number }
                ).ToListAsync());
        }

        public async Task<FavoriteDto> GetSingleAsync(int id)
        {
            return
                (await
                    DbContext.Database.SqlQuery<FavoriteDto>("FavoriteGetSingle @id",
                        new SqlParameter("id", SqlDbType.Int) { Value = id }).FirstOrDefaultAsync());
        }

        public async Task<Favorite> GetFavoriteAsync(int id)
        {
            return (await
                        DbContext.Database.SqlQuery<Favorite>("FavoriteGetOne @id",
                        new SqlParameter("id", SqlDbType.Int) { Value = id }).FirstOrDefaultAsync());
        }

        public async Task<Favorite> GetSingleAsyncByUser(int id, string userId)
        {
            return await DbContext.Favorites.SingleOrDefaultAsync(l => l.AdvertismentId == id && l.ApplicationUserId == userId);

        }
    }
}
