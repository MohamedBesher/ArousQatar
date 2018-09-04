using System;

namespace Saned.ArousQatar.Data.Core.Dtos
{
    public class ContactUsMessageDto

    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool IsArchieved { get; set; }
        public DateTime CreatedDate { get;  set; }
        public int OverAllCount { get;  set; }

    }
}
