using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Models
{
   public class ContactUsMessage : IEntityBase
    {
        public ContactUsMessage()
        {         
            IsArchieved = false;
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool IsArchieved { get; set; }
        public DateTime CreatedDate { get; private set; }

        public void Archieve()
        {
            IsArchieved = true;
        }

        public void Modify(string name,string phone, string email, string message)
        {
            Name = name;
            Phone = phone;
            Email = email;
            Message = message;
        }
    }
}
