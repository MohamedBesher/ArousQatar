using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IAdvertisementRepository : IEntityBaseRepository<Advertisment>
    {
        Task<List<AdvertisementDto>> GetAllAsync(int start = 0, int number = 10, string filter = "", bool? isArchieve = null, int? categoryId = null, bool? isPaided = null, string applicationUserId = null, bool? isExpired = null);
        Task<List<Dtos.AdvertisementDto>> GetAllArchiveAsync(int start = 0, int number = 10, string filter = "", bool? isArchieve = null);
        Task<AdvertisementDto> GetSingleDtoAsync(int id);
        Task<Advertisment> GetSingleAsync(int id);

        AdvertisementDto GetSingleData(int id);


        Task<int> CountArchieveAsync();
        Task<List<AdvertisementSmallDto>> GetAllByCategoryId(int? catId = null, int start = 0, int number = 10, string filter = "", bool? isArchieve = null, bool? isActive = null);
        Task<List<AdvertisementListDto>> GetPaginAsync(int pageNumber = 1, int? pageSize = null, bool? isDelete = null, bool? isActive = null);
        Task<List<MyAdvertisementDto>> GetAllByUsername(string username, int start = 0, int number = 10,
            string filter = "", bool? isArchieve = null);
        Task<List<MyAdvertisementDto>> GetByUserId(string ApplicationUserId, int pageNumber = 1, int pageSize = 8, string filter = null, bool? isArchieve = null, bool? isActive = null);
        Task<int> DeleteFromAllTables(int id);
    }
}