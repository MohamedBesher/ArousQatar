namespace Saned.ArousQatar.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdvertisementPriceGetAll : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "AdvertisementPriceGetAll",
                p => new
                {
                    index = p.Int(),
                    rowNumber = p.Int(),
                    filter = p.String(100, defaultValue: ""),
                    IsArchieved = p.Boolean(defaultValueSql: "NULL")
                },
                @"
                    SELECT
                		AdvertismentPrices.Id ,
                		AdvertismentPrices.[Period],
                		AdvertismentPrices.[Price],
                		COUNT(AdvertismentPrices.Id) OVER() AS 'OverAllCount'  
                	FROM
                	    AdvertismentPrices 
                    Where 
                		AdvertismentPrices.IsArchieved = ISNULL(@IsArchieved , AdvertismentPrices.IsArchieved) 
                	and 
                		(AdvertismentPrices.Period like '%'+@filter+'%' or 
                		 AdvertismentPrices.Price like '%'+@filter+'%')
                    ORDER BY
                		 Id OFFSET @index ROWS FETCH NEXT @rowNumber ROWS ONLY
                ");

        }

        public override void Down()
        {
            DropStoredProcedure("AdvertisementPriceGetAll");
        }
    }
}
