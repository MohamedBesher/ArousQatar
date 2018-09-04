using Saned.ArousQatar.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.ArousQatar.Data.Persistence.EntityConfigurations
{
    class EntityBaseConfigurations<T> : EntityTypeConfiguration<T> where T : class, IEntityBase
    {
        public EntityBaseConfigurations()
        {
            HasKey(x => x.Id);
        }
    }
}
