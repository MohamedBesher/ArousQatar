using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public class AdvertisementImageRepository : EntityBaseRepository<AdvertismentImage>, IAdvertisementImageRepository
    {
        public AdvertisementImageRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<AdvertismentImage>> GetAllAsync(int advId)
        {
            return await DbContext.AdvertismentImages.Where(u => u.AdvertismentId == advId).ToListAsync();
            //(await SqlQueryAsync("EXEC[dbo].[" + "AdvertismentImage" + "GetAll] @id"
            //, new SqlParameter("id", SqlDbType.Int) { Value = advId })).ToList();
        }

        public async Task<AdvertismentImage> GetSingleAsync(int id)
        {
            return (await GetSingleAsync("AdvertismentImage", id));
        }


        public AdvertismentImage GetimagebyAdId(int adId)
        {

            return
             (
                 DbContext.Database.SqlQuery<AdvertismentImage>("EXEC AdvertismentImagesByAdsId @AdId", new SqlParameter("AdId", SqlDbType.Int) { Value = adId }).FirstOrDefault());
           
        }

        public async void ResetMainImage(int advertismentId)
        {
            var advId = new SqlParameter("id", SqlDbType.Int) { Value = advertismentId };
            await DbContext.Database.SqlQuery<int>("AdvertismentImageResetMain @id", advId).FirstOrDefaultAsync();
        }

        public void RemoveImages(List<AdvertismentImage> images)
        {
            //List<AdvertismentImage> images =(await SqlQueryAsync("EXEC[dbo].[" + "AdvertismentImage" + "GetAll] @id"
            //     , new SqlParameter("id", SqlDbType.Int) { Value = advertismentId })).ToList();
            if (images.Count > 0)
                DbContext.AdvertismentImages.RemoveRange(images);


        }

        public Task<AdvertismentImage> GetImageById(int id)
        {
            return DbContext.AdvertismentImages.FirstOrDefaultAsync(u => u.Id== id);
        }
    }
}
