namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdvertisementPriceArchieveCount : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("AdvertisementPriceArchieveCount",
                p => new { },
                @"
                    select Count(AdvertismentPrices.Id) from AdvertismentPrices where IsArchieved = 1
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("AdvertisementPriceArchieveCount");
        }
    }
}
