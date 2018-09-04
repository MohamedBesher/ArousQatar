using System;

namespace Saned.ArousQatar.Data.Core.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
        public int AdvertismentId { get; set; }
        public string ApplicationUserId { get; set; }
        public string UserFirstName { get; set; }
        public int? CommentParentId { get; set; }
        public int OverAllCount { get; set; }
        public string AdvertismentName { get; set; }
        public string PhotoUrl { get; set; }
    }
}
