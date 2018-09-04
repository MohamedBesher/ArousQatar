using Saned.ArousQatar.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.ArousQatar.Data.Persistence.EntityConfigurations
{
    class ContactTypeConfigurations : EntityTypeConfiguration<ContactType>
    {
        public ContactTypeConfigurations()
        {
            Property(x => x.Type).IsRequired().HasMaxLength(50);
        }
    }
}
