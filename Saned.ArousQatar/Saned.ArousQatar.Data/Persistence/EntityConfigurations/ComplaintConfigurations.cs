using Saned.ArousQatar.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.ArousQatar.Data.Persistence.EntityConfigurations
{
    class ComplaintConfigurations : EntityTypeConfiguration<Complaint>
    {
        public ComplaintConfigurations()
        {
            //Property(x => x.AdvertismentId).IsRequired();
            Property(x => x.Message).IsRequired();
        }
    }
}
