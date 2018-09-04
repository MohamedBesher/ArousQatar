using Saned.ArousQatar.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.ArousQatar.Data.Persistence.EntityConfigurations
{
    class LikeConfigurations : EntityTypeConfiguration<Like>
    {
        public LikeConfigurations()
        {
            Property(x => x.AdvertismentId).IsRequired();
            Property(x => x.ApplicationUserId).IsRequired();
        }
    }
}
