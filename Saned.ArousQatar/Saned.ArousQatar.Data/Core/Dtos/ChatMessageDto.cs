using System;

namespace Saned.ArousQatar.Data.Core.Dtos
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public string MessageContent { get; set; }
        public DateTime SentDate { get;  set; }
        public string SenderId { get; set; }
        public int ChatId { get; set; }
        public string UserName { get; set; }
        public int OverallCount { get; set; }
        public string FullDate { get; set; }
    }

    public class ChatListDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } 
        public string PhotoUrl { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverUserName { get; set; }
        public string ReceiverPhotoUrl { get; set; }
        public string MessageContent { get; set; }
        public DateTime SentDate { get; set; }
        public int OverallCount { get; set; }
        public string ReceiverName { get; set; }

    }
}