namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ContactInformationGetAll : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("ContactInformationGetAll", p => new
            {
                index = p.Int(),
                rowNumber = p.Int(),
                filter = p.String(100, defaultValue: "")
            },
               @"
                    SELECT * FROM ContactInformations
                    where 
                         ContactInformations.Contact like '%'+@filter+'%' 
                	ORDER BY Id OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("ContactInformationGetAll");
        }
    }
}
