using Saned.ArousQatar.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.ArousQatar.Data.Persistence.EntityConfigurations
{
    class AdvertismentConfigurations : EntityTypeConfiguration<Advertisment>
    {
        public AdvertismentConfigurations()
        {
            Property(x => x.ApplicationUserId).IsRequired();
            Property(x => x.CategoryId).IsRequired();
            Property(x => x.CreateDate).IsRequired();
            Property(x => x.NumberOfLikes).IsRequired();
            Property(x => x.NumberOfViews).IsRequired();
           // Property(x => x.StartDate).IsRequired();


           
        }
    }
}
