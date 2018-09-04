using Saned.ArousQatar.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.ArousQatar.Data.Persistence.EntityConfigurations
{
    class FavoritesConfigurations : EntityTypeConfiguration<Favorite>
    {
        public FavoritesConfigurations()
        {
            Property(x => x.AdvertismentId).IsRequired();
            Property(x => x.ApplicationUserId).IsRequired();
        }
    }
}
