namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createAdPriceGetAllSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("AdvertisementPrice_GetAll",
            @"SELECT
    		AdvertismentPrices.Id ,
    		AdvertismentPrices.[Period],
    		AdvertismentPrices.[Price],
    		COUNT(AdvertismentPrices.Id) OVER() AS 'OverAllCount'  
    	    FROM
    	    AdvertismentPrices 
            Where 
    		AdvertismentPrices.IsArchieved <> 1
            ORDER BY AdvertismentPrices.Id
    		");
        }
        
        public override void Down()
        {
            DropStoredProcedure("AdvertisementPrice_GetAll");
        }
    }
}
