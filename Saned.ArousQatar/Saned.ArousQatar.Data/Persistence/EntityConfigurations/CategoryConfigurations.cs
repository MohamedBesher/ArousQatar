using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saned.ArousQatar.Data.Core.Models;

namespace Saned.ArousQatar.Data.Persistence.EntityConfigurations
{
    public class CategoryConfigurations : EntityTypeConfiguration<Category>
    {
        public CategoryConfigurations()
        {
            Property(u => u.Name).
               IsRequired()
               .HasMaxLength(150);

            Property(u => u.IconName).
                       IsRequired()
                       .HasMaxLength(50);

        }
    }
}
