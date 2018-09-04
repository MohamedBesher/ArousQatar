using Saned.ArousQatar.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.ArousQatar.Data.Persistence.EntityConfigurations
{
    class CommentConfigurations : EntityTypeConfiguration<Comment>
    {
        public CommentConfigurations()
        {
            Property(x => x.AdvertismentId).IsRequired();
            Property(x => x.CreateDate).IsRequired();
            Property(x => x.Message).IsRequired();
            Property(x => x.ApplicationUserId).IsRequired();



        }
    }
}
