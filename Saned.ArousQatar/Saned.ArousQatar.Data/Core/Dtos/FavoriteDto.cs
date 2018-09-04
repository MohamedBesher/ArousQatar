namespace Saned.ArousQatar.Data.Core.Dtos
{
    public class FavoriteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int NumberOfViews { get; set; }
        public int NumberOfLikes { get; set; }
        public int Comments { get; set; }
        public string ImageUrl { get; set; }
        public int OverAllCount { get; set; }
        public int AdvertismentId { get; set; }
        public bool IsExpired { get; set; } = true;
        public bool? IsPaided { get; set; } = false;

    }
}
