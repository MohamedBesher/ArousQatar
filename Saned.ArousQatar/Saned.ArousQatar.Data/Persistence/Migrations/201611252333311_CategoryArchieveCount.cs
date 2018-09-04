namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CategoryArchieveCount : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "CategoryArchieveCount",
                p => new { },
                @"
                    select count(*) from Categories where 
                    Categories.IsArchieved = 1
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("CategoryArchieveCount");
        }
    }
}
