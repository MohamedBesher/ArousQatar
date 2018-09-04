using System.Data.Entity.ModelConfiguration;
using Saned.ArousQatar.Data.Core.Models;

namespace Saned.ArousQatar.Data.Persistence.EntityConfigurations
{
    public class AdvertismentTransactionConfigurations : EntityTypeConfiguration<AdvertismentTransaction>
    {
        public AdvertismentTransactionConfigurations()
        {
            Property(x => x.AdvertismentId).IsRequired();
            Property(x => x.PaymentId).IsRequired();


        }
    }
}