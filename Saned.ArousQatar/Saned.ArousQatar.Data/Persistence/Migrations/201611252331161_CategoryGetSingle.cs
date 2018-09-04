namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CategoryGetSingle : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure(
                "CategoryGetSingle",
                p => new
                {
                    id = p.Int()
                },
                @"
                    select * from Categories 
                    where Categories.Id = @id
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("CategoryGetSingle");
        }
    }
}
