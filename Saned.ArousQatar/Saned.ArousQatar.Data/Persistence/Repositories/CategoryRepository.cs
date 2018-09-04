using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public class CategoryRepository : EntityBaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<List<CategoryDto>> GetAllNameAsync(bool? isArchieved = null)
        {
            return (await DbContext.Database.SqlQuery<CategoryDto>("EXEC [CategoryGetName]").ToListAsync());

        }
        public async Task<List<CategoryListDto>> GetPaginAsync(int pageNumber = 1, int? pageSize = null, string filter = null, bool? isDelete = null)
        {
            return (await DbContext.Database.SqlQuery<CategoryListDto>
                ("Categories_SelectAllPaging @PageNumber,@PageSize,@Filter,@IsArchieved",
                    new SqlParameter("@PageNumber", pageNumber),
                    Getparamter(pageSize, "PageSize"),
                    Getparamter(filter, "Filter"),
                    Getparamter(isDelete, "IsArchieved")
                ).ToListAsync());
        }
       
        public async Task<Category> GetSingleAsync(int id)
        {
            return (await GetSingleAsync("Category", id));
        }

        public async Task<int> CountCategoryAsync()
        {
            return (await DbContext.Database.SqlQuery<int>("CategoryCount").FirstOrDefaultAsync());
        }

        public async Task<bool> IsRelatedWithAdvertisements(int catId)
        {
            SqlParameter id = new SqlParameter("id", SqlDbType.NVarChar) { Value = catId };
            return (await DbContext.Database.SqlQuery<int>("CategoryIsRelatedWithAdvertisements @id", id).FirstOrDefaultAsync()) > 0;
        }
        
        public async Task<List<CategoryAllDto>> GetAllAsync(int start = 0, int number = 10, string filter = "", bool? isArchieved = null)
        {
            filter = filter.Trim();
            var res = (await DbContext.Database.SqlQuery<CategoryAllDto>("EXEC [dbo].[CategoryGetAll] @index, @rowNumber, @filter , @IsArchieved",
                 new SqlParameter("index", SqlDbType.Int) { Value = start },
                 new SqlParameter("rowNumber", SqlDbType.Int) { Value = number },
                 new SqlParameter("filter", SqlDbType.NVarChar) { Value = filter },
                 new SqlParameter("IsArchieved", SqlDbType.Bit) { Value = isArchieved }).ToListAsync())
                 .ToList();

            return res;
        }

        public async Task<int> CountArchieveAsync()
        {
            return (await DbContext.Database.SqlQuery<int>("CategoryArchieveCount").FirstOrDefaultAsync());
        }
    }
}
