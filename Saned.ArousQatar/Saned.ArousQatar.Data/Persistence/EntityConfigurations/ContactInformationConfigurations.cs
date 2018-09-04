using Saned.ArousQatar.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.ArousQatar.Data.Persistence.EntityConfigurations
{
    class ContactInformationConfigurations : EntityTypeConfiguration<ContactInformation>
    {
        public ContactInformationConfigurations()
        {
            Property(x => x.Contact).IsRequired().HasMaxLength(200);
            Property(x => x.IconName).IsRequired().HasMaxLength(70);
            Property(x => x.ContactTypeId).IsRequired();
        }
    }
}
