namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SPGetSingleCategory : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
               "dbo.SPGetSingleCategory",
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
            DropStoredProcedure("dbo.SPGetSingleCategory");
        }
    }
}
