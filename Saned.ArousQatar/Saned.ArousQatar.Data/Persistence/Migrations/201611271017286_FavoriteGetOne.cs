namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FavoriteGetOne : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("FavoriteGetOne", p => new { id = p.Int() },
                @"
                    select * from Favorites  where Favorites.Id = @id
                    ");
        }

        public override void Down()
        {
            DropStoredProcedure("FavoriteGetOne");
        }
    }
}
