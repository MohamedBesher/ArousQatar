using System;

namespace Saned.ArousQatar.Api.Models
{
    public class AdvertisementSmallViewModel
    {
        public int Id { get; set; }
        public int NumberOfViews { get; set; }
        public int NumberOfLikes { get; set; }
        public int Comments { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
    }

    public class MyAdvertisementViewModel : AdvertisementSmallViewModel
    {
        public bool IsExpired { get; set; } = true;
        public bool IsActive { get; set; }
        public bool? IsPaided { get; set; } = false;
        public decimal? PaidEdPrice { get; set; }
        public int? AdvertismentPriceId { get; set; }
        public decimal? AdvertisementPrice { get; set; }
        public string AdvertisementPeriod { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}