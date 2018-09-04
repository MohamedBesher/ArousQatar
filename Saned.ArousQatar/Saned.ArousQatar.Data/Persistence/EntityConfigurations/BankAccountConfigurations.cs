using Saned.ArousQatar.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.ArousQatar.Data.Persistence.EntityConfigurations
{
    class BankAccountConfigurations : EntityTypeConfiguration<BankAccount>
    {
        public BankAccountConfigurations()
        {
            Property(x => x.BankNumber).IsRequired().HasMaxLength(200);
            Property(x => x.BankName).IsRequired().HasMaxLength(100);
        }
    }
}
