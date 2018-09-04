using System;

namespace Saned.ArousQatar.Data.Core.Dtos
{
    public class AdvertisementListDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int OverallCount { get; set; }
    }

    public class AdvertisementDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsPaided { get; set; } = false;
        public string Description { get; set; }
        public int? NumberOfViews { get; set; }
        public int? NumberOfLikes { get; set; }
        public int? Comments { get; set; }
        public decimal? PaidEdPrice { get; set; }
        public bool? IsArchieved { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string AdvertisementPeriod { get; set; }
        public decimal? AdvertisementPrice { get; set; }
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        public  string ImageUrl { get; set; }
        public string UserName { get; set; }
        public string ApplicationUserId { get; set; }
        public int? AdvertismentPriceId { get; set; }
        public int OverAllCount { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsOwner { get; set; } = false;
        public bool IsExpired { get; set; } =true;
        public decimal Cost { get; set; }


        public bool IsActive { get; set; }
    }
}
