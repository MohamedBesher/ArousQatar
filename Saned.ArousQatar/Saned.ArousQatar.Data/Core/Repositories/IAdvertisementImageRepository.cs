using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IAdvertisementImageRepository : IEntityBaseRepository<AdvertismentImage>
    {
        Task<List<AdvertismentImage>> GetAllAsync(int id);

        Task<AdvertismentImage> GetSingleAsync(int id);
        Task<AdvertismentImage> GetImageById(int id);

        void ResetMainImage(int advertismentId);
        void RemoveImages(List<AdvertismentImage> images);

        AdvertismentImage GetimagebyAdId(int adId);
    }
}
