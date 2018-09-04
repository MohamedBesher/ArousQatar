namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CategoryCount : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("CategoryCount",
                p => new { },
                @"
                    select count (*)from Categories
                    where Categories.IsArchieved = 0
                 ");
        }

        public override void Down()
        {
            DropStoredProcedure("CategoryCount");
        }
    }
}
