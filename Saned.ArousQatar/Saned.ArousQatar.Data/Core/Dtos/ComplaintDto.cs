namespace Saned.ArousQatar.Data.Core.Dtos
{
    public class ComplaintDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool? IsArchieved { get; set; }

        public string ApplicationUserId { get; set; }

        public int? AdvertismentId { get; set; }

        public string ComplainedId { get; set; }

        public string CamplaintUser { get; set; }
        public string Name { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string ComplainedUser { get; set; }
        public string ComplainedEmail { get; set; }
        public string ComplainedName { get; set; }
        public string ComplainedPhoneNumber { get; set; }


        public string AdvertisementName { get; set; }
    }
}
