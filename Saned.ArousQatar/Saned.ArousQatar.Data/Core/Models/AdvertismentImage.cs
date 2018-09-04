using System;

namespace Saned.ArousQatar.Data.Core.Models
{
    public class AdvertismentImage : IEntityBase
    {
        public int Id { get; set; }
        public bool? IsMainImage { get; set; }
        public string ImageUrl { get; set; }

        public int AdvertismentId { get; set; }
        public virtual Advertisment Advertisment { get; set; }

        public void Modify(string imageUrl, int advertisementId)
        {
            ImageUrl = imageUrl;
            AdvertismentId = advertisementId;
        }

        public void SetAsMainImage()
        {
            IsMainImage = true;
        }

        public void Modify(string randomImage, int advId, bool isMainImage)
        {
            ImageUrl = randomImage;
            AdvertismentId = advId;
            IsMainImage = isMainImage;
        }
    }
}
