namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class adding_stored_proc : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure(
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
