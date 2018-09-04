using System.Collections.Generic;

namespace Saned.ArousQatar.Data.Core.Models
{
    public class ContactType : IEntityBase
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<ContactInformation> ContactInformations { get; set; }

        public void Modify(string type)
        {
            Type = type;
        }
    }
}
