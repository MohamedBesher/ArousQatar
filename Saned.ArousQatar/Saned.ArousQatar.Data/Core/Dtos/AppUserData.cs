using System;

namespace Saned.ArousQatar.Data.Core.Dtos
{
    public class AppUserData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Shelter { get; set; }
        public int OverallCount { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Gender { get; set; }
        public string PhotoUrl { get; set; }
        public bool? IsApprove { get; set; }
        public bool? EmailConfirmed { get; set; }
        public bool? IsDeleted { get; set; }
        public string LoginProvider { get; set; }
        public bool? IsSoicalLogin { get; set; }
        public int? AnimalCount { get; set; }
        public int? ReportFromCount { get; set; }
        public int? ReportTo { get; set; }
        public int? ChatSenderCount { get; set; }
        public int? ChatRecieverCount { get; set; }
        public int? CommentCount { get; set; }
        public int? GiveUpRequestsCount { get; set; }
        public int? SucessStoryCount { get; set; }
        public int? VolunteerCount { get; set; }
        public int? MessagesCount { get; set; }

    }

    public class UserData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
        public bool? Gender { get; set; }
        public int? Age { get; set; }
        public int? CityId { get; set; }
        public int OverAllCount { get; set; }
        public string UserName { get; set; }
        public bool IsSoicalLogin { get; set; }

        public string SocialText { get { return IsSoicalLogin ? "نعم" : "لا"; } }
    }
}