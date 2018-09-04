namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class BankAccountGetAll : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
            "BankAccountGetAll",
                p => new
                {
                    index = p.Int(),
                    rowNumber = p.Int(),
                    filter = p.String(100, defaultValue: "")
                },
                @"
                    SELECT * FROM BankAccounts 
                	Where  
                        (BankAccounts.BankNumber like '%'+@filter+'%' or 
                         BankAccounts.BankName like '%'+@filter+'%')
                	ORDER BY Id OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("BankAccountGetAll");
        }
    }
}
