using Saned.ArousQatar.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.ArousQatar.Data.Persistence.EntityConfigurations
{
    class AdvertismentPriceConfigurations : EntityTypeConfiguration<AdvertismentPrice>
    {
        public AdvertismentPriceConfigurations()
        {
            Property(x => x.Period).IsRequired().HasMaxLength(200);
            Property(x => x.Price).IsRequired();
        }
    }
}
