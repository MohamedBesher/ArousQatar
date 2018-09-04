namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CategoryIsRelatedWithAdvertisements : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "CategoryIsRelatedWithAdvertisements",
                p => new
                {
                    id = p.Int()
                },
                @"
                    select count(*) from Advertisments
                    where Advertisments.CategoryId = @id
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("CategoryIsRelatedWithAdvertisements");
        }
    }
}
