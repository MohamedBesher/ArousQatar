namespace Saned.ArousQatar.Data.Core.Models
{
    public class Complaint : IEntityBase
    {
        public Complaint()
        {
            IsArchieved = false;
        }
        public int Id { get; set; }
        public string Message { get; set; }
        public bool? IsArchieved { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int? AdvertismentId { get; set; }
        public virtual Advertisment Advertisment { get; set; }

        public string ComplainedId { get; set; }
        public ApplicationUser Complained { get; set; }

        public void Archieve()
        {
            IsArchieved = true;
        }


        public void Modify(string message, string applicationUserId, int? advertismentId, string complainedId)
        {
            Message = message;
            ApplicationUserId = applicationUserId;
            AdvertismentId = advertismentId;
            ComplainedId = complainedId;
        }
    }
}
