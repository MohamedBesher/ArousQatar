namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ContactTypeGetAll : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("ContactTypeGetAll", p => new
            {
                index = p.Int(),
                rowNumber = p.Int(),
                filter = p.String(100, defaultValue: "")
            },
               @"
                    SELECT * FROM ContactTypes 
                	ORDER BY Id OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("ContactTypeGetAll");
        }
    }
}
