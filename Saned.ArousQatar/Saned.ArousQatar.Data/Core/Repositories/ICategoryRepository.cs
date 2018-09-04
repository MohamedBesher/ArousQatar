using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface ICategoryRepository : IEntityBaseRepository<Category>
    {
        Task<List<CategoryAllDto>> GetAllAsync(int start = 0, int number = 10, string filter = "", bool? isArchieved = null);
        Task<List<CategoryDto>> GetAllNameAsync(bool? isArchieved = null);
        Task<Category> GetSingleAsync(int id);
        Task<int> CountCategoryAsync();
        Task<bool> IsRelatedWithAdvertisements(int catId);
        Task<int> CountArchieveAsync();

        Task<List<CategoryListDto>> GetPaginAsync(int pageNumber = 1, int? pageSize = null, string filter = null,
            bool? isDelete = null);
    }
}