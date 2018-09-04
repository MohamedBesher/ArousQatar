using Saned.ArousQatar.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.ArousQatar.Data.Persistence.EntityConfigurations
{
    class AdvertismentImageConfigurations : EntityTypeConfiguration<AdvertismentImage>
    {
        public AdvertismentImageConfigurations()
        {
            Property(x => x.AdvertismentId).IsRequired();
            Property(x => x.ImageUrl).IsRequired().HasMaxLength(500);

        }
    }
}
