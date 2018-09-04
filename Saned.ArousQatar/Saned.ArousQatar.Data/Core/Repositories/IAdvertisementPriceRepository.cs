using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IAdvertisementPriceRepository : IEntityBaseRepository<AdvertismentPrice>
    {
        Task<List<AdvertisementPriceDto>> GetAllAsync(int start = 0, int number = 10, string filter = "", bool? isArchieve = null);
        Task<List<AdvertisementPriceDto>> GetAllAsync();
        Task<AdvertismentPrice> GetSingleAsync(int id);
        Task<AdvertismentPrice> GetSingle(int? id);
        Task<bool> IsRelatedWithAdvertisements(int advPriceId);

        Task<bool> IsDuplicateDaysCount(int daysCount, int? advertisementPriceId = null);
         bool IsDuplicateDaysCount(string daysCount, int? advertisementPriceId = null);

    }
}
