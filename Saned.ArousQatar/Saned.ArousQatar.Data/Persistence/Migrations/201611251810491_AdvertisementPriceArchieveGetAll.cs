namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdvertisementPriceArchieveGetAll : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "AdvertisementPriceArchieveGetAll",
                p => new
                {
                    index = p.Int(),
                    rowNumber = p.Int(),
                    filter = p.String(100, defaultValue: "")
                },
                @"
                    SELECT * FROM AdvertismentPrices 
                	Where AdvertismentPrices.IsArchieved = 1 and 
                        (AdvertismentPrices.Period like '%'+@filter+'%' or 
                           AdvertismentPrices.Price like '%'+@filter+'%')
                	ORDER BY Id OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY
                ");

        }

        public override void Down()
        {
            DropStoredProcedure("AdvertisementPriceArchieveGetAll");
        }
    }
}
