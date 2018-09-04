namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdvertisementPriceIsRelatedWithAdvertisement : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "AdvertisementPriceIsRelatedWithAdvertisement",
                p => new
                {
                    id = p.Int()
                },
                @"
                    select count(*) from Advertisments
                    where Advertisments.AdvertismentPriceId = @id
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("AdvertisementPriceIsRelatedWithAdvertisement");
        }
    }
}
