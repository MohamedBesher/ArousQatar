namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdvertisementPriceGetSingle : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("AdvertisementPriceGetSingle"
                , p => new
                {
                    id = p.Int()
                },
                @"
                    select * from AdvertismentPrices where AdvertismentPrices.Id = @id
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("AdvertisementPriceGetSingle");
        }
    }
}
