namespace Saned.ArousQatar.Data.Core.Models
{
    public class Like : IEntityBase
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int AdvertismentId { get; set; }
        public virtual Advertisment Advertisment { get; set; }
    }
}
