namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class BankAccountGetSingle : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "BankAccountGetSingle",
                p => new
                {
                    id = p.Int()
                },
                @"
                    select * from BankAccounts 
                    where BankAccounts.Id = @id
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("BankAccountGetSingle");
        }
    }
}
