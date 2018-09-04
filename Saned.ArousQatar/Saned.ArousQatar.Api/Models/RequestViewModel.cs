using System;
using System.ComponentModel.DataAnnotations;

namespace Saned.ArousQatar.Api.Models
{
    public class MessageViewModel
    {
     
        [Required]
        public string MessageContent { get; set; }
       
        public string SenderId { get; set; }
        [Required]
        public int ChatId { get; set; }
    }

    public class ChatViewModel
    {
      
        public int AdvertismentId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string UserId { get; set; }
    }

    public class RequestViewModel
    {
        public int Id { get; set; }
        [Required]
        public int AdvertismentId { get; set; }
        public string RequestAuthorId { get; set; }
        public DateTime RequestDate { get; private set; } = DateTime.Now;
    }

    public class UpdateRequestViewModel
    {
        [Required]
        public int ChatId { get; set; }
     
        public decimal Price { get; set; }
    }
    public class MessagePagingViewModel
    {
        public int ChatId { get; set; } = 1;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public DateTime? LastSentDate { get; set; } = null;
    }

    public class ChatHeaderViewModel
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string UserOne { get; set; }
        public string PictureUserOne { get; set; }
        public string UserTwo { get; set; }
        public string PictureUserTwo { get; set; }
        public string UserOneId { get; set; }
        public string UserTwoId { get; set; }
        public DateTime CreateDate { get; set; }
        public int AdvertismentId { get; set; }
        public string RequestAuthorId { get; set; }   // Who Request Chat
    }
}