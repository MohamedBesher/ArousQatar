using Saned.ArousQatar.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.ArousQatar.Data.Persistence.EntityConfigurations
{
    class ApplicationUserConfigurations : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfigurations()
        {
            Property(x => x.Name).IsRequired().HasMaxLength(100);

            HasMany(a => a.Comments).WithRequired(c => c.ApplicationUser).WillCascadeOnDelete(false);
            HasMany(a => a.Favorites).WithRequired(c => c.ApplicationUser).WillCascadeOnDelete(false);
            HasMany(a => a.Likes).WithRequired(c => c.ApplicationUser).WillCascadeOnDelete(false);
            HasMany(a => a.Complaints).WithRequired(c => c.ApplicationUser).WillCascadeOnDelete(false);
        }
    }
}
