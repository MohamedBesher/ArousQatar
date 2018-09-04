namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SPGetAllCategories : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "dbo.SPGetAllCategory",
                p => new { },
                body: @"
						SELECT * from Categories 
						where Categories.IsArchieved != 1
						"
                );

        }

        public override void Down()
        {
            DropStoredProcedure("dbo.SPGetAllCategory");
        }
    }
}
