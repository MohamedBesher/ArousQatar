namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class adding_stored_proc_ : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "dbo.CategoryGetAll",
                p => new { },
                body: @"
						SELECT * from Categories 
						where Categories.IsArchieved != 1
						"
                );

            CreateStoredProcedure(
               "dbo.CategoryGetSingle",
               p => new
               {
                   Id = p.Int()
               },
               body: @"
						SELECT * from Categories 
						where Categories.Id = @Id
						"
               );
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.CategoryGetAll");
            DropStoredProcedure("dbo.CategoryGetSingle");
        }
    }
}
