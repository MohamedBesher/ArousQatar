using System;

namespace Saned.ArousQatar.Data.Core.Models
{
  
    public class ChatMessage : IEntityBase
    {
        public ChatMessage()
        {
           
 //DateTime.Now;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("Middle East Standard Time");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
            SentDate = now;
        }
        public int Id { get; set; }
        public string MessageContent { get; set; }
        public DateTime SentDate { get; private set; }
        public string SenderId { get; set; }
        public int ChatId { get; set; }



        public virtual ChatHeader Chat { get; set; }
        public ApplicationUser Sender { get; set; }



    }
  
}